using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PCM.INTERFACE.ATRIO.Configuration;
using PCM.INTERFACE.ATRIO.Http;
using PCM.INTERFACE.ATRIO.Models;
using PCM.INTERFACE.ATRIO.Repositories;
using System.Net.Http.Headers;

namespace PCM.INTERFACE.ATRIO.Services;

public interface IOracleHousekeepingService
{
    Task<(bool Success, int Registros)> SyncAllRoomsAsync(CancellationToken ct = default);
}

public class OracleHousekeepingService : IOracleHousekeepingService
{
    private readonly OracleHttpClient _oracleHttp;
    private readonly IOracleAuthService _authService;
    private readonly IHousekeepingRepository _repository;
    private readonly OracleHospitalitySettings _oracleSettings;
    private readonly ServiceSettings _serviceSettings;
    private readonly ILogger<OracleHousekeepingService> _logger;

    public OracleHousekeepingService(
        OracleHttpClient oracleHttp,
        IOracleAuthService authService,
        IHousekeepingRepository repository,
        IOptions<OracleHospitalitySettings> oracleOptions,
        IOptions<ServiceSettings> serviceOptions,
        ILogger<OracleHousekeepingService> logger)
    {
        _oracleHttp      = oracleHttp;
        _authService     = authService;
        _repository      = repository;
        _oracleSettings  = oracleOptions.Value;
        _serviceSettings = serviceOptions.Value;
        _logger          = logger;
    }

    public async Task<(bool Success, int Registros)> SyncAllRoomsAsync(CancellationToken ct = default)
    {
        var offset       = 0;
        var totalResults = 0;
        var totalRows    = 0;
        const int limit  = 100;

        _logger.LogInformation("Iniciando sync housekeeping (hotel: {HotelId})...",
            _oracleSettings.HotelId);

        do
        {
            var (pageData, rawJson) = await GetPageAsync(offset, limit, ct);

            if (pageData is null || rawJson is null)
            {
                _logger.LogError("Falha ao buscar página housekeeping offset={Offset}.", offset);
                return (false, totalRows);
            }

            var collection = pageData.HousekeepingRoomInfo;
            totalResults   = collection?.TotalResults ?? 0;

            _logger.LogInformation(
                "Housekeeping offset={Offset} | {Count} quartos | Total: {Total}",
                offset, collection?.Rooms.Length ?? 0, totalResults);

            // Envia o JSON bruto desta página para a SP — processamento em massa via OPENJSON
            var rows = await _repository.InterfaceHotelRoomsAsync(
                codigoEmpresa : _serviceSettings.CodigoEmpresa,
                hotelId       : _oracleSettings.HotelId,
                json          : rawJson,
                type          : "OPERA",
                ct            : ct);

            totalRows += rows;
            offset    += limit;

        } while (offset <= totalResults);

        _logger.LogInformation(
            "Sync housekeeping concluído. Total de registros processados: {Total}", totalRows);

        return (true, totalRows);
    }

    private async Task<(HousekeepingOverviewResponse? Data, string? RawJson)> GetPageAsync(
        int offset, int limit, CancellationToken ct)
    {
        try
        {
            var token = await _authService.GetAccessTokenAsync(ct);
            var url   = $"/hsk/v1/hotels/{_oracleSettings.HotelId}/housekeepingOverview" +
                        $"?limit={limit}&offset={offset}";

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("x-hotelId", _oracleSettings.HotelId);

            _logger.LogDebug("GET {Url}", url);

            var response = await _oracleHttp.Client.SendAsync(request, ct);
            var body     = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Erro HTTP {Status} ao buscar housekeeping offset={Offset}. Body: {Body}",
                    (int)response.StatusCode, offset, body);
                return (null, null);
            }

            var data = JsonConvert.DeserializeObject<HousekeepingOverviewResponse>(body);
            return (data, body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exceção ao buscar housekeeping offset={Offset}.", offset);
            return (null, null);
        }
    }
}
