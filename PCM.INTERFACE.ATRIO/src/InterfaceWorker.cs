using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PCM.INTERFACE.ATRIO.Configuration;
using PCM.INTERFACE.ATRIO.Infrastructure;
using PCM.INTERFACE.ATRIO.Services;

namespace PCM.INTERFACE.ATRIO;

public class InterfaceWorker : BackgroundService
{
    private readonly ILogger<InterfaceWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ServiceSettings _settings;

    public InterfaceWorker(
        ILogger<InterfaceWorker> logger,
        IServiceScopeFactory scopeFactory,
        IOptions<ServiceSettings> settings)
    {
        _logger      = logger;
        _scopeFactory = scopeFactory;
        _settings    = settings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PCM Interface ATRIO iniciado em: {Time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            await RunCycleAsync(stoppingToken);

            await Task.Delay(
                TimeSpan.FromMinutes(_settings.IntervalMinutes),
                stoppingToken);
        }

        _logger.LogInformation("PCM Interface ATRIO encerrado.");
    }

    private async Task RunCycleAsync(CancellationToken ct)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();

        var audit = scope.ServiceProvider.GetRequiredService<IAuditService>();
        var email = scope.ServiceProvider.GetRequiredService<IEmailNotificationService>();

        _logger.LogInformation("=== Iniciando ciclo {Time} ===", DateTimeOffset.Now);

        // -------------------------------------------------------
        // HOUSEKEEPING
        // -------------------------------------------------------
        if (_settings.SyncHousekeeping)
        {
            var auditId = await audit.StartAsync("HOUSEKEEPING", ct);
            try
            {
                var hskService = scope.ServiceProvider.GetRequiredService<IOracleHousekeepingService>();
                var (success, registros) = await hskService.SyncAllRoomsAsync(ct);

                if (success)
                {
                    await audit.FinishAsync(auditId, registros, ct);
                    _logger.LogInformation("Housekeeping concluído. Registros: {Reg}", registros);
                }
                else
                {
                    await audit.FailAsync(auditId, "Falha no sync de housekeeping.", ct);
                    await email.SendErrorAsync(
                        "Falha no Sync de Housekeeping",
                        $"O sync de housekeeping falhou em {DateTimeOffset.Now:dd/MM/yyyy HH:mm}.\n" +
                        $"Hotel: {_settings.CodigoEmpresa}\n" +
                        "Verifique os logs para mais detalhes.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exceção no sync de housekeeping.");
                await audit.FailAsync(auditId, ex.ToString(), ct);
                await email.SendErrorAsync("Exceção no Sync de Housekeeping",
                    $"Ocorreu uma exceção em {DateTimeOffset.Now:dd/MM/yyyy HH:mm}.", ex);
            }
        }
        else
        {
            _logger.LogDebug("Sync de housekeeping desabilitado (SyncHousekeeping=false).");
        }

        // -------------------------------------------------------
        // RESERVAS
        // -------------------------------------------------------
        if (_settings.SyncReservations)
        {
            var auditId = await audit.StartAsync("RESERVATIONS", ct);
            try
            {
                var rsvService = scope.ServiceProvider.GetRequiredService<IOracleReservationService>();
                var startDate  = DateOnly.FromDateTime(DateTime.Today);
                var endDate    = startDate.AddDays(30);

                var result = await rsvService.GetReservationsAsync(startDate, endDate, ct);

                if (result.Success)
                {
                    var count = result.Data?.Reservations?.ReservationInfo?.Length ?? 0;
                    await audit.FinishAsync(auditId, count, ct);
                    _logger.LogInformation("Reservas concluído. Registros: {Reg}", count);

                    // TODO: chamar repositório de reservas quando SP estiver pronta
                }
                else
                {
                    await audit.FailAsync(auditId, result.ErrorMessage ?? "Erro desconhecido", ct);
                    await email.SendErrorAsync(
                        "Falha no Sync de Reservas",
                        $"O sync de reservas falhou em {DateTimeOffset.Now:dd/MM/yyyy HH:mm}.\n" +
                        $"Motivo: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exceção no sync de reservas.");
                await audit.FailAsync(auditId, ex.ToString(), ct);
                await email.SendErrorAsync("Exceção no Sync de Reservas",
                    $"Ocorreu uma exceção em {DateTimeOffset.Now:dd/MM/yyyy HH:mm}.", ex);
            }
        }
        else
        {
            _logger.LogDebug("Sync de reservas desabilitado (SyncReservations=false).");
        }

        _logger.LogInformation("=== Ciclo concluído ===");
    }
}
