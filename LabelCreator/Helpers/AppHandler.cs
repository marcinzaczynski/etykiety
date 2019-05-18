using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Drawing.Printing;

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
    }
}
