using LabelCreator.Helpers;
using OwnBarcode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        private ImageSource imgSource;

        private TYPE selectedCodeFormat = TYPE.EAN13;

        private double codeWidth = 220;
        private double codeHeight = 80;

        private int codeMargin = 6;

        private string codeName = "TstBcc";
        private string codeText = "111122223333";
        private string textHint = "";

        private bool pureCode = false;

        /// <summary>
        /// Konstruktor ustawiający parametry obiektu tymczasowego, który zostanie zwrócony do okna głównego
        /// </summary>
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
                CodeType = selectedCodeFormat
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
            ImgSource = barocdeTmp.Source;
            SelectedCodeFormat = barocdeTmp.CodeType;
        }

        /// <summary>
        /// Źródło danych dla kodu.
        /// </summary>
        public ImageSource ImgSource
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
                OnPropertyChanged("CodeName");
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

        //private Image barcodeImage;

        //public Image BarcodeImage
        //{
        //    get { return barcodeImage; }
        //    set { barcodeImage = value; OnPropertyChanged("BarcodeImage"); }
        //}


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
                        // QR
                        //ImgSource = BarcodeHandler.GenerateCode(SelectedCodeFormat, CodeText, CodeWidth, CodeHeight, PureCode, CodeMargin);

                        // BARCODE
                        ImgSource = BarcodeHandler.GenerateBarcode(SelectedCodeFormat, CodeText, CodeWidth, CodeHeight, PureCode);

                        //var i = new Image();
                        //System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)BarcodeHandler.GetBarcodeImage(SelectedCodeFormat, CodeText, CodeWidth, CodeHeight, PureCode);
                        //var bi = new BitmapImage();
                        //using (var ms = new MemoryStream())
                        //{
                        //    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
                        //    ms.Position = 0;                            
                        //    bi.BeginInit();
                        //    bi.CacheOption = BitmapCacheOption.OnLoad;
                        //    bi.StreamSource = ms;
                        //    bi.EndInit();
                        //}
                        //i.Source = bi;
                        //BarcodeImage = i;  
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

        internal void SaveCodeToFile()
        {
            var dateStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            var fileName = $"{SelectedCodeFormat.ToString()}-{CodeText}";//-{dateStamp}

            ImgSource = new ImageSourceConverter().ConvertFromString(BarcodeHandler.BitmapToFile((BitmapImage)ImgSource, fileName)) as ImageSource;
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
