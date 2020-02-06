using LabelCreator.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TextBox = System.Windows.Controls.TextBox;

namespace LabelCreator
{
    /// <summary>
    /// Interaction logic for NewCanvasWindow.xaml
    /// </summary>
    public partial class NewCanvasWindow : Window
    {
        public bool WindowResult = false;

        public NewCanvasWindow()
        {
            InitializeComponent();

            SetPrintersList();                        
        }

        private void SetPrintersList()
        {
            NewCanvasVM.InstalledPrinters = new List<string>();

            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                NewCanvasVM.InstalledPrinters.Add(printer);
            }            
        }        

        private void CommandOk_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // NAZWA MUSI BYĆ WPISANA A ROZMIAR ETYKIETY WIĘKSZY OD 0
            if(!string.IsNullOrWhiteSpace(NewCanvasVM.FileName) && NewCanvasVM.Width > 0 && NewCanvasVM.Height > 0)
            {
                // JEŚLI ZAZNACZONY CHECKBOX FIRMA Z BAZY TO MUSI BYĆ JESZCZE WYBRANA FIRMA Z LISTY
                if(NewCanvasVM.DbGroups && NewCanvasVM.SelectedDbGroups == null)
                {
                    return;
                }

                e.CanExecute = true;
            }
        }

        private void CommanOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            WindowResult = true;
            Close();
        }

        private void CommandCancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommanCancel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }


        private void Double_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox tb)
            {
                // ZABEZPIECZENIE PRZED WPROWADZENIEM WIELU PRZECINKÓW
                if (e.Text.Contains(",") || e.Text.Contains("."))
                {
                    if (tb.Text.Contains(",") || tb.Text.Contains("."))
                    {
                        e.Handled = true;
                        return;
                    }
                }
                e.Handled = !IsTextAllowedDot(e.Text);
                return;
            }
            e.Handled = !IsTextAllowedDot(e.Text);
        }

        private Boolean IsTextAllowedDot(String text)
        {
            return Array.TrueForAll<Char>(text.ToCharArray(),
                 delegate (Char c) { return Char.IsDigit(c) || Char.IsControl(c) || c == '.' || c == ','; });
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.SelectAll();
            }
        }
    }
}
