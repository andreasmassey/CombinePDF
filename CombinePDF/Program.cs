using System;
using System.IO;
using System.Text;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace CombinePDF
{
    class Program
    {
        private const string _fileName = "Expenses";
        static void Main(string[] args)
        {
            string[] pdfs = Directory.GetFiles(@"C:\Users\Andy\Documents\Receipts", "*.PDF");
            using (PdfDocument targetDoc = new PdfDocument())
            {
                foreach (string pdf in pdfs)
                {
                    using (PdfDocument pdfDoc = PdfReader.Open(pdf, PdfDocumentOpenMode.Import))
                    {
                        for (int i = 0; i < pdfDoc.PageCount; i++)
                        {
                            targetDoc.AddPage(pdfDoc.Pages[i]);
                        }
                    }
                }
                MemoryStream stream = new MemoryStream();
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding.GetEncoding("windows-1254");
                targetDoc.Save(stream, false);

                using (FileStream file = new FileStream($"C:\\Temp\\{_fileName}.pdf", FileMode.Create, System.IO.FileAccess.Write))
                {
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    file.Write(bytes, 0, bytes.Length);
                    stream.Close();
                }
            }

            Console.WriteLine("File Saved");
            Console.ReadLine();
        }
    }
}
