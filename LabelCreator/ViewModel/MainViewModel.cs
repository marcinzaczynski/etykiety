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
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private double canvasWidth;

        public double CanvasWidth
        {
            get { return canvasWidth; }
            set { canvasWidth = value; OnPropertyChanged("CanvasWidth"); }
        }

        private double canvasHeight;

        public double CanvasHeight
        {
            get { return canvasHeight; }
            set { canvasHeight = value; OnPropertyChanged("CanvasHeight"); }
        }

        public double MarginWidth { get { return canvasWidth + 20; } }
        public double MarginHeight { get { return CanvasHeight + 20; } }

    }
}
