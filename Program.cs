using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

internal class Program
{
    private static void Main(string[] args)
    {
        // specify the input pdf file
        Console.WriteLine("Please provide the filepath for the original PDF file - formatted as per example:");
        Console.WriteLine("C:\\Documents\\Pdfs\\input.pdf");
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
                    string text = PdfTextExtractor.GetTextFromPage(document.GetPage(i), strategy);
                    int startIndex = text.IndexOf("Account number: ") + "Account number: ".Length;
                    int endIndex = text.IndexOf(' ', startIndex);
                    if (endIndex > 0)
                    {
                        string accountNumber = text.Substring(startIndex, endIndex - startIndex);
                        // add the page and the remainder of the group to a new file
                        PdfDocument newDocument = new PdfDocument(new PdfWriter(output + "\\" + accountNumber + ".pdf"));
                        document.CopyPagesTo(i, i + (pagesPerFile - 1), newDocument);
                        Console.WriteLine("New File Created: " + output + "\\" + accountNumber + ".pdf");
                        newDocument.Close();
                    }

                }
            }
        }
    }
}