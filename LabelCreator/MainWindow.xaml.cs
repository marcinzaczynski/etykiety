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

namespace LabelCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Współrzędne przesówanego komponentu
        double FirstXPos, FirstYPos;

        // współrzędne kursora myszki
        double FirstArrowXPos, FirstArrowYPos;

        // Przeciągany element na Canvasie
        object MovingObject;


        public MainWindow()
        {
            InitializeComponent();

            // Setting the MouseMove event for our parent control(In this case it is DesigningCanvas).
            this.PreviewMouseMove += this.MouseMove;
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            //if(sender is Label lbl)
            //{
            //    lbl.BorderBrush = Brushes.Black;
            //    lbl.BorderThickness = new Thickness(1);
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Label lbl = new Label();
            lbl.Width = 100;
            lbl.Height = 40;
            lbl.Content = "new label";
            lbl.BorderBrush = Brushes.Black;
            lbl.BorderThickness = new Thickness(1);
            DesigningCanvas.Children.Add(lbl);
            Canvas.SetLeft(lbl, 0);
            Canvas.SetTop(lbl, 0);
            lbl.MouseEnter += this.Label_MouseEnter;
            lbl.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            lbl.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;
            lbl.Cursor = Cursors.Hand;
        }


        private void Command_NewCanvas(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private new void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            //In this event, we get current mouse position on the control to use it in the MouseMove event.
            FirstXPos = e.GetPosition(sender as Control).X;
            FirstYPos = e.GetPosition(sender as Control).Y;
            FirstArrowXPos = e.GetPosition((sender as Control).Parent as Control).X - FirstXPos;
            FirstArrowYPos = e.GetPosition((sender as Control).Parent as Control).Y - FirstYPos;
            MovingObject = sender;
        }


        private void DesigningCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Delta > 0)
                {
                    SliderCanvasZoom.Value += 0.2;
                }
                else
                {
                    SliderCanvasZoom.Value -= 0.2;
                }
            }
        }

        private void SliderCanvasZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var newVal = e.NewValue;

            CanvasScaleTransform.ScaleX = newVal;
            CanvasScaleTransform.ScaleY = newVal;

        }

        private new void PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MovingObject = null;
        }

        private new void MouseMove(object sender, MouseEventArgs e)
        {
            // JEŚLI WCIŚNIĘTY LEWY PRZYCISK MYSZY NA JAKIMŚ KOMPONENCIE
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (MovingObject is FrameworkElement tempMovingObject)
                {
                    var xNewPosition = e.GetPosition((tempMovingObject).Parent as FrameworkElement).X - FirstXPos;
                    var yNewPosition = e.GetPosition((tempMovingObject).Parent as FrameworkElement).Y - FirstYPos;

                    if (xNewPosition > 0 && xNewPosition + tempMovingObject.ActualWidth < DesigningCanvas.ActualWidth)
                    {
                        (MovingObject as FrameworkElement).SetValue(Canvas.LeftProperty, xNewPosition);
                    }

                    if (yNewPosition > 0 && yNewPosition + tempMovingObject.ActualHeight < DesigningCanvas.ActualHeight)
                    {
                        (MovingObject as FrameworkElement).SetValue(Canvas.TopProperty, yNewPosition);
                    }
                }
            }
        }


    }
}
