using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Markup;
using Microsoft.Win32;
using System.IO;
using LabelCreator.ViewModel;
using System.Xml.Linq;
using System.Windows.Data;
using System.Threading;
using System.Management;

namespace LabelCreator.Helpers
{
    public partial class AppHandler
    {
        //public static Rectangle DrawMargin(double width)
        //{
        //    Rectangle rect = new Rectangle();

        //    rect.Stroke = new SolidColorBrush(Colors.Red);
        //    rect.StrokeThickness = 1;
        //    rect.Fill = new SolidColorBrush(Colors.Red);
        //    rect.Width = width;
        //    rect.Height = 2;

        //    return rect;
        //}

        public static void BindData(Label NewText, TextBlock NewTextBlock)
        {
            Binding l1 = new Binding("BorderColor");
            BindingOperations.SetBinding(NewText, Label.BorderBrushProperty, l1);

            Binding l2 = new Binding("BorderThickness");
            BindingOperations.SetBinding(NewText, Label.BorderThicknessProperty, l2);

            Binding l3 = new Binding("TbWidth");
            BindingOperations.SetBinding(NewText, Label.WidthProperty, l3);

            Binding l4 = new Binding("TbHeight");
            BindingOperations.SetBinding(NewText, Label.HeightProperty, l4);

            Binding l5 = new Binding("TbHorizontalContentAligment");
            BindingOperations.SetBinding(NewText, Label.HorizontalContentAlignmentProperty, l5);

            Binding l6 = new Binding("TbVerticalContentAligment");
            BindingOperations.SetBinding(NewText, Label.VerticalContentAlignmentProperty, l6);

            Binding t1 = new Binding("LabelContent");
            BindingOperations.SetBinding(NewTextBlock, TextBlock.TextProperty, t1);

            Binding t2 = new Binding("TbFontFamily");
            BindingOperations.SetBinding(NewTextBlock, TextBlock.FontFamilyProperty, t2);

            Binding t3 = new Binding("TbFontSize");
            BindingOperations.SetBinding(NewTextBlock, TextBlock.FontSizeProperty, t3);

            Binding t4 = new Binding("FontColor");
            BindingOperations.SetBinding(NewTextBlock, TextBlock.ForegroundProperty, t4);

            Binding t5 = new Binding("TbFontWeight");
            BindingOperations.SetBinding(NewTextBlock, TextBlock.FontWeightProperty, t5);

            Binding t6 = new Binding("TbFontStyle");
            BindingOperations.SetBinding(NewTextBlock, TextBlock.FontStyleProperty, t6);

            Binding t7 = new Binding("TbTextDecorations");
            BindingOperations.SetBinding(NewTextBlock, TextBlock.TextDecorationsProperty, t7);           
        }                       

        // TYLKO LICZBY 
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        public static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        public static double StrToDouble(string number)
        {
            var separator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            var tmp = number.Replace(".", separator).Replace(",", separator);

            var dec = Convert.ToDouble(tmp);

            return dec;
        }

        public static string LoadImage()
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            openDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";

            var result = openDialog.ShowDialog();

            if(result == true)
            {
                return openDialog.FileName;
            }

            return null;
        }
        
    }
}
