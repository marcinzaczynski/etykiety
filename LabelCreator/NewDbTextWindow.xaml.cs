using LabelCreator.Model;
using LabelCreator.ViewModel;
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
        public static event AddNewComponent NewDbTextEvent;
                
        public bool WindowResult = false;

        private int idGrupaTmp;

        public NewDbTextWindow(int idGrupa)
        {
            InitializeComponent();

            idGrupaTmp = idGrupa;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NewDbTextVM.Firma = "Ładowanie danych...";
            Mouse.OverrideCursor = Cursors.Wait;
            Task.Run(() =>
            {
                NewDbTextVM.IdGrupa = idGrupaTmp;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
            });
        }

        private void CommanOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var DbText = new DbTextModel();
            var DbTextContent = new TextBlock();
            var Dc = new NewTextViewModel();

            var fixName = DateTime.Now.ToString("yyyyMMddHHmmss");

            DbTextContent.Text = Dc.LabelContent = NewDbTextVM.SelectedElem.wartosc;
            DbText.Name = Dc.Name = $"DBT_{fixName}";
            DbText.Width = Dc.LabelContent.Count() * 8;
            DbText.Height = 30;
            DbText.Id_Pole = NewDbTextVM.SelectedElem.id_pole;
            DbText.Content = DbTextContent;
            DbText.DataContext = Dc;

            NewDbTextEvent?.Invoke(DbText, 5, 5);

            WindowResult = true;
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
