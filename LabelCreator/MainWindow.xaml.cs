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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LabelCreator.Helpers;
using LabelCreator.ViewModel;

namespace LabelCreator
{
    public delegate void AddNewComponent(FrameworkElement newElement, double left, double top);
    public delegate void EditComponent(FrameworkElement element);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<FrameworkElement> ComponentList = new List<FrameworkElement>();

        // wartość przeskoku skalowania
        double SliderChangeVal = 0.2;

        // Współrzędne przesówanego komponentu
        double FirstXPos, FirstYPos;

        // współrzędne kursora myszki
        double FirstArrowXPos, FirstArrowYPos;

        // Przeciągany element na Canvasie
        object MovingObject;


        public MainWindow()
        {
            InitializeComponent();

            NewTextWindow.NewTextEvent += new AddNewComponent(AddComponentToCanvas);
            NewTextWindow.EditEvent += new EditComponent(EditComponent);

            // Setting the MouseMove event for our parent control(In this case it is DesigningCanvas).
            PreviewMouseMove += this.MouseMove;

            PreviewMouseLeftButtonUp += this.OnPreviewMouseLeftButtonUp;
        }

        private void Command_NewText(object sender, ExecutedRoutedEventArgs e)
        {
            var newTextWindow = new NewTextWindow();

            newTextWindow.ShowDialog();

            //if(newTextWindow.NewText != null)
            //{
            //    AddComponentToCanvas(newTextWindow.NewText);
            //}
        }


        private void AddComponentToCanvas(FrameworkElement control, double left = 5, double top = 5)
        {
            // Sprawdzenie czy canvas zawiera już komponent o takiej samej nazwie
            //var controlExist = MainCanvas.Children.Cast<FrameworkElement>().Where(c => c.Name == control.Name).FirstOrDefault();

            if(control is Label lbl)
            {
                TreeViewItemTextsRoot.Items.Add(((NewTextViewModel)lbl.DataContext).Name);
                TreeViewItemTextsRoot.IsExpanded = true; 
            }

            ComponentList.Add(control);

            MainCanvas.Children.Add(control);

            Canvas.SetLeft(control, left);
            Canvas.SetTop(control, top);

            control.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            control.PreviewMouseLeftButtonUp += this.OnPreviewMouseLeftButtonUp;
            control.Cursor = Cursors.Hand;

        }

