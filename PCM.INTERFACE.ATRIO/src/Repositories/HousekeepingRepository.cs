using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PCM.INTERFACE.ATRIO.Configuration;
using System.Data;

namespace PCM.INTERFACE.ATRIO.Repositories;

public interface IHousekeepingRepository
{
    /// <summary>
    /// Envia o JSON de uma página para a SP, que faz DELETE+INSERT em massa.
    /// Equivalente ao InterfaceHotelRooms do VB.
    /// </summary>
    Task<int> InterfaceHotelRoomsAsync(
        int codigoEmpresa,
        string hotelId,
        string json,
        string type = "OPERA",
        CancellationToken ct = default);
}

public class HousekeepingRepository : IHousekeepingRepository
{
    private readonly string _connectionString;
    private readonly ILogger<HousekeepingRepository> _logger;

    public HousekeepingRepository(
        IOptions<ServiceSettings> settings,
        ILogger<HousekeepingRepository> logger)
    {
        _connectionString = settings.Value.ConnectionString;
        _logger           = logger;
    }

    public async Task<int> InterfaceHotelRoomsAsync(
        int codigoEmpresa,
        string hotelId,
        string json,
        string type = "OPERA",
        CancellationToken ct = default)
    {
        try
        {
            await using var conn = new SqlConnection(_connectionString);
            await using var cmd  = new SqlCommand("sp_interface_cadastro_basico_apartamento_opera_json", conn)
            {
                CommandType    = CommandType.StoredProcedure,
                CommandTimeout = 120
            };

            cmd.Parameters.Add(new SqlParameter("@codigo_empresa", SqlDbType.SmallInt)
                { Value = codigoEmpresa });

            cmd.Parameters.Add(new SqlParameter("@hotel_id", SqlDbType.VarChar, 20)
                { Value = hotelId });

            // JSON completo da página — SP usa OPENJSON para processar em massa
            cmd.Parameters.Add(new SqlParameter("@json", SqlDbType.VarChar, -1)
                { Value = json });

            cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar, 50)
                { Value = type });

            await conn.OpenAsync(ct);
            var rows = await cmd.ExecuteNonQueryAsync(ct);

            _logger.LogDebug(
                "InterfaceHotelRooms OK. Empresa: {Empresa} | Hotel: {Hotel} | Rows: {Rows}",
                codigoEmpresa, hotelId, rows);

            return rows < 0 ? 0 : rows;
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx,
                "Erro SQL em InterfaceHotelRooms. Empresa: {Empresa} | Hotel: {Hotel}",
                codigoEmpresa, hotelId);
            throw;
        }
    }
}
