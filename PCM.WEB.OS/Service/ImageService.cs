using SkiaSharp;
using System.Drawing;

public class ImageService
{
    private const int MaxDimension = 400;
    private const int JpegQuality = 85; // 0-100

    public void ResizeAndSave(Stream inputStream, string outputFilePath)
    {
        using var original = SKBitmap.Decode(inputStream) ?? throw new InvalidOperationException("Não foi possível decodificar a imagem.");
        using var corrected = AutoOrient(original);

        (int newWidth, int newHeight) = CalcSize(corrected.Width, corrected.Height);

        using var resized = corrected.Resize(new SKImageInfo(newWidth, newHeight), new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
        using var image = SKImage.FromBitmap(resized);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, JpegQuality);
        using var output = File.OpenWrite(outputFilePath);

        data.SaveTo(output);
    }

    public async Task ResizeAndSaveAsync(Stream inputStream, string outputFilePath,
                                         CancellationToken ct = default)
    {
        using var ms = new MemoryStream();
        await inputStream.CopyToAsync(ms, ct);
        ms.Position = 0;

        await Task.Run(() => ResizeAndSave(ms, outputFilePath), ct);
    }

    private static (int w, int h) CalcSize(int width, int height)
    {
        if (width <= MaxDimension && height <= MaxDimension)
            return (width, height);

        double scale = width > height ? (double)MaxDimension / width : (double)MaxDimension / height;

        return ((int)(width * scale), (int)(height * scale));
    }

    private static SKBitmap AutoOrient(SKBitmap bitmap)
    {
        var info = bitmap.Info;

        if (info.Width == bitmap.Width && info.Height == bitmap.Height)
            return bitmap.Copy();

        return bitmap;
    }
}