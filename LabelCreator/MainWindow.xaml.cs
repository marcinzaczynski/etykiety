using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Printing;
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
    public delegate void AddNewComponent(FrameworkElement newElement, double left, double top, bool edit = false);
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

        PrintDialog dlg = new PrintDialog();

        public MainWindow()
        {
            InitializeComponent();

            NewTextWindow.NewTextEvent += new AddNewComponent(AddComponentToCanvas);
            NewTextWindow.EditTextEvent += new EditComponent(EditComponent);

            NewImageWindow.NewImageEvent += new AddNewComponent(AddComponentToCanvas);
            NewImageWindow.EditImageEvent += new EditComponent(EditComponent);

            NewBarcodeWindow.NewBarcodeEvent += new AddNewComponent(AddComponentToCanvas);
            NewBarcodeWindow.EditBarcodeEvent += new EditComponent(EditComponent);

            // Setting the MouseMove event for our parent control(In this case it is DesigningCanvas).
            PreviewMouseMove += this.MouseMove;

            PreviewMouseLeftButtonUp += this.OnPreviewMouseLeftButtonUp;

            SetPrintersList();

            
        }

        private void SetPrintersList()
        {
            MainVM.InstalledPrinters = new List<string>();

            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                MainVM.InstalledPrinters.Add(printer);
            }
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

        private void Command_NewImage(object sender, ExecutedRoutedEventArgs e)
        {
            var newImage = new NewImageWindow();

            newImage.ShowDialog();
        }

        private void Command_NewBarcode(object sender, ExecutedRoutedEventArgs e)
        {
            
            var newBarcodeWindow = new NewBarcodeWindow();

            newBarcodeWindow.ShowDialog();
        }

        private void AddComponentToCanvas(FrameworkElement control, double left = 5, double top = 5, bool edit = false)
        {
            // Sprawdzenie czy canvas zawiera już komponent o takiej samej nazwie
            //var controlExist = MainCanvas.Children.Cast<FrameworkElement>().Where(c => c.Name == control.Name).FirstOrDefault();

            // Jeśli edytujemy komponent to nie dodajemy go do lewego menu - on tam już jest.
            if (!edit)
            {
                if (control is Label lbl)
                {
                    MainVM.ControlList[0].Childrens.Add(new OwnControl { ControlName = lbl.Name });
                    //MainVM.TreeComponentsList[0].Children.Add(new TreeComponents() { Header = lbl.Name });

                    //TreeViewItemTextsRoot.Items.Add(lbl.Name);
                }
                else if (control is BarcodeControl bcc)
                {
                    MainVM.ControlList[2].Childrens.Add(new OwnControl { ControlName = bcc.Name });
                    //TreeViewItemBarcodeRoot.Items.Add(bcc.Name);
                    //TreeViewItemBarcodeRoot.IsExpanded = true;
                }
                else if (control is Image img)
                {
                    MainVM.ControlList[1].Childrens.Add(new OwnControl { ControlName = img.Name });
                    //TreeViewItemImageFromFileRoot.Items.Add(img.Name);
                    //TreeViewItemImageFromFileRoot.IsExpanded = true;
                }
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
            //if (control.DataContext is NewTextViewModel vm)
            //{
            //}

            var componentToEdit = GetCanvasComponentByName(control.Name);

            if (componentToEdit != null)
            {
                double tmpLeft = Canvas.GetLeft(componentToEdit);
                double tmpTop = Canvas.GetTop(componentToEdit);

                ComponentList.Remove(componentToEdit);
                MainCanvas.Children.Remove(componentToEdit);

                AddComponentToCanvas(control, tmpLeft, tmpTop, true);

                //MainCanvas.Children.Add(control);

                //Canvas.SetTop(control, tmpTop);
                //Canvas.SetLeft(control, tmpLeft);
            }
        }

        private void ClearCanvas()
        {
            var children = MainCanvas.Children.Cast<FrameworkElement>().Where(c => !c.Name.Contains("Margin")).ToList();

            foreach (FrameworkElement item in children)
            {
                if (!item.Name.Contains("Margin"))
                {
                    MainCanvas.Children.Remove(item);
                }
            }

            MainVM.ControlList[0].Childrens.Clear();
            MainVM.ControlList[1].Childrens.Clear();
            MainVM.ControlList[2].Childrens.Clear();
            ComponentList.Clear();
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
                ClearCanvas();

                MainVM.FileName = cnw.NewCanvasVM.FileName;
                MainVM.CanvasHeightMM = cnw.NewCanvasVM.Height;
                MainVM.CanvasWidthMM = cnw.NewCanvasVM.Width;
                MainVM.SelectedPrinter = cnw.NewCanvasVM.SelectedPrinter;
                MainVM.SelectedPaperSizes = cnw.NewCanvasVM.SelectedPaperSizes;
                //MainVM.DPI = cnw.NewCanvasVM.DPI;
                
                // FIX - wybranie papieru z listy 
                var item = ComboboxPapers.Items.OfType<PaperSize>().FirstOrDefault(x => x.PaperName == cnw.NewCanvasVM.SelectedPaperSizes.PaperName);
                var index = ComboboxPapers.Items.IndexOf(item);
                ComboboxPapers.SelectedIndex = index;


                MainCanvas.UpdateLayout();
            }
        }

        private void Command_FileOpen(object sender, ExecutedRoutedEventArgs e)
        {
            var output = AppHandler.OpenFile();

            if (output != null)
            {
                ClearCanvas();

                MainVM.CurrentComponentName = null;

                MainVM.CanvasHeightMM = MainVM.pxToMm(output.CanvasHeight);
                MainVM.CanvasWidthMM = MainVM.pxToMm(output.CanvasWidht);

                foreach (var component in output.Components)
                {
                    var cmp = component.Key;
                    var pos = component.Value;

                    if (cmp is Label lbl)
                    {
                        if (lbl.Name.Contains("Margin"))
                        {
                            var margin = MainCanvas.FindName(lbl.Name) as Label;
                            Canvas.SetLeft(margin, pos.CanvasLeft);
                            Canvas.SetTop(margin, pos.CanvasTop);
                        }
                        else
                        {
                            AddComponentToCanvas(lbl, pos.CanvasLeft, pos.CanvasTop);
                        }
                    }

                    if (cmp is Image img)
                    {
                        AddComponentToCanvas(img, pos.CanvasLeft, pos.CanvasTop);
                    }
                }
            }
        }

        private void Command_FileSave(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                SliderCanvasZoom.Value = 1;

                var codes = GetCanvasComponents<BarcodeControl>();

                foreach (var code in codes)
                {
                    //BarcodeHandler.BitmapToFile(code.Source, code.CodeType.ToString());
                }

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
                var result = new NewTextWindow(lbl).ShowDialog();
            }
            else if (currentComponent is BarcodeControl bcc)
            {
                var result = new NewBarcodeWindow(bcc).ShowDialog();
            }
            else if (currentComponent is Image img)
            {
                var result = new NewImageWindow().ShowDialog();
            }
        }

        private void Command_CanEditComponent(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainVM.CurrentComponentName))
            {
                e.CanExecute = true;
            }
        }

        private void Command_DeleteComponent(object sender, ExecutedRoutedEventArgs e)
        {
            var result = MessageBox.Show("Czy na pewno chcesz usunąć ten komponent?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var currentComponent = GetCanvasComponentByName(MainVM.CurrentComponentName);

                ComponentList.Remove(currentComponent);
                MainCanvas.Children.Remove(currentComponent);

                if (currentComponent is Label lbl)
                {
                    MainVM.ControlList[0].Childrens.Remove(MainVM.ControlList[0].Childrens.Where(r => r.ControlName == lbl.Name).FirstOrDefault());
                }
                else if (currentComponent is BarcodeControl bcc)
                {
                    MainVM.ControlList[2].Childrens.Remove(MainVM.ControlList[2].Childrens.Where(r => r.ControlName == bcc.Name).FirstOrDefault());
                }
                else if (currentComponent is Image img)
                {
                    MainVM.ControlList[1].Childrens.Remove(MainVM.ControlList[1].Childrens.Where(r => r.ControlName == img.Name).FirstOrDefault());
                }

                MainVM.CurrentComponentName = null;
            }
        }

        private void Command_CanDeleteComponent(object sender, CanExecuteRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(MainVM.CurrentComponentName))
            {
                e.CanExecute = true;
            }
        }

        private void Command_Print(object sender, ExecutedRoutedEventArgs e)
        {
            MainVM.MarginVisibility = Visibility.Hidden;
            SliderCanvasZoom.Value = 1;

            Canvas root = new Canvas();

            root.Width = MainCanvas.Width;
            root.Height = MainCanvas.Height;
            root.UseLayoutRounding = true;

            foreach (UIElement child in MainCanvas.Children)
            {
                var xaml = System.Windows.Markup.XamlWriter.Save(child);

                if (xaml.Contains("PreviewMouseLeftButtonDown")) continue;

                var deepCopy = System.Windows.Markup.XamlReader.Parse(xaml) as UIElement;
                root.Children.Add(deepCopy);
            }

            PrinterHandler.Print(root, MainVM.SelectedPrinter);

            //PrinterHandler.PrintFixed(root, MainVM.SelectedPrinter);


            MainVM.MarginVisibility = Visibility.Visible;
        }

        private void Command_CanPrint(object sender, CanExecuteRoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(MainVM.SelectedPrinter))
            {
                e.CanExecute = true;
            }
        }

        private void Command_Exit(object sender, ExecutedRoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        #endregion

        #region ZOOM AND SLIDER

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

        #endregion

        #region MOVING COMPONENTS ON CANVAS

        // NACIŚNIĘCIE KOMPONENTU I ROZPOCZĘCIE PRZESUNIĘCIA 
        private new void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowClickedControlInfo(sender);

            //In this event, we get current mouse position on the control to use it in the MouseMove event.
            var currentComponentPosition = e.GetPosition(sender as FrameworkElement);
            FirstXPos = currentComponentPosition.X;
            FirstYPos = currentComponentPosition.Y;

            //var mainCanvasPosition = e.GetPosition(MainCanvas);
            //FirstArrowXPos = mainCanvasPosition.X - FirstXPos;
            //FirstArrowYPos = mainCanvasPosition.Y - FirstYPos;

            MovingObject = sender;
        }

        // PRZESUWANIE KOMPONENTU
        private new void MouseMove(object sender, MouseEventArgs e)
        {
            // JEŚLI WCIŚNIĘTY LEWY PRZYCISK MYSZY NA JAKIMŚ KOMPONENCIE
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (MovingObject is FrameworkElement tmp)
                {
                    bool isMargin = false;
                    if (tmp.Name.Contains("Margin")) isMargin = true;

                    var mainCanvasPos = e.GetPosition(MainCanvas);

                    var xNewPosition = mainCanvasPos.X - FirstXPos;
                    var yNewPosition = mainCanvasPos.Y - FirstYPos;

                    var lMargin = tmp.Name.Contains("MarginL");
                    var rMargin = tmp.Name.Contains("MarginR");
                    var tMargin = tmp.Name.Contains("MarginT");
                    var bMargin = tmp.Name.Contains("MarginB");

                    // JEŚLI PRZESÓWAMY MARGINESY TO SideLimit = POZYCJE MARGINESU W PRZECIWNYM WYPADKU SideLimit = POZYCJE BOCZNE CANVASU
                    var LSideLimit = lMargin ? 0 : (double)MarginL.GetValue(Canvas.LeftProperty);
                    var RSideLimit = rMargin ? MainCanvas.ActualWidth : (double)MarginR.GetValue(Canvas.LeftProperty);

                    var TopLimit = tMargin ? 0 : (double)MarginT.GetValue(Canvas.TopProperty);
                    var BotLimit = bMargin ? MainCanvas.ActualHeight : (double)MarginB.GetValue(Canvas.TopProperty);

                    if (MainVM.HiedeMargins)
                    {
                        LSideLimit = 0;
                        RSideLimit = MainCanvas.ActualWidth;
                        TopLimit = 0;
                        BotLimit = MainCanvas.ActualHeight;
                    }

                    // PRZESÓWANIE PO OSI X - LEWO, PRAWO - OGRANICZENIE DO ROZMIARÓW CANVASA LUB MARGINESÓW
                    if (xNewPosition >= LSideLimit)
                    {
                        if (xNewPosition + tmp.ActualWidth <= RSideLimit)
                        {
                            // PRZESUNIĘCIE KOMPONENTU W POZIOMIE ZGODNIE Z RUCHEM MYSZKI
                            tmp.SetValue(Canvas.LeftProperty, xNewPosition);
                        }
                        else
                        {
                            // POPRAWIA PROBLEM NIEDOCIĄGNIĘCIA KOMPONENTU DO PRAWEGO MARGINESU PRZY SZYBKIM PRZESUNIĘCIU MYSZKĄ
                            if (tMargin || bMargin) ; else tmp.SetValue(Canvas.LeftProperty, RSideLimit - tmp.ActualWidth);
                        }
                    }
                    else
                    {
                        // POPRAWIA PROBLEM NIEDOCIĄGNIĘCIA KOMPONENTU DO LEWWEGO MARGINESU PRZY SZYBKIM PRZESUNIĘCIU MYSZKĄ
                        if (tMargin || bMargin) ; else tmp.SetValue(Canvas.LeftProperty, LSideLimit);
                    }

                    // PRZESÓWANIE PO OSI Y - GÓRA, DÓŁ - OGRANICZENIE DO ROZMIARÓW CANVASA LUB MARGINESÓW 
                    if (yNewPosition >= TopLimit)
                    {
                        if (yNewPosition + tmp.ActualHeight <= BotLimit)
                        {
                            // PRZESUNIĘCIE KOMPONENTU W PIONIE ZGODNIE Z RUCHEM MYSZKI
                            tmp.SetValue(Canvas.TopProperty, yNewPosition);
                        }
                        else
                        {
                            if (isMargin) return;
                            // POPRAWIENIE DOCIĄGNIĘCIA DO DOLNEGO MARGINESU
                            tmp.SetValue(Canvas.TopProperty, BotLimit - tmp.ActualHeight);
                        }
                    }
                    else
                    {
                        if (isMargin) return;
                        // POPRAWIENIE DOCIĄGNIĘCIA DO GÓRNEGO MARGINESU
                        tmp.SetValue(Canvas.TopProperty, TopLimit);
                    }
                }
            }
        }

        // PUSZCZENIE PRZESUWANEGO KOMPONENTU
        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MovingObject = null;
        }
        #endregion

        private void ShowClickedControlInfo(object sender)
        {
            if (sender is FrameworkElement fe)
            {
                if (fe.DataContext is NewTextViewModel tvm)
                {
                    MainVM.CurrentComponentName = tvm.Name;
                    SelectControlOnTree(fe.Name, 0);
                }

                if (fe.DataContext is NewImageViewModel ivm)
                {
                    MainVM.CurrentComponentName = ivm.Name;
                    SelectControlOnTree(fe.Name, 1);
                }

                if (fe is BarcodeControl bcc)
                {
                    MainVM.CurrentComponentName = bcc.Name;
                    SelectControlOnTree(fe.Name, 2);
                }
            }
        }

        private void SelectControlOnTree(string name, int type)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            var toSelect = MainVM.ControlList[type].Childrens.Where(r => r.ControlName == name).FirstOrDefault();

            if (toSelect != null)
            {
                toSelect.IsSelected = true;
            }
        }

        private FrameworkElement GetCanvasComponentByName(string name)
        {
            //return MainCanvas.Children.Cast<FrameworkElement>().Where(c => c.DataContext is NewTextViewModel).ToList().Where(cc => ((NewTextViewModel)cc.DataContext).Name == name).FirstOrDefault();

            return MainCanvas.Children.Cast<FrameworkElement>().Where(c => c.Name == name).FirstOrDefault();
        }
        

        private List<T> GetCanvasComponents<T>() where T : FrameworkElement
        {
            return MainCanvas.Children.OfType<T>().ToList();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is OwnControl oc)
            {
                var currentComponent = GetCanvasComponentByName(oc.ControlName);

                if (currentComponent != null)
                {
                    if (currentComponent is Control cn)
                    {
                        //cn.BorderThickness = new Thickness(2);
                        //cn.BorderBrush = BorderBrush = Brushes.Black;
                        return;
                    }
                }
            }
        }
    }
}
