using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 

namespace Yanis.Vroland.Odr.WebApi;

[ApiController]
[Route("[controller]")]
public class OcrController : ControllerBase
{
    private readonly Ocr.Ocr _ocr;
    public OcrController(Ocr.Ocr ocr)
    {
        _ocr = ocr;
    }
    [HttpPost]
    public async Task<IList<OcrResult>>
        OnPostUploadAsync([FromForm(Name = "files")] IList<IFormFile> files)
    {
        var images = new List<byte[]>();
        foreach (var formFile in files)
        {
            using var sourceStream = formFile.OpenReadStream();
            using var memoryStream = new MemoryStream();
            sourceStream.CopyTo(memoryStream);
            images.Add( memoryStream.
            images.Add(memoryStream.ToArray());
        }
        
        var ocrResults = new List<OcrResult>();
        foreach (var image in images)
        {
            var result = _ocr.Read(image); 
            ocrResults.AddRange(result);
        }

        return ocrResults;
    }
}