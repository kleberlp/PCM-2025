using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PCM.INTERFACE.ATRIO;
using PCM.INTERFACE.ATRIO.Configuration;
using PCM.INTERFACE.ATRIO.Http;
using PCM.INTERFACE.ATRIO.Infrastructure;
using PCM.INTERFACE.ATRIO.Repositories;
using PCM.INTERFACE.ATRIO.Services;
using Polly;
using Polly.Extensions.Http;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("PCM Interface ATRIO iniciando...");

    var host = Host.CreateDefaultBuilder(args)

        .UseWindowsService(options => options.ServiceName = "PCM.INTERFACE.ATRIO")

        .UseSerilog((context, services, config) =>
            config
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
        )

        .ConfigureServices((context, services) =>
        {
            var cfg = context.Configuration;

            // -------------------------------------------------------
            // CONFIGURAÇÕES TIPADAS
            // -------------------------------------------------------
            services.Configure<OracleHospitalitySettings>(
                cfg.GetSection(OracleHospitalitySettings.Section));
            services.Configure<ServiceSettings>(
                cfg.GetSection(ServiceSettings.Section));
            services.Configure<EmailSettings>(
                cfg.GetSection(EmailSettings.Section));

            // -------------------------------------------------------
            // HTTP — cliente limpo para OAuth (sem headers de API)
            // -------------------------------------------------------
            services.AddHttpClient("OracleAuth")
                .ConfigureHttpClient(c => c.Timeout = TimeSpan.FromSeconds(30));

            // -------------------------------------------------------
            // HTTP — cliente tipado para chamadas de API
            // -------------------------------------------------------
            services.AddHttpClient<OracleHttpClient>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            // -------------------------------------------------------
            // INFRAESTRUTURA
            // -------------------------------------------------------
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IEmailNotificationService, EmailNotificationService>();

            // -------------------------------------------------------
            // REPOSITÓRIOS
            // -------------------------------------------------------
            services.AddScoped<IHousekeepingRepository, HousekeepingRepository>();

            // -------------------------------------------------------
            // SERVIÇOS
            // -------------------------------------------------------
            services.AddSingleton<IOracleAuthService, OracleAuthService>();
            services.AddScoped<IOracleHousekeepingService, OracleHousekeepingService>();
            services.AddScoped<IOracleReservationService, OracleReservationService>();

            // -------------------------------------------------------
            // WORKER
            // -------------------------------------------------------
            services.AddHostedService<InterfaceWorker>();
        })
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "PCM Interface ATRIO encerrado inesperadamente.");
}
finally
{
    Log.CloseAndFlush();
}

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
            onRetry: (outcome, timespan, attempt, _) =>
                Log.Warning("Tentativa {Attempt} falhou. Aguardando {Delay}s.",
                    attempt, timespan.TotalSeconds));

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30),
            onBreak:  (_, d) => Log.Warning("Circuit breaker ABERTO por {D}s.", d.TotalSeconds),
            onReset:  ()     => Log.Information("Circuit breaker FECHADO."));
