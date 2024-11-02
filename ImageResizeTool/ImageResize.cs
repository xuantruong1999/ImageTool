using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace ImageResizeTool;

public class ImageResize
{
    public static void ResizeImage(string sourcePath, string targetPath, int width, int height)
    {
        var listImages = Directory.GetFiles(sourcePath, "*.*", SearchOption.TopDirectoryOnly)
                             .Where(f => f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".png"));

        foreach (var imag in listImages)
        {
            using (var image = Image.Load(imag))
            {
                var resizeOptions = new ResizeOptions
                {
                    Size = new Size(width, height),
                    Mode = ResizeMode.Max,
                    Sampler = KnownResamplers.Lanczos3
                };

                image.Mutate(x => x.Resize(resizeOptions));
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                image.Save(Path.Combine(targetPath, Path.GetFileName(imag)));
            }
        }
    }


    public static void ConvertImageFormat(string sourcePath, string targetPath, string format)
    {
        var listImages = Directory.GetFiles(sourcePath, "*", SearchOption.TopDirectoryOnly)
                             .Where(f => f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".png"));

        foreach (var imag in listImages)
        {
            using (var image = Image.Load(imag))
            {
                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }
                image.Save(Path.Combine(targetPath, Path.GetFileNameWithoutExtension(imag) + "." + format));
            }
        }
    }
    public static void CompressImage(string sourcePath, string targetPath, int quality, bool compressAll = false)
    {
        try
        {
            var files = Directory.GetFiles(sourcePath, "*.*", SearchOption.TopDirectoryOnly)
                                 .Where(f => f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".png"));

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                if (compressAll || fileInfo.Length > 100 * 1024) // 100 KB
                {
                    using (var image = Image.Load(file))
                    {
                        // Apply any additional processing if needed
                        image.Mutate(x => x.AutoOrient());

                        var encoder = new JpegEncoder
                        {
                            Quality = quality
                        };

                        if (!Directory.Exists(targetPath))
                        {
                            Directory.CreateDirectory(targetPath);
                        }
                        var targetFilePath = Path.Combine(targetPath, Path.GetFileName(file));
                        image.Save(targetFilePath, encoder);

                        // Check if the file size is greater than 70 KB
                        var compressedFileInfo = new FileInfo(targetFilePath);
                        if (compressedFileInfo.Length > 70 * 1024)
                        {
                            // Reduce quality to try to get under 70 KB
                            for (int q = quality - 10; q > 0; q -= 10)
                            {
                                using (var tempImage = Image.Load(file))
                                {
                                    var tempEncoder = new JpegEncoder
                                    {
                                        Quality = q
                                    };
                                    tempImage.Save(targetFilePath, tempEncoder);
                                    compressedFileInfo = new FileInfo(targetFilePath);
                                    if (compressedFileInfo.Length <= 70 * 1024)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    var targetFilePath = Path.Combine(targetPath, Path.GetFileName(file));
                    File.Copy(file, targetFilePath, true);
                }
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"Access to the path '{sourcePath}' or '{targetPath}' is denied: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}