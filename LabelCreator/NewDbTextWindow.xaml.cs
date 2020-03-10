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
    /// Interaction logic for NewDbTextWindow.xaml
    /// </summary>
    public partial class NewDbTextWindow : Window
    {
        public NewDbTextWindow(int idGrupa)
        {
            InitializeComponent();

            NewDbTextVM.IdGrupa = idGrupa;
        }

        private void CommanOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {


            Close();
        }

        private void CommandOk_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(NewDbTextVM.SelectedElem != null)
            {
                e.CanExecute = true;
            }
        }

        private void CommanCancel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
