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
using System.Windows.Forms;
using Label = System.Windows.Controls.Label;
using LabelCreator.Helpers;
using LabelCreator.ViewModel;
using Binding = System.Windows.Data.Binding;
using MessageBox = System.Windows.MessageBox;

namespace LabelCreator
{
    /// <summary>
    /// Interaction logic for NewTextWindow.xaml
    /// </summary>
    public partial class NewTextWindow : Window
    {
        public static event AddNewComponent NewTextEvent;
        public static event EditComponent EditEvent;

        public Label NewText;
        public TextBlock NewTextBlock;

        FontDialog fontDialog = new FontDialog();

        public NewTextWindow(Label lbl = null)
        {
            InitializeComponent();

            if (lbl != null)
            {
                NewTextVM.EditMode = true;

                var dc = lbl.DataContext as NewTextViewModel;

                NewTextVM.Refresh(dc);

                fontDialog = dc.FontDialog;
            }

            NewText = new Label();
            NewTextBlock = new TextBlock();
            NewText.Content = NewTextBlock;

            AppHandler.BindData(NewText, NewTextBlock);

            NewText.DataContext = NewTextVM;
        }        
        
        private void CommanOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NewTextVM.FontDialog = fontDialog;

            NewText.Name = NewTextVM.Name;

            if (NewTextVM.EditMode)
            {
                //NewTextVM.

                EditEvent?.Invoke(NewText);
            }
            else
            {
                var nameExist = MainWindow.ComponentList.Where(c => c.Name == NewTextVM.Name).FirstOrDefault();

                if (nameExist == null)
                {
                    NewTextEvent.Invoke(NewText, 2, 2, false);
                }
                else
                {
                    MessageBox.Show($"Komponent o nazwie {NewTextVM.Name} jest już dodany. Zmień nazwię.", "Dodawanie komponentu", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            Close();
        }

        private void CommandOk_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NewTextVM.Name))
            {
                e.CanExecute = true;
            }
        }

        private void CommanCancel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NewText = null;
            Close();
        }

        private void CommandCancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            NewText = null;
        }

        private void ButtonChangeFont(object sender, RoutedEventArgs e)
        {
            fontDialog.ShowColor = true;
            fontDialog.ShowApply = false;
            fontDialog.ShowEffects = true;
            fontDialog.ShowHelp = false;
            fontDialog.AllowScriptChange = false;

            if (fontDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                NewTextVM.FontColor = new SolidColorBrush(Color.FromArgb(fontDialog.Color.A, fontDialog.Color.R, fontDialog.Color.G, fontDialog.Color.B));
                NewTextVM.TbFontFamily = new FontFamily(fontDialog.Font.Name);
                NewTextVM.TbFontSize = fontDialog.Font.Size;
                NewTextVM.TbFontWeight = fontDialog.Font.Bold ? FontWeights.Bold : FontWeights.Regular;
                NewTextVM.TbFontStyle = fontDialog.Font.Italic ? FontStyles.Italic : FontStyles.Normal;
                //TextBlockPreview.Foreground = NewTextVM.FontColor;

                TextDecorationCollection tdc = new TextDecorationCollection();
                if (fontDialog.Font.Underline) tdc.Add(TextDecorations.Underline);
                if (fontDialog.Font.Strikeout) tdc.Add(TextDecorations.Strikethrough);
                NewTextVM.TbTextDecorations = tdc;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AppHandler.IsTextAllowed(e.Text);
        }
    }
}
