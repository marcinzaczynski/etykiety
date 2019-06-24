using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LabelCreator.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {

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

        // SZEROKOŚĆ OBSZARU ROBOCZEGO CANVAS
        private double canvasWidth;
        public double CanvasWidth
        {
            get { return canvasWidth; }
            set
            {
                canvasWidth = value;
                OnPropertyChanged("CanvasWidth");

                // odświeżenie długości i pozycje marginesów
                OnPropertyChanged("MarginWidth"); 
                MarginDefaultLeft = 0;
                MarginDefaultRight = value;
            }
        }

        // WYSOKOŚĆ OBSZARU ROBOCZEGO CANVAS
        private double canvasHeight;
        public double CanvasHeight
        {
            get { return canvasHeight; }
            set
            {
                canvasHeight = value;
                OnPropertyChanged("CanvasHeight");

                // odświeżenie długości i pozycje marginesów
                OnPropertyChanged("MarginHeight");
                MarginDefaultTop = 0;
                MarginDefaultBottom = value;
            }
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
        private Visibility marginVisibility;
        public Visibility MarginVisibility
        {
            get { return marginVisibility; }
            set { marginVisibility = value; OnPropertyChanged("MarginVisibility"); }
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

        #endregion
        // =====================================================================


        // ========================= PROPERTY CHANGE ===========================
        #region 
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
        // =====================================================================
    }
}
