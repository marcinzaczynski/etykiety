using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace LabelCreator.ViewModel
{
    public class NewTextViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        

        public NewTextViewModel()
        {
            //NewLabel = new Label();
            //NewText = new TextBlock();

            //NewLabel.Content = NewText;
        }

        public void Refresh(NewTextViewModel dc)
        {
            Name = dc.Name;            
            Border = dc.Border;
            BorderColor = dc.BorderColor;
            BorderThickness = dc.BorderThickness;
            FontColor = dc.FontColor;
            LabelContent = dc.LabelContent;
            TbFontFamily = dc.TbFontFamily;
            TbFontSize = dc.TbFontSize;
            TbFontStyle = dc.TbFontStyle;
            TbFontWeight = dc.TbFontWeight;
            TbTextDecorations = dc.TbTextDecorations;
        }

        //private Label newLabel;

        //public Label NewLabel
        //{
        //    get { return newLabel; }
        //    set { newLabel = value; OnPropertyChanged("NewLabel"); }
        //}

        //private TextBlock newText;
        //public TextBlock NewText
        //{
        //    get { return newText; }
        //    set { newText = value; OnPropertyChanged("NewText"); }
        //}

        
        private bool editMode = false;

        // ZAPAMIĘTYWANIE CZCIONKI DLA TEKSTU
        private FontDialog fontDialog;

        public FontDialog FontDialog
        {
            get { return fontDialog; }
            set { fontDialog = value; OnPropertyChanged("FontDialog"); }
        }

        public bool EditMode
        {
            get { return editMode; }
            set { editMode = value; OnPropertyChanged("EditMode"); }
        }

        private string name = "TEXT_001";
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged("Name"); }
        }        

        private string labelContent;
        public string LabelContent
        {
            get { return labelContent; }
            set { labelContent = value; /*NewText.Text = value;*/ OnPropertyChanged("LabelContent"); }
        }                

        private bool border;

        public bool Border
        {
            get { return border; }
            set
            {
                border = value;
                OnPropertyChanged("Border");

                if (value)
                {
                    BorderColor = FontColor;
                    //BorderThickness = 1;
                }
                else
                {
                    BorderColor = new SolidColorBrush(Colors.White);
                    BorderThickness = 0;
                }
                
            }
        }

        private SolidColorBrush borderColor = new SolidColorBrush(Colors.White);
        public SolidColorBrush BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; OnPropertyChanged("BorderColor"); }
        }

        private SolidColorBrush fontColor = new SolidColorBrush(Colors.Black);
        public SolidColorBrush FontColor
        {
            get { return fontColor; }
            set { fontColor = value; OnPropertyChanged("FontColor"); BorderColor = value; }
        }

        private int borderThickness;
        public int BorderThickness
        {
            get { return borderThickness; }
            set { borderThickness = value; OnPropertyChanged("BorderThickness"); }
        }

        private FontFamily tbFontFamily;
        public FontFamily TbFontFamily
        {
            get { return tbFontFamily; }
            set { tbFontFamily = value; OnPropertyChanged("TbFontFamily"); }
        }

        private float tbFontSize;
        public float TbFontSize
        {
            get { return tbFontSize; }
            set { tbFontSize = value; OnPropertyChanged("TbFontSize"); }
        }


        private FontWeight tbFontWeight;
        public FontWeight TbFontWeight
        {
            get { return tbFontWeight; }
            set { tbFontWeight = value; OnPropertyChanged("TbFontWeight"); }
        }

        private FontStyle tbFontStyle;
        public FontStyle TbFontStyle
        {
            get { return tbFontStyle; }
            set { tbFontStyle = value; OnPropertyChanged("TbFontStyle"); }
        }

        private TextDecorationCollection tbTextDecorations;
        public TextDecorationCollection TbTextDecorations
        {
            get { return tbTextDecorations; }
            set { tbTextDecorations = value; OnPropertyChanged("TbTextDecorations"); }
        }




        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
