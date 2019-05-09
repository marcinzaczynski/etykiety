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
        private Point clickPosition;

        private bool isDragging;

        double FirstXPos, FirstYPos, FirstArrowXPos, FirstArrowYPos;
        object MovingObject;
        //Line Path1, Path2, Path3, Path4;
        //Rectangle FirstPosition, CurrentPosition;

        public MainWindow()
        {
            InitializeComponent();

            //foreach (Control control in DesigningCanvas.Children)
            //{
            //    control.PreviewMouseLeftButtonDown += this.MouseLeftButtonDown;
            //    control.PreviewMouseLeftButtonUp += this.PreviewMouseLeftButtonUp;
            //    control.Cursor = Cursors.Hand;
            //}

            //// Setting the MouseMove event for our parent control(In this case it is DesigningCanvas).
            this.PreviewMouseMove += this.MouseMove;

            //// Setting up the Lines that we want to show the path of movement
            //List<Double> Dots = new List<double>();
            //Dots.Add(1);
            //Dots.Add(2);
            //Path1 = new Line();
            //Path1.Width = DesigningCanvas.Width;
            //Path1.Height = DesigningCanvas.Height;
            //Path1.Stroke = Brushes.DarkGray;
            //Path1.StrokeDashArray = new DoubleCollection(Dots);

            //Path2 = new Line();
            //Path2.Width = DesigningCanvas.Width;
            //Path2.Height = DesigningCanvas.Height;
            //Path2.Stroke = Brushes.DarkGray;
            //Path2.StrokeDashArray = new DoubleCollection(Dots);

            //Path3 = new Line();
            //Path3.Width = DesigningCanvas.Width;
            //Path3.Height = DesigningCanvas.Height;
            //Path3.Stroke = Brushes.DarkGray;
            //Path3.StrokeDashArray = new DoubleCollection(Dots);

            //Path4 = new Line();
            //Path4.Width = DesigningCanvas.Width;
            //Path4.Height = DesigningCanvas.Height;
            //Path4.Stroke = Brushes.DarkGray;
            //Path4.StrokeDashArray = new DoubleCollection(Dots);

            //FirstPosition = new Rectangle();
            //FirstPosition.Stroke = Brushes.DarkGray;
            //FirstPosition.StrokeDashArray = new DoubleCollection(Dots);

            //CurrentPosition = new Rectangle();
            //CurrentPosition.Stroke = Brushes.DarkGray;
            //CurrentPosition.StrokeDashArray = new DoubleCollection(Dots);

            //// Adding Lines to main designing panel(Canvas)
            //DesigningCanvas.Children.Add(Path1);
            //DesigningCanvas.Children.Add(Path2);
            //DesigningCanvas.Children.Add(Path3);
            //DesigningCanvas.Children.Add(Path4);
            //DesigningCanvas.Children.Add(FirstPosition);
            //DesigningCanvas.Children.Add(CurrentPosition);
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

        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //isDragging = true;
            //var draggableControl = sender as Control;
            //clickPosition = e.GetPosition(this);
            //draggableControl.CaptureMouse();


            //In this event, we get current mouse position on the control to use it in the MouseMove event.
            FirstXPos = e.GetPosition(sender as Control).X;
            FirstYPos = e.GetPosition(sender as Control).Y;
            FirstArrowXPos = e.GetPosition((sender as Control).Parent as Control).X - FirstXPos;
            FirstArrowYPos = e.GetPosition((sender as Control).Parent as Control).Y - FirstYPos;
            MovingObject = sender;
        }


        void PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //isDragging = false;
            //var draggable = sender as Control;
            //draggable.ReleaseMouseCapture();

            // In this event, we should set the lines visibility to Hidden
            MovingObject = null;

            //Path1.Visibility = System.Windows.Visibility.Hidden;
            //Path2.Visibility = System.Windows.Visibility.Hidden;
            //Path3.Visibility = System.Windows.Visibility.Hidden;
            //Path4.Visibility = System.Windows.Visibility.Hidden;
            //FirstPosition.Visibility = System.Windows.Visibility.Hidden;
            //CurrentPosition.Visibility = System.Windows.Visibility.Hidden;
        }

        private new void MouseMove(object sender, MouseEventArgs e)
        {
            // JEŚLI WCIŚNIĘTY LEWY PRZYCISK MYSZY NA JAKIMŚ KOMPONENCIE
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if(MovingObject is FrameworkElement tempMovingObject)
                {
                    var xNewPosition = e.GetPosition((tempMovingObject).Parent as FrameworkElement).X - FirstXPos;
                    var yNewPosition = e.GetPosition((tempMovingObject).Parent as FrameworkElement).Y - FirstYPos;

                    if (xNewPosition > 0 && xNewPosition + tempMovingObject.ActualWidth < DesigningCanvas.ActualWidth)
                    {
                        (MovingObject as FrameworkElement).SetValue(Canvas.LeftProperty, xNewPosition);
                    }

                    if (yNewPosition > 0 && yNewPosition + tempMovingObject.ActualHeight < DesigningCanvas.ActualHeight )
                    {
                        (MovingObject as FrameworkElement).SetValue(Canvas.TopProperty, yNewPosition);
                    }
                }                
            }
        }

        
    }
}
