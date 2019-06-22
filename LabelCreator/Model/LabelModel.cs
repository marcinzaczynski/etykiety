using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LabelCreator.Helpers
{
    public class LabelModel
    {
        public double CanvasWidht { get; set; }
        public double CanvasHeight { get; set; }
        public Dictionary<Label, CanvasPosition> Labels { get; set; }

    }

    public class CanvasPosition
    {
        public CanvasPosition(double left, double top)
        {
            CanvasLeft = left;
            CanvasTop = top;
        }

        public double CanvasLeft { get; set; }
        public double CanvasTop { get; set; }
    }
}
