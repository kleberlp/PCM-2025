using PCM.INTERFACE.DAL;

namespace PCM.INTERFACE.INTERCITY
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly List<EmpresaSettings> _empresas;
        private readonly int _timer;

        public Worker(
            ILogger<Worker> logger,
            ILoggerFactory loggerFactory,
            IConfiguration config,
            List<EmpresaSettings> empresas)
        {
            _logger = logger;
            _loggerFactory = loggerFactory;
            _empresas = empresas;
            _timer = config.GetValue<int>("timer");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PCM INTERCITY Worker iniciado. Empresas: {Count}", _empresas.Count);

            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var empresa in _empresas)
                {
                    if (stoppingToken.IsCancellationRequested) break;

                    if (!empresa.Enabled)
                    {
                        _logger.LogDebug("Empresa {Empresa} desabilitada — ignorada.", empresa.Label);
                        continue;
                    }

                    try
                    {
                        await ProcessEmpresa(empresa);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro no ciclo da empresa {Empresa} ({Codigo}).", empresa.Label, empresa.CodigoEmpresa);
                    }
                }

                await Task.Delay(_timer, stoppingToken);
            }
        }

        private async Task ProcessEmpresa(EmpresaSettings empresa)
        {
            // DAL por empresa (usa a conexão SQL/Oracle daquela empresa)
            var sqlHelper = new SqlHelper(empresa.DefaultConnection, _loggerFactory.CreateLogger<SqlHelper>());
            var apiOracle = new InterfaceApiOracle(empresa.ConnectionStringIntercity, sqlHelper, _loggerFactory.CreateLogger<InterfaceApiOracle>());

            List<string> hotelIds = await apiOracle.LoadHotelIdAsync(empresa.CodigoEmpresa);

            foreach (var hotel in hotelIds)
            {
                // STATUS UH
                await apiOracle.GetStatusUH(empresa.CodigoEmpresa, hotel);
                _logger.LogInformation("PCM - Status UH ({Empresa} / {Hotel})", empresa.Label, hotel);

                // RESERVA UH
                await apiOracle.GetReservasUH(empresa.CodigoEmpresa, hotel);
                _logger.LogInformation("PCM - Reservas UH ({Empresa} / {Hotel})", empresa.Label, hotel);
            }
        }
    }
}
