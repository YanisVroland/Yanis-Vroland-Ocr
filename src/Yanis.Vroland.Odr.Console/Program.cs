namespace Yanis.Vroland.Odr.Console;

using System;
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("Veuillez spécifier les chemins des images en tant qu'arguments.");
                return;
            }

            foreach (var imagePath in args)
            {
                var ocr = new Ocr(); 
                var ocrResults = ocr.Read(imagePath);

                foreach (var ocrResult in ocrResults)
                {
                    System.Console.WriteLine($"Confidence: {ocrResult.Confidence}");
                    System.Console.WriteLine($"Text: {ocrResult.Text}");
                }
            }
        }
    }
}
