using OwnBarcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace LabelCreator.Helpers
{
    public class BarcodeControl : Image
    {
        public BarcodeControl() : base()
        {
            this.SnapsToDevicePixels = true;
        }

        //public string CodeName { get; set; }

        public TYPE CodeType { get; set; }

        public string CodeText { get; set; }

        public int CodeMargin { get; set; }

        public bool PureCode { get; set; }


    }
}
