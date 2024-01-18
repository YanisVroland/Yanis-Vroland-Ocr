using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Tesseract;

namespace Yanis.Vroland.Odr;

public class Ocr
{
    private static string GetExecutingPath()
    {
        var executingAssemblyPath =
            Assembly.GetExecutingAssembly().Location;
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);
        return executingPath;
    }
    public List<OcrResult> Read(IList<byte[]> images)
    {
        var tasks = new List<Task<OcrResult>>();
        var results = new List<OcrResult>();

        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);
        
        using var engine = new TesseractEngine(Path.Combine(executingPath, @"tessdata"), "fra", EngineMode.Default);

        foreach (var image in images)
        {
            var task = Task.Run(() =>
            {
                using var pix = Pix.LoadFromMemory(image);
                var test = engine.Process(pix);
                
                OcrResult tmp = new OcrResult();
                tmp.Text = test.GetText();
                tmp.Confidence = test.GetMeanConfidence();
                return tmp;
            });

            tasks.Add(task);
        }

        Task.WaitAll(tasks.ToArray()); 

        foreach (var task in tasks)
        {
            var result = task.Result;
            results.Add(result);
        }

        
        return results;
    }
}
