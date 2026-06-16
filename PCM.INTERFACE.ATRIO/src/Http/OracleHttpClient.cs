using PCM.INTERFACE.ATRIO.Configuration;
using Microsoft.Extensions.Options;

namespace PCM.INTERFACE.ATRIO.Http;

/// <summary>
/// HttpClient tipado para chamadas de API Oracle Hospitality (reservas, perfis etc).
/// NÃO é usado para o endpoint de token — este tem cliente próprio no OracleAuthService.
/// </summary>
public class OracleHttpClient
{
    public HttpClient Client { get; }

    public OracleHttpClient(HttpClient client, IOptions<OracleHospitalitySettings> options)
    {
        var settings = options.Value;

        client.BaseAddress = new Uri(settings.BaseUrl);
        client.Timeout = TimeSpan.FromSeconds(30);

        // Headers obrigatórios pela Oracle em chamadas de API (não de token)
        client.DefaultRequestHeaders.Add("x-app-key", settings.AppKey);
        client.DefaultRequestHeaders.Add("enterpriseId", settings.EnterpriseId);

        Client = client;
    }
}
