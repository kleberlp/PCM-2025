using TAG.SERVICE.DAL;
using TAG.SERVICE.Models;
using TAG.SERVICE.Services;

namespace TAG.SERVICE.Workers
{
    /// <summary>
    /// Ciclo do PCM.SERVICE.TAG legado: a cada intervalo, baixa a imagem do QR
    /// das TAGs pendentes (equipamento, rotina, apartamento) e grava no banco.
    /// Uma falha em um item não derruba o lote; uma falha num lote não derruba
    /// os demais — igual ao serviço antigo, mas com log estruturado.
    /// </summary>
    public class TagWorker : BackgroundService
    {
        private readonly ITagRepository _repo;
        private readonly TagImageService _imagens;
        private readonly IConfiguration _config;
        private readonly ILogger<TagWorker> _logger;

        public TagWorker(
            ITagRepository repo,
            TagImageService imagens,
            IConfiguration config,
            ILogger<TagWorker> logger)
        {
            _repo = repo;
            _imagens = imagens;
            _config = config;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var intervalo = TimeSpan.FromSeconds(
                _config.GetValue("Worker:IntervalSeconds", 60));

            using var timer = new PeriodicTimer(intervalo);

            do
            {
                await ProcessarLoteAsync("equipamento",
                    _repo.GetEquipamentosPendentesAsync,
                    _repo.UpdateEquipamentoTagAsync, stoppingToken);

                await ProcessarLoteAsync("rotina",
                    _repo.GetRotinasPendentesAsync,
                    _repo.UpdateRotinaTagAsync, stoppingToken);

                await ProcessarApartamentosAsync(stoppingToken);

            } while (await timer.WaitForNextTickAsync(stoppingToken));
        }

        private async Task ProcessarLoteAsync(
            string tipo,
            Func<Task<IEnumerable<TagPendente>>> buscar,
            Func<TagPendente, byte[], Task> gravar,
            CancellationToken ct)
        {
            try
            {
                foreach (var item in await buscar())
                {
                    if (ct.IsCancellationRequested) return;
                    if (string.IsNullOrWhiteSpace(item.Code)) continue;

                    try
                    {
                        var jpeg = await _imagens.DownloadJpegAsync(item.Code, ct);
                        await gravar(item, jpeg);
                    }
                    catch (OperationCanceledException) when (ct.IsCancellationRequested)
                    {
                        return;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "TAG {Tipo} codigo {Codigo}: falha ao gerar/gravar", tipo, item.Codigo);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TAG {Tipo}: falha ao buscar pendentes", tipo);
            }
        }

        private async Task ProcessarApartamentosAsync(CancellationToken ct)
        {
            try
            {
                foreach (var item in await _repo.GetApartamentosPendentesAsync())
                {
                    if (ct.IsCancellationRequested) return;
                    if (string.IsNullOrWhiteSpace(item.Code)) continue;

                    try
                    {
                        var jpeg = await _imagens.DownloadJpegAsync(item.Code, ct);
                        await _repo.UpdateApartamentoTagAsync(item.UniqueId, jpeg);
                    }
                    catch (OperationCanceledException) when (ct.IsCancellationRequested)
                    {
                        return;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "TAG apartamento {UniqueId}: falha ao gerar/gravar", item.UniqueId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TAG apartamento: falha ao buscar pendentes");
            }
        }
    }
}
