using LabelCreator.ViewModel;
using Microsoft.Win32;
using OwnBarcode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace LabelCreator.Helpers
{
    public partial class AppHandler
    {
        private static XNamespace NamespaceCanvas = "clr-namespace:LabelCreator;assembly=LabelCreator";
        private static XNamespace ns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

        public static LabelModel OpenFile()
        {
            LabelModel labelModel = null;

            var fileDialog = new OpenFileDialog();

            fileDialog.DefaultExt = ".lblc";
            fileDialog.Filter = "Label Creator document (.lblc)|*.lblc";
            
            bool? result = fileDialog.ShowDialog();
 
            if (result == true)
            {
                string filename = fileDialog.FileName;

                try
                {
                    XDocument doc = XDocument.Parse(File.ReadAllText(filename));

                    labelModel = ParseXml(doc);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Read file text", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return labelModel;
        }

        private static LabelModel ParseXml(XDocument doc)
        {
            var canvas = doc.Descendants(NamespaceCanvas + "CanvasForLabel").FirstOrDefault();

            if(canvas != null)
            {
                try
                {
                    LabelModel lm = new LabelModel();

                    var w = canvas.Attribute("Width").Value;
                    var h = canvas.Attribute("Height").Value;

                    var idGrupa = canvas.Attribute("id_grupa")?.Value;
                    var idPole = canvas.Attribute("id_pole")?.Value;

                    lm.CanvasWidht = Convert.ToDouble(StrToDouble(w));
                    lm.CanvasHeight = Convert.ToDouble(StrToDouble(h));

                    lm.Id_Grupa = Convert.ToInt32(idGrupa);

                    GetLabels(canvas, lm.Components);
                    GetImages(canvas, lm.Components);
                    GetBarcodes(canvas, lm.Components);

                    return lm;
                }
                catch (Exception ex )
                {
                    throw ex; 
                }                
            }


            throw new Exception("Nie rozpoznano głownego elementu Canvas z pliku");
        }               

        private static void GetLabels(XElement canvas, Dictionary<UIElement, CanvasPosition> list)
        {
            try
            {
                //Dictionary<UIElement, CanvasPosition> list = new Dictionary<UIElement, CanvasPosition>();

                var labels = canvas.Descendants(ns + "Label").ToList();

                foreach (var lbl in labels)
                {
                    Label newLabel = new Label();
                    TextBlock newTextBlock = new TextBlock();

                    NewTextViewModel newTextViewModel = new NewTextViewModel();

                    var textBlock = lbl.Descendants(ns + "TextBlock").FirstOrDefault();

                    if (textBlock != null)
                    {
                        var text = textBlock.Attribute("Text").Value;
                        var fontFamily = textBlock.Attribute("FontFamily").Value;
                        var fontStyle = textBlock.Attribute("FontStyle").Value;
                        var fontWeight = textBlock.Attribute("FontWeight").Value;
                        var fontSize = textBlock.Attribute("FontSize").Value;
                        var fontColor = textBlock.Attribute("Foreground").Value;
                        var hca = lbl.Attribute("HorizontalContentAlignment")?.Value;
                        var vca = lbl.Attribute("VerticalContentAlignment")?.Value;

                        newTextViewModel.LabelContent = text;
                        newTextViewModel.TbFontFamily = new FontFamily(fontFamily);
                        newTextViewModel.TbFontStyle = fontStyle == "Italic" ? FontStyles.Italic : FontStyles.Normal;
                        newTextViewModel.TbFontWeight = fontWeight == "Bold" ? FontWeights.Bold : FontWeights.Regular;
                        newTextViewModel.TbFontSize = (float)Convert.ToDouble(StrToDouble(fontSize));
                        newTextViewModel.FontColor = new BrushConverter().ConvertFromString(fontColor) as SolidColorBrush;
                        newTextViewModel.TbHorizontalContentAligment = hca != null? GetHorizontalAligment(hca) : HorizontalAlignment.Left;
                        newTextViewModel.TbVerticalContentAligment = vca != null ? GetVerticalAligment(vca) : VerticalAlignment.Top;

                        // UTWORZENIE OKNA DIALOGOWEGO DO WYBIERANIA CZCIONKI
                        System.Drawing.FontStyle fs1 = System.Drawing.FontStyle.Regular;
                        System.Drawing.FontStyle fs2 = System.Drawing.FontStyle.Regular;
                        System.Drawing.FontStyle fs3 = System.Drawing.FontStyle.Regular;
                        System.Drawing.FontStyle fs4 = System.Drawing.FontStyle.Regular;
                        if (fontStyle == "Italic")
                        {
                            fs1 = System.Drawing.FontStyle.Italic;
                        }

                        if(fontWeight == "Bold")
                        {
                            fs2 = System.Drawing.FontStyle.Bold;
                        }                        

                        var textDecoration = textBlock.Descendants(ns + "TextDecoration").ToList();

                        if(textDecoration != null)
                        {
                            TextDecorationCollection tdc = new TextDecorationCollection();

                            foreach (var decoration in textDecoration)
                            {
                                var d = decoration.Attribute("Location").Value;

                                if (d == "Underline")
                                {
                                    tdc.Add(TextDecorations.Underline);
                                    fs3 = System.Drawing.FontStyle.Underline;
                                }

                                if (d == "Strikethrough")
                                {
                                    tdc.Add(TextDecorations.Strikethrough);
                                    fs4 = System.Drawing.FontStyle.Strikeout;
                                }
                            }

                            newTextViewModel.TbTextDecorations = tdc;
                        }

                        newTextViewModel.FontDialog = new System.Windows.Forms.FontDialog();
                        newTextViewModel.FontDialog.Color = System.Drawing.ColorTranslator.FromHtml(fontColor);
                        newTextViewModel.FontDialog.Font = new System.Drawing.Font(fontFamily, (float)Convert.ToDouble(StrToDouble(fontSize)), fs1 | fs2 | fs3 | fs4);

                        newLabel.Content = newTextBlock;
                    }

                    var name = lbl.Attribute("Name").Value;

                    newLabel.Name = name;
                    newTextViewModel.Name = name;                    

                    // POBRANIE WŁAŚCIWOŚCI MARGINESÓW 
                    if (!name.Contains("Margin"))
                    {
                        var borderBrush = lbl.Attribute("BorderBrush").Value;
                        var borderThick = lbl.Attribute("BorderThickness").Value;
                        var thickness = Convert.ToInt32(borderThick.Split(',')[0]);

                        newTextViewModel.BorderColor = new BrushConverter().ConvertFromString(borderBrush) as SolidColorBrush;
                        newTextViewModel.BorderThickness = thickness;
                        if(thickness > 0)
                        {
                            newTextViewModel.Border = true;
                        }
                    }

                    //var width = lbl.Attribute("Width")?.Value;
                    //var height = lbl.Attribute("Height")?.Value;

                    //newLabel.Width = Convert.ToDouble(StrToDouble(width));
                    //newLabel.Height = Convert.ToDouble(StrToDouble(height));                    

                    var left = lbl.Attribute(ns + "Canvas.Left").Value;
                    var top = lbl.Attribute(ns + "Canvas.Top").Value;

                    var canvasLeft = Convert.ToDouble(StrToDouble(left));
                    var canvasTop = Convert.ToDouble(StrToDouble(top));
                    
                    newLabel.DataContext = newTextViewModel;

                    AppHandler.BindData(newLabel, newTextBlock);

                    list.Add(newLabel, new CanvasPosition(canvasLeft, canvasTop));
                }

                //return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas przetwarzania komponentów typu Label. " + ex.Message);
            }            
        }
        
        private static HorizontalAlignment GetHorizontalAligment(string val)
        {
            var result = Enum.TryParse(val, out HorizontalAlignment aligment);
            
            if (result)
            {
                return aligment;
            }
            else
            {
                return HorizontalAlignment.Left;
            }
        }

        private static VerticalAlignment GetVerticalAligment(string val)
        {
            var result = Enum.TryParse(val, out VerticalAlignment aligment);

            if (result)
            {
                return aligment;
            }
            else
            {
                return VerticalAlignment.Top;
            }
        }

        private static void GetImages(XElement canvas, Dictionary<UIElement, CanvasPosition> list)
        {
            var images = canvas.Descendants(ns + "Image").ToList();

            // poprawić odczyt img z pliku 
            

            foreach (var img in images)
            {
                try
                {
                    Image image = new Image();
                    NewImageViewModel newImageViewModel = new NewImageViewModel();

                    newImageViewModel.Name = img.Attribute("Name").Value;
                    newImageViewModel.ImageSource = img.Attribute("Source").Value.Replace("file:///", "");

                    image.Name = newImageViewModel.Name;
                    image.Source = new BitmapImage(new Uri(newImageViewModel.ImageSource));

                    var left = img.Attribute("Canvas.Left").Value;
                    var top = img.Attribute("Canvas.Top").Value;

                    var canvasLeft = Convert.ToDouble(StrToDouble(left));
                    var canvasTop = Convert.ToDouble(StrToDouble(top));

                    image.DataContext = newImageViewModel;

                    list.Add(image, new CanvasPosition(canvasLeft, canvasTop));
                }
                catch (Exception ex)
                {
                    throw new Exception("Błąd podczas przetwarzania komponentów typu Image. " + ex.Message);
                }                
            }
        }

        private static void GetBarcodes(XElement canvas, Dictionary<UIElement, CanvasPosition> list)
        {
            var codes = canvas.Descendants(NamespaceCanvas + "BarcodeControl").ToList();

            foreach (var code in codes)
            {
                try
                {
                    var bc = new BarcodeControl();

                    bc.CodeType = (TYPE)Enum.Parse(typeof(TYPE),code.Attribute("CodeType").Value);
                    bc.CodeText = code.Attribute("CodeText").Value;
                    bc.CodeMargin = Convert.ToInt32(code.Attribute("CodeMargin").Value);
                    bc.PureCode = Convert.ToBoolean(code.Attribute("PureCode").Value);
                    bc.Source = new BitmapImage(new Uri(code.Attribute("Source").Value.Replace("file:///", "")));
                    bc.Name = code.Attribute("Name").Value;
                    bc.Width = Convert.ToDouble(code.Attribute("Width").Value);
                    bc.Height = Convert.ToDouble(code.Attribute("Height").Value);
                    bc.Cursor = Cursors.Hand;

                    var canvasLeft = Convert.ToDouble(StrToDouble(code.Attribute(ns + "Canvas.Left").Value));
                    var canvasTop = Convert.ToDouble(StrToDouble(code.Attribute(ns + "Canvas.Top").Value));

                    list.Add(bc, new CanvasPosition(canvasLeft, canvasTop));
                }
                catch (Exception ex)
                {
                    throw new Exception("Błąd podczas przetwarzania komponentów typu Barcode. " + ex.Message);
                }
            }
        }
    }
}
