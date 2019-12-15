using LabelCreator.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace LabelCreator.ViewModel
{
    public class NewImageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private Image TmpImage = new Image();

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); TmpImage.Name = value; }
        }

        private string imgageSource;

        public string ImageSource
        {
            get { return imgageSource; }
            set { imgageSource = value; OnPropertyChanged("ImageSource"); TmpImage.Source = new ImageSourceConverter().ConvertFromString(value) as ImageSource; }
        }



        public bool LoadImage()
        {            
            var imgSrc = AppHandler.LoadImage();

            if(imgSrc != null)
            {
                Name = $"IMG_{Path.GetFileName(imgSrc)}".Replace(" ", "_").Replace("-","_").Replace(".","_");
                ImageSource = imgSrc;
                return true;
            }

            return false;
        }

        public Image GetImage()
        {
            return TmpImage;
        }
    }
}