        private void EditComponent(FrameworkElement control)
        {
            if (control.DataContext is NewTextViewModel vm)
            {
                var componentToEdit = GetCanvasComponentByName(vm.Name);

                if (componentToEdit != null)
                {
                    double tmpLeft = Canvas.GetLeft(componentToEdit);
                    double tmpTop = Canvas.GetTop(componentToEdit);

                    ComponentList.Remove(componentToEdit);
                    MainCanvas.Children.Remove(componentToEdit);

                    AddComponentToCanvas(control, tmpLeft, tmpTop);

                    //MainCanvas.Children.Add(control);

                    //Canvas.SetTop(control, tmpTop);
                    //Canvas.SetLeft(control, tmpLeft);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Label lbl = new Label();
            lbl.Width = 100;
            lbl.Height = 40;
            lbl.Content = "new label";
            lbl.BorderBrush = Brushes.Black;
            lbl.BorderThickness = new Thickness(1);
        }

        private void ButtonMarginesy_Click(object sender, RoutedEventArgs e)
        {
            var x = MainCanvas.ActualWidth;
            var y = MainCanvas.ActualHeight;

            //var marginTop = AppHandler.DrawMargin(x + 10);
            //var marginTop = new Label();
            //marginTop.Width = x + 10;
            //marginTop.Height = 1;
            //marginTop.BorderBrush = new SolidColorBrush(Colors.Red);
            //marginTop.BorderThickness = new Thickness(2);
            //marginTop.Name = "MarginTop";
            //DesigningCanvas.Children.Add(marginTop);
            //Canvas.SetLeft(marginTop, -5);
            //Canvas.SetTop(marginTop, 5);
            //marginTop.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            //marginTop.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;
            //marginTop.Cursor = Cursors.SizeNS;

            //var marginBottom = AppHandler.DrawMargin(x + 10);
            //marginBottom.Name = "MarginBottom";
            //DesigningCanvas.Children.Add(marginBottom);
            //Canvas.SetLeft(marginBottom, -5);
            //Canvas.SetTop(marginBottom, y-5);
            //marginBottom.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            //marginBottom.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;
            //marginBottom.Cursor = Cursors.SizeNS;
        }


        #region COMMAND

        private void Command_NewCanvas(object sender, ExecutedRoutedEventArgs e)
        {
            NewCanvasWindow cnw = new NewCanvasWindow();

            cnw.ShowDialog();

            if (cnw.WindowResult)
            {
                MainVM.FileName = cnw.NewCanvasVM.FileName;
                MainVM.CanvasHeight = cnw.NewCanvasVM.Height;
                MainVM.CanvasWidth = cnw.NewCanvasVM.Width;

                MainCanvas.UpdateLayout();
            }

            // Drukowanie Canvas
            //var dialog = new PrintDialog();
            //if (dialog.ShowDialog() == true)
            //{
            //    dialog.PrintVisual(DesigningCanvas, "Printing canvas");
            //}
        }

        private void Command_FileSave(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                AppHandler.SaveCanvas(MainCanvas);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Zapis etykiety", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }        

        private void Command_EditComponent(object sender, ExecutedRoutedEventArgs e)
        {
            var currentComponent = GetCanvasComponentByName(MainVM.CurrentComponentName);

            if (currentComponent is Label lbl)
            {
                NewTextWindow newTextWindow = new NewTextWindow(lbl);

                newTextWindow.ShowDialog();
            }
        }

        private void Command_CanEditComponent(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainVM.CurrentComponentName))
            {
                e.CanExecute = true;
            }
        }

        private void Command_Exit(object sender, ExecutedRoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion 

        private void DesigningCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Delta > 0)
                {
                    SliderCanvasZoom.Value += SliderChangeVal;
                }
                else
                {
                    SliderCanvasZoom.Value -= SliderChangeVal;
                }
            }
        }

        private void SliderCanvasZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var newVal = e.NewValue;

            CanvasScaleTransform.ScaleX = newVal;
            CanvasScaleTransform.ScaleY = newVal;
        }

        private void Button_ChangeCanvasZoom(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                switch (btn.Tag)
                {
                    case "Plus":
                        SliderCanvasZoom.Value += SliderChangeVal;
                        break;
                    case "Minus":
                        SliderCanvasZoom.Value -= SliderChangeVal;
                        break;
                    case "Default":
                        SliderCanvasZoom.Value = 1;
                        break;
                    default:
                        break;
                }
            }
        }


        private new void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowClickedControlInfo(sender);
            //In this event, we get current mouse position on the control to use it in the MouseMove event.
            FirstXPos = e.GetPosition(sender as Control).X;
            FirstYPos = e.GetPosition(sender as Control).Y;
            FirstArrowXPos = e.GetPosition(MainCanvas).X - FirstXPos;
            FirstArrowYPos = e.GetPosition(MainCanvas).Y - FirstYPos;
            MovingObject = sender;
        }

        private void ShowClickedControlInfo(object sender)
        {
            if (sender is FrameworkElement fe)
            {
                if (fe.DataContext is NewTextViewModel vm)
                {
                    MainVM.CurrentComponentName = vm.Name;
                }
            }
        }
        
        private new void MouseMove(object sender, MouseEventArgs e)
        {
            // JEŚLI WCIŚNIĘTY LEWY PRZYCISK MYSZY NA JAKIMŚ KOMPONENCIE
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (MovingObject is FrameworkElement tmp)
                {
                    var tmpParent = tmp.Parent as FrameworkElement;

                    var xNewPosition = e.GetPosition(tmpParent).X - FirstXPos;
                    var yNewPosition = e.GetPosition(tmpParent).Y - FirstYPos;

                    var lMarginPos = tmp.Name.Contains("MarginL") ? 0 : (double)MarginL.GetValue(Canvas.LeftProperty);
                    var rMarginPos = tmp.Name.Contains("MarginR") ? MainCanvas.ActualWidth : (double)MarginR.GetValue(Canvas.LeftProperty);

                    // PRZESÓWANIE PO OSI X - LEWO, PRAWO - OGRANICZENIE DO ROZMIARÓW CANVASA LUB MARGINESÓW
                    if (xNewPosition > lMarginPos && xNewPosition + tmp.ActualWidth < rMarginPos)
                    {
                        tmp.SetValue(Canvas.LeftProperty, xNewPosition);
                    }

                    var tMarginPos = tmp.Name.Contains("MarginT") ? 0 : (double)MarginT.GetValue(Canvas.TopProperty);
                    var bMarginPos = tmp.Name.Contains("MarginB") ? MainCanvas.ActualHeight : (double)MarginB.GetValue(Canvas.TopProperty);

                    // PRZESÓWANIE PO OSI Y - GÓRA, DÓŁ - OGRANICZENIE DO ROZMIARÓW CANVASA LUB MARGINESÓW 
                    if (yNewPosition > tMarginPos && yNewPosition + tmp.ActualHeight < bMarginPos)
                    {
                        tmp.SetValue(Canvas.TopProperty, yNewPosition);
                    }
                }
            }
        }       

        private FrameworkElement GetCanvasComponentByName(string name)
        {
            return MainCanvas.Children.Cast<FrameworkElement>().Where(c => c.DataContext is NewTextViewModel).ToList().Where(cc => ((NewTextViewModel)cc.DataContext).Name == name).FirstOrDefault();
        }

        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MovingObject = null;
        }

    }
}
