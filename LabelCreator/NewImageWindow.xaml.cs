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
    /// Interaction logic for NewImageWindow.xaml
    /// </summary>
    public partial class NewImageWindow : Window
    {
        public static event AddNewComponent NewImageEvent;
        public static event EditComponent EditEvent;

        public NewImageWindow()
        {
            InitializeComponent();
        }

        private void CommanOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandOk_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        private void CommanCancel_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandCancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }
    }
}
