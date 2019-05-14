using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LabelCreator.Helpers
{
    public class AppHandler
    {
        public static Rectangle DrawMargin(double width)
        {
            Rectangle rect = new Rectangle();

            rect.Stroke = new SolidColorBrush(Colors.Red);
            rect.StrokeThickness = 1;
            rect.Fill = new SolidColorBrush(Colors.Red);
            rect.Width = width;
            rect.Height = 2;

            return rect;
        }
    }
}
