using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Management;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LabelCreator.Helpers
{
    public class PrinterHandler
    {


        public static PrinterSettings GetPrinterSettings(string printerName)
        {
            var settings = new PrinterSettings
            {
                PrinterName = printerName
            };

            return settings;
        }

        public static void GetPrinterSettings2(string printerName)
        {
            string query = string.Format("SELECT * from Win32_Printer WHERE Name LIKE '%{0}'", printerName);

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            using (ManagementObjectCollection coll = searcher.Get())
            {
                try
                {
                    foreach (ManagementObject printer in coll)
                    {
                        foreach (PropertyData property in printer.Properties)
                        {
                            Console.WriteLine(string.Format("{0}: {1}", property.Name, property.Value));
                        }
                    }
                }
                catch (ManagementException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }




        public static void Print(Canvas c, string printerName, object o = null)
        {
            var printDialog = new PrintDialog();
            using (var printQueue = new PrintQueue(new PrintServer(), printerName))
            {
                printDialog.PrintQueue = printQueue;
                
                var area = printDialog.PrintQueue.GetPrintCapabilities();
                
                if (area.PageImageableArea == null) throw new Exception("Failed to load printer settings.");

                var fd = new FlowDocument();

                fd.PagePadding = new Thickness(area.PageImageableArea.OriginWidth, 0, 0, 0);
                fd.PageWidth = area.PageImageableArea.ExtentWidth + area.PageImageableArea.OriginWidth;
                

                //var flowDocument = new FlowDocument
                //{
                //    PagePadding = new Thickness(18, 0, 0, 0),
                //    PageWidth = c.Width,
                //    PageHeight = c.Height
                //}; 

                c.SnapsToDevicePixels = true;
                RenderOptions.SetBitmapScalingMode(c, BitmapScalingMode.NearestNeighbor);
                fd.Blocks.Add(new BlockUIContainer(c));

                var paginator = ((IDocumentPaginatorSource)fd).DocumentPaginator;

                printDialog.PrintDocument(paginator, "A Flow Document");
            }
        }

        public static void PrintFixed(Canvas c, string printerName)
        {
            PrintDialog dialog = new PrintDialog();
            
            //if(dialog.ShowDialog() != true) return;
            
            using (var printQueue = new PrintQueue(new PrintServer(), printerName))
            {
                dialog.PrintQueue = printQueue;

                c.SnapsToDevicePixels = true;
                RenderOptions.SetBitmapScalingMode(c, BitmapScalingMode.NearestNeighbor);

                var doc = GetPrintDocument(c);                

                PrintPreview preview = new PrintPreview();
                preview.Document = doc;                
                preview.ShowDialog();

                dialog.PrintDocument(doc.DocumentPaginator, "Test");
            }
        }

        private static FixedDocument GetPrintDocument(Canvas canvas)
        {
            var magicOffset = 20;               // 5.29 mm

            FixedDocument document = new FixedDocument()
            {
                
            };
            
            document.DocumentPaginator.PageSize = new System.Windows.Size(canvas.Width, canvas.Height);

            // create a page
            FixedPage page1 = new FixedPage();
            page1.Width = document.DocumentPaginator.PageSize.Width;
            page1.Height = document.DocumentPaginator.PageSize.Height;       
            page1.Children.Add(canvas);

            PageContent page1Content = new PageContent();
            ((IAddChild)page1Content).AddChild(page1);
            document.Pages.Add(page1Content);

            return document;
        }

        public static void Print(Canvas canvas, int dpi)
        {
            System.Windows.Forms.PrintDialog dialog = new System.Windows.Forms.PrintDialog();

            dialog.Document = GetPrintDocument(canvas, dpi);
            dialog.AllowSomePages = true;
            dialog.AllowCurrentPage = true;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dialog.Document.Print();
            }
        }

        public static PrintDocument GetPrintDocument(Canvas canvas, int dpi)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.DefaultPageSettings.Landscape = false; // Set orientation here

            printDocument.PrinterSettings.MaximumPage = 1;// set maximum page count here
            bool isCalculatedrange = false;
            int printedpagescount = 0;
            int startPage = 0;
            int printPageCount = 0;


            printDocument.PrintPage += (s, args) =>
            {
                try
                {
                    if (printDocument.PrinterSettings.ToPage > 0 && !isCalculatedrange)
                    {
                        startPage = printDocument.PrinterSettings.FromPage - 1;
                        printPageCount = (printDocument.PrinterSettings.ToPage - printDocument.PrinterSettings.FromPage) + 1 > printPageCount ? printPageCount : (printDocument.PrinterSettings.ToPage - printDocument.PrinterSettings.FromPage) + 1;
                        isCalculatedrange = true;
                    }
                    args.Graphics.PageUnit = System.Drawing.GraphicsUnit.Pixel;

                    args.Graphics.DrawImage(GetImageFromUIElement(canvas),
                        new System.Drawing.Rectangle(printDocument.PrinterSettings.DefaultPageSettings.Margins.Left, printDocument.PrinterSettings.DefaultPageSettings.Margins.Top, (int)canvas.ActualWidth, (int)canvas.ActualHeight));
                    startPage++;
                    printedpagescount++;
                    args.HasMorePages = startPage < printPageCount;

                    if (isCalculatedrange)
                    {
                        args.HasMorePages = (printPageCount != printedpagescount);
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + "\n StackTrace: \n" + e.StackTrace);
                }
            };



            printDocument.EndPrint += (s, args) =>
            {
                //MessageBox.Show("Documents printed completed...");
            };

            return printDocument;
        }

        public static System.Drawing.Image GetImageFromUIElement(Canvas canva)
        {
            var w = (int)canva.ActualWidth;
            var h = (int)canva.ActualHeight;

            RenderTargetBitmap bitmap = new RenderTargetBitmap(w, h, 96d, 96d, PixelFormats.Default);

            bitmap.Render(canva);

            var encoder = new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                canva.Clip = null;
                bitmap.Freeze();
                bitmap.Clear();
                return System.Drawing.Image.FromStream(stream);
            }
        }

        public static void CreateImage(Canvas canvas)
        {
            var w = (int)canvas.Width;
            var h = (int)canvas.Height;

            RenderTargetBitmap rtb = new RenderTargetBitmap(w, h, 96d, 96d, PixelFormats.Pbgra32);
            rtb.Render(canvas);

            BitmapEncoder jpegEcnoder = new JpegBitmapEncoder();
            jpegEcnoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var fs = File.OpenWrite(@"logo.jpg"))
            {
                jpegEcnoder.Save(fs);
            }
        }

        private static ImageSource GetSource(System.Drawing.Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }       

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
