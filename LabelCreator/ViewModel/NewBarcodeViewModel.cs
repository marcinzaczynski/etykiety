using LabelCreator.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ZXing;

namespace LabelCreator.ViewModel
{
    class NewBarcodeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public static ToolTipMsg Message = new ToolTipMsg();

        private int codeWidth = 150;

        public int CodeWidth
        {
            get { return codeWidth; }
            set { codeWidth = value; OnPropertyChanged("CodeWidth"); RefreshCode(); }
        }

        private int codeHeight = 150;

        public int CodeHeight
        {
            get { return codeHeight; }
            set { codeHeight = value; OnPropertyChanged("CodeHeight"); RefreshCode(); }
        }

        private int codeMargin = 6;

        public int CodeMargin
        {
            get { return codeMargin; }
            set { codeMargin = value; OnPropertyChanged("CodeMargin"); RefreshCode(); }
        }

        private string codeText = "1234567890123";

        public string CodeText
        {
            get { return codeText; }
            set { codeText = value; OnPropertyChanged("CodeText"); RefreshCode(); }
        }

        private bool pureCode =false;

        public bool PureCode
        {
            get { return pureCode; }
            set { pureCode = value; OnPropertyChanged("PureCode"); RefreshCode(); }
        }

        public IEnumerable<BarcodeFormat> CodeFormatList { get; set; } = Enum.GetValues(typeof(BarcodeFormat)).Cast<BarcodeFormat>();

        private BarcodeFormat selectedCodeFormat;

        public BarcodeFormat SelectedCodeFormat
        {
            get { return selectedCodeFormat; }
            set { selectedCodeFormat = value; OnPropertyChanged("SelectedCodeFormat"); RefreshCode(); }
        }

        private BitmapImage imgSource;

        public BitmapImage ImgSource
        {
            get { return imgSource; }
            set { imgSource = value; OnPropertyChanged("ImgSource"); }
        }

        private string textHint = "";

        public string TextHint
        {
            get { return textHint; }
            set { textHint = value; OnPropertyChanged("TextHint"); }
        }


        private void RefreshCode()
        {
            try
            {
                TextHint = "";

                if (SelectedCodeFormat != 0)
                {
                    if (!string.IsNullOrWhiteSpace(CodeText))
                    {
                        ImgSource = BarcodeHandler.GenerateCode(SelectedCodeFormat, CodeText, CodeWidth, CodeHeight, PureCode, CodeMargin);

                        BarcodeHandler.BarcodeLib();
                    }
                    else
                    {

                    }
                }
                else
                {
                    TextHint = "Wybierz typ kodu";
                }
            }
            catch (Exception ex)
            {
                ImgSource = null;
                TextHint = ex.Message;
            }
        }
    }

    public class ToolTipMsg
    {
        public ToolTipMsg()
        {

        }
    }
}
