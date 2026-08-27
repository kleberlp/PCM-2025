using SkiaSharp;

namespace TAG.SERVICE.Services
{
    /// <summary>
    /// Baixa a imagem do QR code e reencoda como JPEG — mesmo comportamento do
    /// serviço legado (que usava System.Drawing), preservando o formato gravado
    /// nas colunas tag_imagem/tag.
    /// </summary>
    public class TagImageService
    {
        private readonly HttpClient _http;

        public TagImageService(HttpClient http)
        {
            _http = http;
            _http.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<byte[]> DownloadJpegAsync(string url, CancellationToken ct)
        {
            var original = await _http.GetByteArrayAsync(url, ct);

            using var bitmap = SKBitmap.Decode(original)
                ?? throw new InvalidOperationException("Conteúdo baixado não é uma imagem: " + url);

            using var image = SKImage.FromBitmap(bitmap);
            using var jpeg = image.Encode(SKEncodedImageFormat.Jpeg, 90);

            return jpeg.ToArray();
        }
    }
}
