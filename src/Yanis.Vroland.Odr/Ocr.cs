using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

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
        List<OcrResult> results = new List<OcrResult>();

        foreach (var image in images)
        {
            var task = Task.Run(() =>
            {
                var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
                var executingPath = Path.GetDirectoryName(executingAssemblyPath);

                using (var engine = new TesseractEngine(Path.Combine(executingPath, @"tessdata"), "fra", EngineMode.Default))
                {
                    using (var pix = Pix.LoadFromMemory(image))
                    {
                        var result = engine.Process(pix);
                        var text = result.GetText();
                        var confidence = result.GetMeanConfidence();

                        return new OcrResult { Text = text, Confidence = confidence };
                    }
                }
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
