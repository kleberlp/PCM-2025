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
    private readonly IReadOnlyList<PropertySettings> _properties;

    public InterfaceWorker(
        ILogger<InterfaceWorker> logger,
        IServiceScopeFactory scopeFactory,
        IOptions<ServiceSettings> settings,
        IReadOnlyList<PropertySettings> properties)
    {
        _logger       = logger;
        _scopeFactory = scopeFactory;
        _settings     = settings.Value;
        _properties   = properties;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PCM Interface ATRIO iniciado em: {Time}", DateTimeOffset.Now);
        _logger.LogInformation("Propriedades configuradas: {Count} ({Nomes})",
            _properties.Count,
            string.Join(", ", _properties.Select(p => $"{p.Label}{(p.Enabled ? "" : " (desabilitada)")}")));

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("=== Iniciando ciclo {Time} ===", DateTimeOffset.Now);

            foreach (var property in _properties)
            {
                if (stoppingToken.IsCancellationRequested) break;

                if (!property.Enabled)
                {
                    _logger.LogDebug("Propriedade {Property} desabilitada — ignorada.", property.Label);
                    continue;
                }

                try
                {
                    await RunPropertyAsync(property, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Falha inesperada ao processar a propriedade {Property}.", property.Label);
                }
            }

            _logger.LogInformation("=== Ciclo concluído ===");

            await Task.Delay(
                TimeSpan.FromMinutes(_settings.IntervalMinutes),
                stoppingToken);
        }

        _logger.LogInformation("PCM Interface ATRIO encerrado.");
    }

    private async Task RunPropertyAsync(PropertySettings property, CancellationToken ct)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();

        // define a propriedade em processamento para todo o escopo
        scope.ServiceProvider.GetRequiredService<IPropertyContext>().Current = property;

        var audit = scope.ServiceProvider.GetRequiredService<IAuditService>();
        var email = scope.ServiceProvider.GetRequiredService<IEmailNotificationService>();

        _logger.LogInformation("--- Propriedade {Property} (empresa {Empresa} | hotel {Hotel}) ---",
            property.Label, property.CodigoEmpresa, property.HotelId);

        // -------------------------------------------------------
        // HOUSEKEEPING
        // -------------------------------------------------------
        if (property.SyncHousekeeping)
        {
            var auditId = await audit.StartAsync("HOUSEKEEPING", ct);
            try
            {
                var hskService = scope.ServiceProvider.GetRequiredService<IOracleHousekeepingService>();
                var (success, registros) = await hskService.SyncAllRoomsAsync(ct);

                if (success)
                {
                    await audit.FinishAsync(auditId, registros, ct);
                    _logger.LogInformation("Housekeeping concluído ({Property}). Registros: {Reg}", property.Label, registros);
                }
                else
                {
                    await audit.FailAsync(auditId, "Falha no sync de housekeeping.", ct);
                    await email.SendErrorAsync(
                        $"Falha no Sync de Housekeeping ({property.Label})",
                        $"O sync de housekeeping falhou em {DateTimeOffset.Now:dd/MM/yyyy HH:mm}.\n" +
                        $"Propriedade: {property.Label}\n" +
                        $"Empresa: {property.CodigoEmpresa} | Hotel: {property.HotelId}\n" +
                        "Verifique os logs para mais detalhes.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exceção no sync de housekeeping ({Property}).", property.Label);
                await audit.FailAsync(auditId, ex.ToString(), ct);
                await email.SendErrorAsync($"Exceção no Sync de Housekeeping ({property.Label})",
                    $"Ocorreu uma exceção em {DateTimeOffset.Now:dd/MM/yyyy HH:mm}.", ex);
            }
        }
        else
        {
            _logger.LogDebug("Sync de housekeeping desabilitado ({Property}).", property.Label);
        }

        // -------------------------------------------------------
        // RESERVAS
        // -------------------------------------------------------
        if (property.SyncReservations)
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
                    _logger.LogInformation("Reservas concluído ({Property}). Registros: {Reg}", property.Label, count);

                    // TODO: chamar repositório de reservas quando SP estiver pronta
                }
                else
                {
                    await audit.FailAsync(auditId, result.ErrorMessage ?? "Erro desconhecido", ct);
                    await email.SendErrorAsync(
                        $"Falha no Sync de Reservas ({property.Label})",
                        $"O sync de reservas falhou em {DateTimeOffset.Now:dd/MM/yyyy HH:mm}.\n" +
                        $"Propriedade: {property.Label}\n" +
                        $"Motivo: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exceção no sync de reservas ({Property}).", property.Label);
                await audit.FailAsync(auditId, ex.ToString(), ct);
                await email.SendErrorAsync($"Exceção no Sync de Reservas ({property.Label})",
                    $"Ocorreu uma exceção em {DateTimeOffset.Now:dd/MM/yyyy HH:mm}.", ex);
            }
        }
        else
        {
            _logger.LogDebug("Sync de reservas desabilitado ({Property}).", property.Label);
        }
    }
}
