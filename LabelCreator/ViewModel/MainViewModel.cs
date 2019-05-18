using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                OnPropertyChanged("MarginWidth"); // odświeżenie marginesów
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
                OnPropertyChanged("MarginHeight"); // odświeżenie marginesów
            }
        }

        // DŁUGOŚĆ MARGINESÓW
        // AdditionalMarginLength - marginesy dłuższe niż obszar roboczy
        public double MarginWidth { get { return CanvasWidth + AdditionalMarginLength; } } 
        public double MarginHeight { get { return CanvasHeight + AdditionalMarginLength; } } 

        // PRZESUNIĘCIE MARGINESÓW POZA CANVAS
        private double marginOffsetSize = MarginOffsetSizeField;
        public double MarginOffsetSize
        {
            get { return marginOffsetSize; }
            set { marginOffsetSize = value; OnPropertyChanged("MarginOffsetSize"); }
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
