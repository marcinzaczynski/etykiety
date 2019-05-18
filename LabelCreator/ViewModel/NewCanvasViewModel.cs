using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelCreator.ViewModel
{
    public class NewCanvasViewModel : INotifyPropertyChanged
    {
        public NewCanvasViewModel()
        {

        }

        private string fileName = "Canvas1";
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged("FileName"); }
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
