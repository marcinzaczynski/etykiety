using LabelCreator.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LabelCreator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // ========================= PROPERTY CHANGE ===========================
        #region 
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
        // =====================================================================

        public MainViewModel()
        {
            LoadControlListTree();
        }

        // ========================= FIELDS ====================================
        #region

        // DODATKOWA DŁUGOŚĆ MARGINESU - marginesy wychodzą poza Canvas na rogach
        private const int AdditionalMarginLength = 10;

        // WYSUNIĘCIE MARGINESÓW POZA CANVAS
        private const int MarginOffsetSizeField = -5;
        #endregion
        // =====================================================================


        // ========================= PROPERTIES ================================
        #region

        // NAZWA PLIKU 
        private string fileName = "Canvas1";
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged("FileName"); }
        }

        // SZEROKOŚĆ OBSZARU ROBOCZEGO CANVAS W PX
        private double canvasWidth;
        public double CanvasWidth
        {
            get { return canvasWidth; }
            set
            {
                canvasWidth = Math.Round(value);
                OnPropertyChanged("CanvasWidth");

                // odświeżenie długości i pozycje marginesów
                OnPropertyChanged("MarginWidth");
                MarginDefaultLeft = 0;
                MarginDefaultRight = canvasWidth;

                //CanvasWidthMM = Math.Round(pxToMm(canvasWidth));
                //CanvasWidthIN = Math.Round(pxToIn(canvasWidth), 2);
            }
        }

        private double canvasWidthMM;
        public double CanvasWidthMM
        {
            get { return canvasWidthMM; }
            set 
            { 
                canvasWidthMM = Math.Round(value);
                OnPropertyChanged("CanvasWidthMM");

                CanvasWidth = mmToPx(canvasWidthMM);
                CanvasWidthIN = mmToIn(canvasWidthMM);
            }
        }

        private double canvasWidthIN;
        public double CanvasWidthIN
        {
            get { return canvasWidthIN; }
            set { canvasWidthIN = Math.Round(value, 2); OnPropertyChanged("CanvasWidthIN"); }
        }


        // WYSOKOŚĆ OBSZARU ROBOCZEGO CANVAS W PX
        private double canvasHeight;
        public double CanvasHeight
        {
            get { return canvasHeight; }
            set
            {
                canvasHeight = Math.Round(value);
                OnPropertyChanged("CanvasHeight");

                // odświeżenie długości i pozycje marginesów
                OnPropertyChanged("MarginHeight");
                MarginDefaultTop = 0;
                MarginDefaultBottom = canvasHeight;

                //CanvasHeightMM = Math.Round(pxToMm(canvasHeight));
                //CanvasHeightIN = Math.Round(pxToIn(canvasHeight));
            }
        }

        private double canvasHeightMM;
        public double CanvasHeightMM
        {
            get { return canvasHeightMM; }
            set
            {
                canvasHeightMM = Math.Round(value);
                OnPropertyChanged("CanvasHeightMM");

                CanvasHeight = mmToPx(canvasHeightMM);
                CanvasHeightIN = mmToIn(canvasHeightMM);
            }
        }

        private double canvasHeightIN;
        public double CanvasHeightIN
        {
            get { return canvasHeightIN; }
            set { canvasHeightIN = Math.Round(value,2); OnPropertyChanged("CanvasHeightIN"); }
        }


        // DŁUGOŚĆ MARGINESÓW
        // AdditionalMarginLength - marginesy dłuższe niż obszar roboczy
        public double MarginWidth { get { return CanvasWidth + AdditionalMarginLength; } }
        public double MarginHeight { get { return CanvasHeight + AdditionalMarginLength; } }

        // DOMYŚLNA POZYCJA MARGINESÓW
        private double marginDefaultTop;
        public double MarginDefaultTop
        {
            get { return marginDefaultTop; }
            set { marginDefaultTop = value; OnPropertyChanged("MarginDefaultTop"); }
        }

        private double marginDefaultBottom;
        public double MarginDefaultBottom
        {
            get { return marginDefaultBottom; }
            set { marginDefaultBottom = value; OnPropertyChanged("MarginDefaultBottom"); }
        }

        private double marginDefaultRight;
        public double MarginDefaultRight
        {
            get { return marginDefaultRight; }
            set { marginDefaultRight = value; OnPropertyChanged("MarginDefaultRight"); }
        }

        private double marginDefaultLeft;
        public double MarginDefaultLeft
        {
            get { return marginDefaultLeft; }
            set { marginDefaultLeft = value; OnPropertyChanged("MarginDefaultLeft"); }
        }

        // WIDOCZNOŚĆ MARGINESÓW 
        private Visibility marginVisibility = Visibility.Visible;
        public Visibility MarginVisibility
        {
            get { return marginVisibility; }
            set { marginVisibility = value; OnPropertyChanged("MarginVisibility"); }
        }

        private bool hideMargins;

        public bool HiedeMargins
        {
            get { return hideMargins; }
            set
            {
                hideMargins = value;
                OnPropertyChanged("HiedeMargins");
                switch (value)
                {
                    case true:
                        MarginVisibility = Visibility.Collapsed;
                        break;
                    case false:
                        MarginVisibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
        }



        // PRZESUNIĘCIE MARGINESÓW POZA CANVAS
        private double marginOffsetSize = MarginOffsetSizeField;
        public double MarginOffsetSize
        {
            get { return marginOffsetSize; }
            set { marginOffsetSize = value; OnPropertyChanged("MarginOffsetSize"); }
        }

        // AKTUALNIE KLIKNIĘTY KOMPONENT NA CANVASIE
        private string currentComponentName;
        public string CurrentComponentName
        {
            get { return currentComponentName; }
            set { currentComponentName = value; OnPropertyChanged("CurrentComponentName"); }
        }

        private void LoadControlListTree()
        {
            colntrolList = new ObservableCollection<OwnControl>();

            //add Root items
            ControlList.Add(new OwnControl { ControlName = "Tekst" });
            ControlList.Add(new OwnControl { ControlName = "Obraz" });
            ControlList.Add(new OwnControl { ControlName = "Kod kreskowy" });

            ControlList[0].Childrens = new ObservableCollection<OwnControl>();
            ControlList[1].Childrens = new ObservableCollection<OwnControl>();
            ControlList[2].Childrens = new ObservableCollection<OwnControl>();

            //ControlList[0].Childrens.Add(new OwnControl { ControlName = "Test11"});
            //ControlList[1].Childrens.Add(new OwnControl { ControlName = "Test21"});
            //ControlList[2].Childrens.Add(new OwnControl { ControlName = "Test31"});

        }

        private ObservableCollection<OwnControl> colntrolList;
        public ObservableCollection<OwnControl> ControlList
        {
            get { return colntrolList; }
            set
            {
                colntrolList = value;
                OnPropertyChanged("ControlList");
            }
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

        private PrinterSettings.PaperSizeCollection paperSizes;
        public PrinterSettings.PaperSizeCollection PaperSizes
        {
            get { return paperSizes; }
            set { paperSizes = value; OnPropertyChanged("PaperSizes"); }
        }

        private int dpi = 96;

        public int DPI
        {
            get { return dpi; }
            set { dpi = value; OnPropertyChanged("DPI"); CanvasWidthMM = CanvasWidthMM; CanvasHeightMM = CanvasHeightMM; }
        }


        private PaperSize selectedPaperSizes;
        public PaperSize SelectedPaperSizes
        {
            get { return selectedPaperSizes; }
            set { selectedPaperSizes = value; OnPropertyChanged("SelectedPaperSizes"); SetCanvasSize(value); }
        }

        #endregion
        // =====================================================================

        private const double inConst = 25.4;            // 1 in = 25.4 mm 
        private const double mmConst = 0.03937007874;   // 1 mm = 0.03937007874 in

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

                CanvasWidthMM = inToMm(inW);
                CanvasHeightMM = inToMm(inH);
            }
        }

        public double inToMm(double inVal)
        {
            return inVal * inConst;
        }

        public double mmToPx(double mmVal)
        {
            return mmVal * DPI / inConst;
        }

        public double mmToIn(double mmVal)
        {
            return mmVal * mmConst;
        }

        public double pxToIn(double pxVal)
        {
            return pxVal / DPI;
        }

        public double pxToMm(double pxVal)
        {
            return inToMm(pxToIn(pxVal));
        }
    }

    public class OwnControl : INotifyPropertyChanged
    {
        public string ControlName { get; set; }
        public ObservableCollection<OwnControl> Childrens { get; set; }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; OnPropertyChanged("IsSelected"); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class OwnPaperSize : INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
