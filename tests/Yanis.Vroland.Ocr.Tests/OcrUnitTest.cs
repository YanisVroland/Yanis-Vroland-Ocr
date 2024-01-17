using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit; 

namespace Yanis.Vroland.Ocr.Test;

public class OcrUnitTest
{
    [Fact]
    public async Task ImagesShouldBeReadCorrectly()
    {
        var executingPath = GetExecutingPath();
        var images = new List<byte[]>();
        foreach (var imagePath in
                 Directory.EnumerateFiles(Path.Combine(executingPath, "images")))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            images.Add(imageBytes);
        }

        var ocrResults = new Odr.Ocr().Read(images);

        Assert.Equal(ocrResults[0].Text, @"Set up with your own text");
        Assert.Equal(ocrResults[0].Confidence, 1);
        Assert.Equal(ocrResults[1].Text, @"Set up with your own text");
        Assert.Equal(ocrResults[1].Confidence, 1);
        Assert.Equal(ocrResults[2].Text, @"Set up with your own text");
        Assert.Equal(ocrResults[2].Confidence, 1);
    }
    private static string GetExecutingPath()
    {
        var executingAssemblyPath =
            Assembly.GetExecutingAssembly().Location;
        var executingPath =
            Path.GetDirectoryName(executingAssemblyPath);
        return executingPath;
    }
}