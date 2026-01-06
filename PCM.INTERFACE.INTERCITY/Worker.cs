using PCM.INTERFACE.DAL;
using System.Threading.Tasks;

namespace PCM.INTERFACE.INTERCITY
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly InterfaceApiOracle _apiOracle;
        private readonly int _codigoEmpresa;
        private readonly int _timer;

        public Worker(ILogger<Worker> logger, IConfiguration config, InterfaceApiOracle apiOracle)
        {
            _logger = logger;
            _apiOracle = apiOracle;

            _codigoEmpresa = config.GetValue<int>("codigoEmpresa");
            _timer = config.GetValue<int>("timer");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PCM INTERCITY Worker iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Carrega hotéis
                    List<string> hotelIds = await _apiOracle.LoadHotelIdAsync(_codigoEmpresa);

                    foreach (var hotel in hotelIds)
                    {
                        ProcessHotel(hotel);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro geral no ciclo do Worker.");
                }

                await Task.Delay(_timer, stoppingToken);
            }
        }

        private async Task ProcessHotel(string hotel)
        {
            // STATUS UH
            await _apiOracle.GetStatusUH(_codigoEmpresa, hotel);
            _logger.LogInformation("PCM - Status UH ({hotel})", hotel);
            

            // RESERVA UH
            await _apiOracle.GetReservasUH(_codigoEmpresa, hotel);
            _logger.LogInformation("PCM - Reservas UH ({hotel})", hotel);
        
        }
    }
}
