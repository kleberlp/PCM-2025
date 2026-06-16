using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PCM.INTERFACE.ATRIO.Configuration;
using PCM.INTERFACE.ATRIO.Http;
using PCM.INTERFACE.ATRIO.Models;
using System.Net.Http.Headers;

namespace PCM.INTERFACE.ATRIO.Services;

public interface IOracleReservationService
{
    Task<OracleApiResult<ReservationListResponse>> GetReservationsAsync(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken ct = default);
}

public class OracleReservationService : IOracleReservationService
{
    private readonly OracleHttpClient _oracleHttp;
    private readonly IOracleAuthService _authService;
    private readonly OracleHospitalitySettings _settings;
    private readonly ILogger<OracleReservationService> _logger;

    public OracleReservationService(
        OracleHttpClient oracleHttp,
        IOracleAuthService authService,
        IOptions<OracleHospitalitySettings> options,
        ILogger<OracleReservationService> logger)
    {
        _oracleHttp = oracleHttp;
        _authService = authService;
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<OracleApiResult<ReservationListResponse>> GetReservationsAsync(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken ct = default)
    {
        try
        {
            var token = await _authService.GetAccessTokenAsync(ct);

            // Oracle OHIP exige o hotelId no PATH, não na query string
            // e as datas no formato correto com os parâmetros exatos
            var url = $"/rsv/v1/hotels/{_settings.HotelId}/reservations" +
                      $"?startDate={startDate:yyyy-MM-dd}" +
                      $"&endDate={endDate:yyyy-MM-dd}" +
                      $"&dateType=ARRIVAL";             // parâmetro obrigatório na OHIP

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("x-hotelId", _settings.HotelId);

            _logger.LogDebug("GET {Url}", url);

            var response = await _oracleHttp.Client.SendAsync(request, ct);
            var responseBody = await response.Content.ReadAsStringAsync(ct);

            // Loga SEMPRE o body para diagnóstico (sucesso ou erro)
            _logger.LogDebug("Response {Status}: {Body}",
                (int)response.StatusCode, responseBody);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Erro ao buscar reservas. HTTP {Status} | Body: {Body}",
                    (int)response.StatusCode, responseBody);
                return OracleApiResult<ReservationListResponse>.Fail(
                    $"HTTP {(int)response.StatusCode}: {responseBody}");
            }

            var data = JsonConvert.DeserializeObject<ReservationListResponse>(responseBody);
            _logger.LogInformation("Reservas obtidas: {Total}", data?.Reservations?.TotalResults);

            return OracleApiResult<ReservationListResponse>.Ok(data!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exceção ao buscar reservas.");
            return OracleApiResult<ReservationListResponse>.Fail(ex.Message);
        }
    }
}
