using LabelCreator.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabelCreator.ViewModel
{
    public class NewCanvasViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public NewCanvasViewModel()
        {

        }

        private string fileName = "Canvas1";
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged("FileName"); }
        }

        private List<string> installedPrinters;

        public List<string> InstalledPrinters
        {
            get { return installedPrinters; }
            set { installedPrinters = value; OnPropertyChanged("InstalledPrinters"); }
        }

        private string selectedPrinter;

        public string SelectedPrinter
        {
            get { return selectedPrinter; }
            set { selectedPrinter = value; OnPropertyChanged("SelectedPrinter"); LoadLabels(); }
        }



        private double width;
        public double Width
        {
            get { return width; }
            set { width = value; OnPropertyChanged("Width"); }
        }

        private double height;
        public double Height
        {
            get { return height; }
            set { height = value; OnPropertyChanged("Height"); }
        }

        private double widthPx;

        public double WidthPx
        {
            get { return widthPx; }
            set { widthPx = value; OnPropertyChanged("WidthPx"); }
        }

        private double heightPx;

        public double HeightPx
        {
            get { return heightPx; }
            set { heightPx = value; OnPropertyChanged("HeightPx"); }
        }


        private PrinterSettings.PaperSizeCollection paperSizes;

        public PrinterSettings.PaperSizeCollection PaperSizes
        {
            get { return paperSizes; }
            set { paperSizes = value; OnPropertyChanged("PaperSizes"); }
        }

        private PaperSize selectedPaperSizes;

        public PaperSize SelectedPaperSizes
        {
            get { return selectedPaperSizes; }
            set { selectedPaperSizes = value; OnPropertyChanged("SelectedPaperSizes"); SetCanvasSize(value); }
        }

        private int Resolution;

        private void LoadLabels()
        {
            var printerSettings = PrinterHandler.GetPrinterSettings(SelectedPrinter);

            PaperSizes = printerSettings.PaperSizes;

            ;

            var maxResolution = printerSettings.PrinterResolutions.OfType<PrinterResolution>()
                                         .OrderByDescending(r => r.X)
                                         .ThenByDescending(r => r.Y)
                                         .First();

            Resolution = maxResolution.X;

        }

        private void SetCanvasSize(PaperSize paperSize)
        {
            if (paperSize != null)
            {
                var pixelsW = paperSize.Width;
                var pixelsH = paperSize.Height;

                //var dpi = 11.8;
                var con = 0.254;

                var w = pixelsW * con; /// dpi;
                var h = pixelsH * con; // / dpi;

                FileName = paperSize.PaperName;
                Width = TruncateDecimal(w, 1);
                Height = TruncateDecimal(h, 1);

                WidthPx = pixelsW;
                HeightPx = pixelsH;
            }
        }

        private double TruncateDecimal(double value, int precision)
        {
            double step = Math.Pow(10, precision);
            double tmp = Math.Truncate(step * value);
            return tmp / step;
        }


    }
}
