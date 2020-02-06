using LabelCreator.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private const double inConst = 25.4;            // 1 in = 25.4 mm 
        private const double mmConst = 0.03937007874;   // 1 mm = 0.03937007874 in

        private int dpi = 96;

        private double width;
        private double height;
        private double widthPx;
        private double heightPx;
        private double widthIn;
        private double heightIn;

        private string fileName = "Canvas1";
        private string selectedPrinter;

        private List<string> installedPrinters;

        private PrinterSettings.PaperSizeCollection paperSizes;
        private PaperSize selectedPaperSizes;

        public NewCanvasViewModel()
        {
            try
            {
                //DbGroupsList = DbHandler.T1GetGroups();
            }
            catch (Exception)
            {

            }
        }

        #region PROPERTIES

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged("FileName"); }
        }

        public List<string> InstalledPrinters
        {
            get { return installedPrinters; }
            set { installedPrinters = value; OnPropertyChanged("InstalledPrinters"); }
        }

        public string SelectedPrinter
        {
            get { return selectedPrinter; }
            set { selectedPrinter = value; OnPropertyChanged("SelectedPrinter"); LoadLabels(); }
        }
        
        public double Width
        {
            get { return width; }
            set { width = Math.Round(value); OnPropertyChanged("Width"); WidthPx = mmToPx(value); WidthIn = mmToIn(value); }
        }

        public double Height
        {
            get { return height; }
            set { height = Math.Round(value); OnPropertyChanged("Height"); HeightPx = mmToPx(value); HeightIn = mmToIn(value); }
        }

        public double WidthPx
        {
            get { return widthPx; }
            set { widthPx = Math.Round(value, 2); OnPropertyChanged("WidthPx"); }
        }

        public double HeightPx
        {
            get { return heightPx; }
            set { heightPx = Math.Round(value, 2); OnPropertyChanged("HeightPx"); }
        }

        public double WidthIn
        {
            get { return widthIn; }
            set { widthIn = Math.Round(value, 1); OnPropertyChanged("WidthIn"); }
        }

        public double HeightIn
        {
            get { return heightIn; }
            set { heightIn = Math.Round(value, 1); OnPropertyChanged("HeightIn"); }
        }

        public PrinterSettings.PaperSizeCollection PaperSizes
        {
            get { return paperSizes; }
            set { paperSizes = value; OnPropertyChanged("PaperSizes"); }
        }

        public PaperSize SelectedPaperSizes
        {
            get { return selectedPaperSizes; }
            set { selectedPaperSizes = value; OnPropertyChanged("SelectedPaperSizes"); SetCanvasSize(value); }
        }
        
        public int DPI
        {
            get { return dpi; }
            set { dpi = value; OnPropertyChanged("DPI");}
        }

        private bool dbGroups;

        public bool DbGroups
        {
            get { return dbGroups; }
            set 
            {
                dbGroups = value; 
                OnPropertyChanged("DbGroups");

                if(!value)
                {
                    SelectedDbGroups = null;
                }
            }
        }

        private List<t1> dbGroupsList;

        public List<t1> DbGroupsList
        {
            get { return dbGroupsList; }
            set { dbGroupsList = value; OnPropertyChanged("DbGroupsList"); }
        }

        private t1 selectedDbGroups;

        public t1 SelectedDbGroups
        {
            get { return selectedDbGroups; }
            set { selectedDbGroups = value; OnPropertyChanged("SelectedDbGroups"); }
        }

        private Visibility dbGroupsVisibility;

        public Visibility DbGroupsVisibility
        {
            get { return dbGroupsVisibility; }
            set { dbGroupsVisibility = value; OnPropertyChanged("DbGroupsVisibility"); }
        }


        #endregion

        private void LoadLabels()
        {
            var printerSettings = PrinterHandler.GetPrinterSettings(SelectedPrinter);

            PaperSizes = printerSettings.PaperSizes;

            //DPI = printerSettings.PrinterResolutions.Cast<PrinterResolution>().OrderByDescending(pr => pr.X).First().X;
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
            return mmVal * dpi / inConst;
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
