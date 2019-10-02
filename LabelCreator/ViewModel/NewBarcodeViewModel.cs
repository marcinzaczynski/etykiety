using LabelCreator.Helpers;
using OwnBarcode;
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
        #region PROPERTY CHANGE
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        private BitmapImage imgSource;

        private TYPE selectedCodeFormat = TYPE.EAN13;

        private double codeWidth = 220;
        private double codeHeight = 80;

        private int codeMargin = 6;

        private string codeName = "TstBcc";
        private string codeText = "111122223333";
        private string textHint = "";

        private bool pureCode = false;


        public NewBarcodeViewModel()
        {
            BarcodeTmp = new BarcodeControl()
            {
                Name = CodeName,
                Width = CodeWidth,
                Height = CodeHeight,
                CodeMargin = CodeMargin,
                CodeText = CodeText,
                PureCode = PureCode,
                Source = ImgSource,
                CodeType = selectedCodeFormat,
            };

            RefreshCode();
        }
        
        /// <summary>
        /// Obiekt pomocniczy, który zostaje zwrócony do okna głównego.
        /// </summary>
        public BarcodeControl BarcodeTmp;

        /// <summary>
        /// Ustawienie wszystkich właściwości z obiektu przysłanego z okna głównego
        /// </summary>
        /// <param name="barocdeTmp"></param>
        internal void SetBarcodeTmp(BarcodeControl barocdeTmp)
        {
            CodeName = barocdeTmp.Name;
            CodeWidth = barocdeTmp.Width;
            CodeHeight = barocdeTmp.Height;
            CodeMargin = barocdeTmp.CodeMargin;
            CodeText = barocdeTmp.CodeText;
            PureCode = barocdeTmp.PureCode;
            ImgSource = (BitmapImage)barocdeTmp.Source;
            SelectedCodeFormat = barocdeTmp.CodeType;
        }

        /// <summary>
        /// Źródło danych dla kodu.
        /// </summary>
        public BitmapImage ImgSource
        {
            get { return imgSource; }
            set
            {
                imgSource = value;
                BarcodeTmp.Source = value;
                OnPropertyChanged("ImgSource");
            }
        }

        public string CodeName
        {
            get { return codeName; }
            set
            {
                codeName = value;
                BarcodeTmp.Name = value;
                OnPropertyChanged("ImgSource");
            }
        }

        /// <summary>
        /// Szerokość kodu
        /// </summary>
        public double CodeWidth
        {
            get { return codeWidth; }
            set
            {
                codeWidth = value;
                BarcodeTmp.Width = value;
                OnPropertyChanged("CodeWidth");
                RefreshCode();
            }
        }    

        /// <summary>
        /// Wysokość kodu.
        /// </summary>
        public double CodeHeight
        {
            get { return codeHeight; }
            set
            {
                codeHeight = value;
                BarcodeTmp.Height = value;
                OnPropertyChanged("CodeHeight");
                RefreshCode();
            }
        }        

        public int CodeMargin
        {
            get { return codeMargin; }
            set
            {
                codeMargin = value;
                BarcodeTmp.CodeMargin = value;
                OnPropertyChanged("CodeMargin");
                RefreshCode();
            }
        }        

        public string CodeText
        {
            get { return codeText; }
            set
            {
                codeText = value;
                BarcodeTmp.CodeText = value;
                OnPropertyChanged("CodeText");
                RefreshCode();
            }
        }        

        public bool PureCode
        {
            get { return pureCode; }
            set
            {
                pureCode = value;
                BarcodeTmp.PureCode = value;
                OnPropertyChanged("PureCode");
                RefreshCode();
            }
        }


        public string TextHint
        {
            get { return textHint; }
            set { textHint = value; OnPropertyChanged("TextHint"); }
        }

        //BarcodeFormat
        public IEnumerable<TYPE> CodeFormatList { get; set; } = Enum.GetValues(typeof(TYPE)).Cast<TYPE>();        

        public TYPE SelectedCodeFormat
        {
            get { return selectedCodeFormat; }
            set
            {
                selectedCodeFormat = value;
                BarcodeTmp.CodeType = value;
                OnPropertyChanged("SelectedCodeFormat");
                RefreshCode();
            }
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
                        //ImgSource = BarcodeHandler.GenerateCode(SelectedCodeFormat, CodeText, CodeWidth, CodeHeight, PureCode, CodeMargin);

                        ImgSource = BarcodeHandler.GenerateBarcode(SelectedCodeFormat, CodeText, CodeWidth, CodeHeight, PureCode);
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

        public static ToolTipMsg Message = new ToolTipMsg();
    }

    public class ToolTipMsg
    {
        public ToolTipMsg()
        {

        }
    }
}
