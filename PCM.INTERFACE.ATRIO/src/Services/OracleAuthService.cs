using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PCM.INTERFACE.ATRIO.Configuration;
using PCM.INTERFACE.ATRIO.Models;
using System.Net.Http.Headers;
using System.Text;

namespace PCM.INTERFACE.ATRIO.Services;

public interface IOracleAuthService
{
    Task<string> GetAccessTokenAsync(CancellationToken ct = default);
}

public class OracleAuthService : IOracleAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IPropertyContext _context;
    private readonly IOracleTokenCache _tokenCache;
    private readonly ILogger<OracleAuthService> _logger;

    public OracleAuthService(
        IHttpClientFactory httpClientFactory,
        IPropertyContext context,
        IOracleTokenCache tokenCache,
        ILogger<OracleAuthService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
        _tokenCache = tokenCache;
        _logger = logger;
    }

    public Task<string> GetAccessTokenAsync(CancellationToken ct = default)
    {
        var settings = _context.Current;

        // Token é cacheado por propriedade (credencial), permitindo N hotéis num só serviço.
        return _tokenCache.GetOrRefreshAsync(
            settings.TokenCacheKey,
            token => RequestNewTokenAsync(settings, token),
            ct);
    }

    private async Task<OracleTokenState> RequestNewTokenAsync(PropertySettings settings, CancellationToken ct)
    {
        _logger.LogInformation("Obtendo novo token Oracle Hospitality ({Property})...", settings.Label);

        using var client = _httpClientFactory.CreateClient("OracleAuth");

        var tokenUrl = $"{settings.BaseUrl.TrimEnd('/')}{settings.TokenEndpoint}";

        // Basic Auth com encoding ASCII (igual ao VB original)
        var credentials = $"{settings.ClientId}:{settings.ClientSecret}";
        var base64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));

        using var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl);

        // Headers — idênticos ao curl de referência
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64);
        request.Headers.Add("x-app-key", settings.AppKey);
        request.Headers.Add("enterpriseId", settings.EnterpriseId);

        // Body como string bruta (--data-urlencode do curl)
        var rawBody = $"grant_type=client_credentials&scope={Uri.EscapeDataString(settings.Scope)}";
        request.Content = new StringContent(rawBody, Encoding.UTF8, "application/x-www-form-urlencoded");

        _logger.LogDebug("=== Oracle Auth Request ({Property}) ===", settings.Label);
        _logger.LogDebug("URL: {Url}", tokenUrl);
        _logger.LogDebug("x-app-key: {AppKey}", settings.AppKey);
        _logger.LogDebug("enterpriseId: {EnterpriseId}", settings.EnterpriseId);
        _logger.LogDebug("Authorization: Basic {Preview}...", base64[..10]);
        _logger.LogDebug("Body (raw): {Body}", rawBody);
        _logger.LogDebug("==========================");

        var response = await client.SendAsync(request, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        _logger.LogDebug("Response HTTP {Status}: {Body}", (int)response.StatusCode, body);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Falha ao obter token ({Property}). HTTP {Status} | Body: {Body}",
                settings.Label, (int)response.StatusCode, body);
            throw new HttpRequestException(
                $"Oracle Auth ({settings.Label}) falhou com status {response.StatusCode}: {body}");
        }

        var tokenResponse = JsonConvert.DeserializeObject<OracleTokenResponse>(body)
            ?? throw new InvalidOperationException("Resposta de token inválida.");

        var state = new OracleTokenState
        {
            AccessToken = tokenResponse.AccessToken,
            ExpiresAt = DateTime.UtcNow.AddSeconds(
                tokenResponse.ExpiresIn - settings.TokenExpiryBufferSeconds)
        };

        _logger.LogInformation("Token obtido ({Property}). Expira em: {ExpiresAt:HH:mm:ss} UTC",
            settings.Label, state.ExpiresAt);

        return state;
    }
}
