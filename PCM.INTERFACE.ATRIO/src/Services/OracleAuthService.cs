using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    private readonly OracleHospitalitySettings _settings;
    private readonly ILogger<OracleAuthService> _logger;

    private OracleTokenState _tokenState = new();
    private readonly SemaphoreSlim _lock = new(1, 1);

    public OracleAuthService(
        IHttpClientFactory httpClientFactory,
        IOptions<OracleHospitalitySettings> options,
        ILogger<OracleAuthService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken ct = default)
    {
        if (_tokenState.IsValid())
            return _tokenState.AccessToken;

        await _lock.WaitAsync(ct);
        try
        {
            if (_tokenState.IsValid())
                return _tokenState.AccessToken;

            _logger.LogInformation("Obtendo novo token Oracle Hospitality...");
            var token = await RequestNewTokenAsync(ct);
            _tokenState = token;
            _logger.LogInformation("Token obtido. Expira em: {ExpiresAt:HH:mm:ss} UTC", _tokenState.ExpiresAt);
            return _tokenState.AccessToken;
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task<OracleTokenState> RequestNewTokenAsync(CancellationToken ct)
    {
        using var client = _httpClientFactory.CreateClient("OracleAuth");

        var tokenUrl = $"{_settings.BaseUrl.TrimEnd('/')}{_settings.TokenEndpoint}";

        // Basic Auth com encoding ASCII (igual ao VB original)
        var credentials = $"{_settings.ClientId}:{_settings.ClientSecret}";
        var base64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));

        using var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl);

        // Headers — idênticos ao curl de referência
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64);
        request.Headers.Add("x-app-key", _settings.AppKey);
        request.Headers.Add("enterpriseId", _settings.EnterpriseId);

        // Body como string bruta (--data-urlencode do curl)
        // Evita qualquer diferença de encoding do FormUrlEncodedContent
        var rawBody = $"grant_type=client_credentials&scope={Uri.EscapeDataString(_settings.Scope)}";
        request.Content = new StringContent(rawBody, Encoding.UTF8, "application/x-www-form-urlencoded");

        // Log completo para diagnóstico
        _logger.LogDebug("=== Oracle Auth Request ===");
        _logger.LogDebug("URL: {Url}", tokenUrl);
        _logger.LogDebug("x-app-key: {AppKey}", _settings.AppKey);
        _logger.LogDebug("enterpriseId: {EnterpriseId}", _settings.EnterpriseId);
        _logger.LogDebug("Authorization: Basic {Preview}...", base64[..10]);
        _logger.LogDebug("Body (raw): {Body}", rawBody);
        _logger.LogDebug("==========================");

        var response = await client.SendAsync(request, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        _logger.LogDebug("Response HTTP {Status}: {Body}", (int)response.StatusCode, body);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Falha ao obter token. HTTP {Status} | Body: {Body}",
                (int)response.StatusCode, body);
            throw new HttpRequestException(
                $"Oracle Auth falhou com status {response.StatusCode}: {body}");
        }

        var tokenResponse = JsonConvert.DeserializeObject<OracleTokenResponse>(body)
            ?? throw new InvalidOperationException("Resposta de token inválida.");

        return new OracleTokenState
        {
            AccessToken = tokenResponse.AccessToken,
            ExpiresAt = DateTime.UtcNow.AddSeconds(
                tokenResponse.ExpiresIn - _settings.TokenExpiryBufferSeconds)
        };
    }
}
