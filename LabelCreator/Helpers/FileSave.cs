using LabelCreator.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml.Linq;

namespace LabelCreator.Helpers
{
    public partial class AppHandler
    {
        public static bool SaveCanvas(Canvas mainCanvas)
        {
            var mystrXAML = XamlWriter.Save(mainCanvas);

            mystrXAML = mystrXAML.Replace("<lch:", "<"); // FIX FOR BARCODES

            SaveFileDialog dlg = new SaveFileDialog();

            var dc = mainCanvas.DataContext;

            dlg.FileName = ((MainViewModel)dc).FileName;
            dlg.DefaultExt = ".lblc"; 
            dlg.Filter = "Label Creator document (.lblc)|*.lblc"; 
            dlg.RestoreDirectory = true;

            var result = (bool)dlg.ShowDialog();

            if (result)
            {
                try
                {
                    FileStream filestream = File.Create(dlg.FileName);
                    StreamWriter streamwriter = new StreamWriter(filestream);
                    streamwriter.Write(FormatXml(mystrXAML));
                    streamwriter.Close();
                    filestream.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return result;
        }

        private static string FormatXml(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);

                XNamespace ns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

                // USUNIĘCIE MARGINESÓW Z XMLa
                //doc.Descendants(ns + "Label")
                //    .Where(x=> (string)x.Attribute("Name") == "MarginT" || (string)x.Attribute("Name") == "MarginB" || (string)x.Attribute("Name") == "MarginL" || (string)x.Attribute("Name") == "MarginR")
                //    .Remove();


                return doc.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
