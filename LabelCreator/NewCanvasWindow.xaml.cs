using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LabelCreator
{
    /// <summary>
    /// Interaction logic for NewCanvasWindow.xaml
    /// </summary>
    public partial class NewCanvasWindow : Window
    {
        public NewCanvasWindow()
        {
            InitializeComponent();

            var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");

            foreach (var printer in printerQuery.Get())
            {
                var name = printer.GetPropertyValue("Name");
                var status = printer.GetPropertyValue("Status");
                var isDefault = printer.GetPropertyValue("Default");
                var isNetworkPrinter = printer.GetPropertyValue("Network");

                var str = $"{name} (Status: {status}, Default: {isDefault}, Network: {isNetworkPrinter}";

                ListBoxPrinters.Items.Add(str);
            }
        }

        private void Double_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //var decimalseparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            //if (e.Text.Equals(",") || e.Text.Equals("."))
            //{
            //    var tb = sender as TextBox;

            //    if (!tb.Text.Contains("."))
            //    {
            //        tb.Text += ".";
            //        tb.SelectionStart = tb.Text.Length;
            //    }

            //    e.Handled = true;
            //}

            //string s1 = "12.34";
            //string s2 = "15,75";

            //var c1 = Convert.ToDecimal(s1, CultureInfo.InvariantCulture);
            //var c2 = Convert.ToDecimal(s2, CultureInfo.InvariantCulture);

            //var d1 = decimal.Parse(s1, CultureInfo.InvariantCulture);
            //var d2 = decimal.Parse(s2, CultureInfo.InvariantCulture);

            //Decimal dec = 85.0m;
            //string s = dec.ToString(CultureInfo.InvariantCulture);
            //decimal.Parse(s, CultureInfo.InvariantCulture);

            //NumberFormatInfo nfi = NumberFormatInfo.CurrentInfo;
            //Regex regex = new Regex(@"^[0-9]([\.\,][0-9]{1,3})?$");
            //e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));

            //bool approvedDecimalPoint = false;

            //if (e.Text == decimalseparator)
            //{
            //    if (!((TextBox)sender).Text.Contains(decimalseparator))
            //    {
            //        approvedDecimalPoint = true;
            //    }
            //}

            //if (!(char.IsDigit(e.Text[0]) || approvedDecimalPoint))
            //{
            //    e.Handled = true;
            //}

            //((TextBox)sender).Text = ((TextBox)sender).Text.Replace(" ", "");

            if (!char.IsDigit(e.Text[0]))
            {
                e.Handled = true;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(sender is TextBox tb)
            {
                tb.SelectAll();
            }
        }

    }
}
