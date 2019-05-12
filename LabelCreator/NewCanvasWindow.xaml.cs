using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
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
    /// Interaction logic for NewCanvasWindow.xaml
    /// </summary>
    public partial class NewCanvasWindow : Window
    {
        public NewCanvasWindow()
        {
            InitializeComponent();

            var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");

            foreach (var printer in printerQuery.Get())
            {
                var name = printer.GetPropertyValue("Name");
                var status = printer.GetPropertyValue("Status");
                var isDefault = printer.GetPropertyValue("Default");
                var isNetworkPrinter = printer.GetPropertyValue("Network");

                var str = $"{name} (Status: {status}, Default: {isDefault}, Network: {isNetworkPrinter}";

                ListBoxPrinters.Items.Add(str);
            }
        }
    }
}
