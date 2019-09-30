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

        private const double dpi = 96;                  // 1 in = 96 pixel 
        private const double inConst = 25.4;            // 1 in = 25.4 mm 
        private const double mmConst = 0.03937007874;   // 1 mm = 0.03937007874 in

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
            set { width = Math.Round(value); OnPropertyChanged("Width"); WidthPx = mmToPx(value); WidthIn = mmToIn(value); }
        }

        private double height;
        public double Height
        {
            get { return height; }
            set { height = Math.Round(value); OnPropertyChanged("Height"); HeightPx = mmToPx(value); HeightIn = mmToIn(value); }
        }

        private double widthPx;

        public double WidthPx
        {
            get { return widthPx; }
            set { widthPx = Math.Round(value, 2); OnPropertyChanged("WidthPx"); }
        }

        private double heightPx;

        public double HeightPx
        {
            get { return heightPx; }
            set { heightPx = Math.Round(value, 2); OnPropertyChanged("HeightPx"); }
        }

        private double widthIn;

        public double WidthIn
        {
            get { return widthIn; }
            set { widthIn = Math.Round(value, 1); OnPropertyChanged("WidthIn"); }
        }

        private double heightIn;

        public double HeightIn
        {
            get { return heightIn; }
            set { heightIn = Math.Round(value, 1); OnPropertyChanged("HeightIn"); }
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

        private void LoadLabels()
        {
            var printerSettings = PrinterHandler.GetPrinterSettings(SelectedPrinter);

            PaperSizes = printerSettings.PaperSizes;
        }

        private void SetCanvasSize(PaperSize paperSize)
        {
            if (paperSize != null)
            {               
                FileName = paperSize.PaperName;    

                var inW = paperSize.Width / 100.0;
                var inH = paperSize.Height / 100.0;

                Width = inToMm(inW);
                Height = inToMm(inH);
            }
        }

        private double TruncateDecimal(double value, int precision)
        {
            double step = Math.Pow(10, precision);
            double tmp = Math.Truncate(step * value);
            return tmp / step;
        }

        private double mmToPx(double mmVal)
        {
            return mmToIn(mmVal) * dpi;
        }

        private double mmToIn(double mmVal)
        {
            return mmVal * mmConst;
        }

        private double inToMm(double inVal)
        {
            return inVal * inConst;
        }
    }
}
