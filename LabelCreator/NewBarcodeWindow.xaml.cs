using LabelCreator.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for NewBarcodeWindow.xaml
    /// </summary>
    public partial class NewBarcodeWindow : Window
    {
        public static event AddNewComponent NewBarcodeEvent;
        public static event EditComponent EditBarcodeEvent;

        private bool EditMode = false;

        public NewBarcodeWindow(BarcodeControl barcodeToEdit = null)
        {
            InitializeComponent();

            if(barcodeToEdit != null)
            {
                EditMode = true;
                NewBarcodeVM.SetBarcodeTmp(barcodeToEdit);
            }
        }

        private void CommanOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(EditMode)
            {
                EditBarcodeEvent?.Invoke(NewBarcodeVM.BarcodeTmp);
            }
            else
            {
                NewBarcodeEvent?.Invoke(NewBarcodeVM.BarcodeTmp, 2, 2, false);
            }

            Close();
        }

        private void CommandOk_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(NewBarcodeVM.ImgSource != null)
            {
                e.CanExecute = true;
            }
        }

        private void CommanCancel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void CommandCancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
