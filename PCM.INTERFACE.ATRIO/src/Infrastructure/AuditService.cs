using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PCM.INTERFACE.ATRIO.Configuration;
using System.Data;

namespace PCM.INTERFACE.ATRIO.Infrastructure;

/// <summary>
/// Registra cada execução do serviço na tabela tb_interface_atrio_auditoria.
/// </summary>
public interface IAuditService
{
    Task<long> StartAsync(string processo, CancellationToken ct = default);
    Task FinishAsync(long auditId, int registros, CancellationToken ct = default);
    Task FailAsync(long auditId, string erro, CancellationToken ct = default);
}

public class AuditService : IAuditService
{
    private readonly string _connectionString;
    private readonly ILogger<AuditService> _logger;

    public AuditService(
        IOptions<ServiceSettings> settings,
        ILogger<AuditService> logger)
    {
        _connectionString = settings.Value.ConnectionString;
        _logger           = logger;
    }

    /// <summary>Abre um registro de auditoria e retorna o ID gerado.</summary>
    public async Task<long> StartAsync(string processo, CancellationToken ct = default)
    {
        try
        {
            await using var conn = new SqlConnection(_connectionString);
            await using var cmd  = new SqlCommand(@"
                INSERT INTO tb_interface_atrio_auditoria
                    (processo, dt_inicio, status)
                OUTPUT INSERTED.id
                VALUES (@processo, GETDATE(), 'EXECUTANDO')", conn)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.Add(new SqlParameter("@processo", SqlDbType.VarChar, 100)
                { Value = processo });

            await conn.OpenAsync(ct);
            var id = await cmd.ExecuteScalarAsync(ct);
            return Convert.ToInt64(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao gravar início de auditoria ({Processo}).", processo);
            return -1;
        }
    }

    /// <summary>Marca a auditoria como concluída com sucesso.</summary>
    public async Task FinishAsync(long auditId, int registros, CancellationToken ct = default)
    {
        if (auditId <= 0) return;

        try
        {
            await using var conn = new SqlConnection(_connectionString);
            await using var cmd  = new SqlCommand(@"
                UPDATE tb_interface_atrio_auditoria
                SET    dt_fim = GETDATE(),
                       status = 'CONCLUIDO',
                       registros_processados = @registros
                WHERE  id = @id", conn)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.Add(new SqlParameter("@id",        SqlDbType.BigInt)  { Value = auditId   });
            cmd.Parameters.Add(new SqlParameter("@registros", SqlDbType.Int)     { Value = registros });

            await conn.OpenAsync(ct);
            await cmd.ExecuteNonQueryAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao gravar fim de auditoria (ID {AuditId}).", auditId);
        }
    }

    /// <summary>Marca a auditoria como falha com a mensagem de erro.</summary>
    public async Task FailAsync(long auditId, string erro, CancellationToken ct = default)
    {
        if (auditId <= 0) return;

        try
        {
            await using var conn = new SqlConnection(_connectionString);
            await using var cmd  = new SqlCommand(@"
                UPDATE tb_interface_atrio_auditoria
                SET    dt_fim       = GETDATE(),
                       status       = 'ERRO',
                       mensagem_erro = @erro
                WHERE  id = @id", conn)
            {
                CommandType = CommandType.Text
            };

            cmd.Parameters.Add(new SqlParameter("@id",   SqlDbType.BigInt)       { Value = auditId });
            cmd.Parameters.Add(new SqlParameter("@erro", SqlDbType.VarChar, -1)  { Value = erro    });

            await conn.OpenAsync(ct);
            await cmd.ExecuteNonQueryAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao gravar erro de auditoria (ID {AuditId}).", auditId);
        }
    }
}
