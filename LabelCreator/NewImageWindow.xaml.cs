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
            var img = NewImageVM.GetImage();

            NewImageEvent.Invoke(img, 5, 5, false);

            Close();
        }

        private void CommandOk_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(NewImageVM.Name) && !string.IsNullOrEmpty(NewImageVM.ImageSource))
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

        private void ButtonPickImage_Click(object sender, RoutedEventArgs e)
        {
            NewImageVM.LoadImage();
        }
    }
}
