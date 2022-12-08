﻿using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

internal class Program
{
    private static void Main(string[] args)
    {
        // specify the input pdf file
        Console.WriteLine("Please provide the filepath for the original PDF file - formatted as per example:");
        Console.WriteLine("C:\\Documents\\Pdfs\\input.pdf");
        // string input = "C:\\Users\\avesf\\Documents\\App Testing\\PDF splitter\\input.pdf";
        string? input = Console.ReadLine();
        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Invalid Input");
            return;
        }

        // declare the number of pages in each output file
        Console.WriteLine("Please enter the number of pages required in your output files");
        int pagesPerFile = Convert.ToInt32(Console.ReadLine());

        // Specify the output folder
        Console.WriteLine("Please enter the filepath for the output folder - formatted as per example:");
        Console.WriteLine("C:\\Documents\\Pdfs");
        string? output = Console.ReadLine();
        if (string.IsNullOrEmpty(output))
        {
            Console.WriteLine("Invalid Input");
            return;
        }

        // access the file and get each page
        PdfDocument document = new PdfDocument(new PdfReader(input));
        for (int i = 1; i <= document.GetNumberOfPages(); i++)
        {
            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

            // for each page in the sequence starting with 1 and incrementing by 4, look for text matching 'account number:' and extract the following numeric value
            if (i % pagesPerFile == 1)
            {
                {
                    PdfPage page = document.GetPage(i);
                    string text = PdfTextExtractor.GetTextFromPage(document.GetPage(i), strategy);
                    int index = text.IndexOf("Account Number: ");
                    if (index > 0)
                    {
                        string accountNumber = text.Substring(index + 16, 10);
                        // add the page and the remainder of the group to a new file
                        PdfDocument newDocument = new PdfDocument(new PdfWriter(output + "\\" + accountNumber + ".pdf"));
                        document.CopyPagesTo(i, i + (pagesPerFile - 1), newDocument);
                        Console.WriteLine(newDocument.GetDocumentInfo().GetTitle());
                        newDocument.Close();
                    }

                }
            }
        }
    }
}