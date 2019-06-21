using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Win32;
using System.IO;
using LabelCreator.ViewModel;
using System.Xml.Linq;

namespace LabelCreator.Helpers
{
    public class AppHandler
    {
        public static Rectangle DrawMargin(double width)
        {
            Rectangle rect = new Rectangle();

            rect.Stroke = new SolidColorBrush(Colors.Red);
            rect.StrokeThickness = 1;
            rect.Fill = new SolidColorBrush(Colors.Red);
            rect.Width = width;
            rect.Height = 2;

            return rect;
        }


        public static PageSettings GetPrinterPageInfo(String printerName)
        {
            PrinterSettings settings;

            // If printer name is not set, look for default printer
            if (String.IsNullOrEmpty(printerName))
            {
                foreach (var printer in PrinterSettings.InstalledPrinters)
                {
                    settings = new PrinterSettings();

                    settings.PrinterName = printer.ToString();

                    if (settings.IsDefaultPrinter)
                        return settings.DefaultPageSettings;
                }

                return null; // <- No default printer  
            }

            // printer by its name 
            settings = new PrinterSettings();

            settings.PrinterName = printerName;

            return settings.DefaultPageSettings;
        }

        // TYLKO LICZBY 
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        public static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        public static bool SaveCanvas(Canvas mainCanvas)
        {
            var mystrXAML = XamlWriter.Save(mainCanvas);

            SaveFileDialog dlg = new SaveFileDialog();

            var dc = mainCanvas.DataContext;

            dlg.FileName = ((MainViewModel)dc).FileName;
            //dlg.DefaultExt = ".lblc"; // Default file extension
            dlg.Filter = "Label Creator document (.lblc)|*.lblc"; // Filter files by extension
            dlg.RestoreDirectory = true;

            // Show save file dialog box
            var result = (bool)dlg.ShowDialog();

            if (result)
            {
                try
                {
                    FileStream filestream = File.Create(dlg.FileName);
                    StreamWriter streamwriter = new StreamWriter(filestream);
                    streamwriter.Write(FormatXml(mystrXAML));
                    streamwriter.Close();
                    filestream.Close();
                }
                catch (Exception)
                {
                    throw;
                }                
            }

            return result;
        }

        private static string FormatXml(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);

                XNamespace ns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

                // USUNIĘCIE MARGINESÓW Z XMLa
                //doc.Descendants(ns + "Label")
                //    .Where(x=> (string)x.Attribute("Name") == "MarginT" || (string)x.Attribute("Name") == "MarginB" || (string)x.Attribute("Name") == "MarginL" || (string)x.Attribute("Name") == "MarginR")
                //    .Remove();
                

                return doc.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
