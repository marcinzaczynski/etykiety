using LabelCreator.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace LabelCreator.Helpers
{
    public partial class AppHandler
    {
        private static XNamespace Namespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

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
            var canvas = doc.Descendants(Namespace + "Canvas").FirstOrDefault();

            if(canvas != null)
            {
                try
                {
                    LabelModel lm = new LabelModel();

                    var w = canvas.Attribute("Width").Value;
                    var h = canvas.Attribute("Height").Value;

                    lm.CanvasWidht = Convert.ToDouble(StrToDouble(w));
                    lm.CanvasHeight = Convert.ToDouble(StrToDouble(h));

                    GetLabels(canvas, lm.Components);
                    GetImages(canvas, lm.Components);

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

                var labels = canvas.Descendants(Namespace + "Label").ToList();

                foreach (var lbl in labels)
                {
                    Label newLabel = new Label();
                    TextBlock newTextBlock = new TextBlock();

                    NewTextViewModel newTextViewModel = new NewTextViewModel();

                    var textBlock = lbl.Descendants(Namespace + "TextBlock").FirstOrDefault();

                    if (textBlock != null)
                    {
                        var text = textBlock.Attribute("Text").Value;
                        var fontFamily = textBlock.Attribute("FontFamily").Value;
                        var fontStyle = textBlock.Attribute("FontStyle").Value;
                        var fontWeight = textBlock.Attribute("FontWeight").Value;
                        var fontSize = textBlock.Attribute("FontSize").Value;
                        var fontColor = textBlock.Attribute("Foreground").Value;

                        newTextViewModel.LabelContent = text;
                        newTextViewModel.TbFontFamily = new FontFamily(fontFamily);
                        newTextViewModel.TbFontStyle = fontStyle == "Italic" ? FontStyles.Italic : FontStyles.Normal;
                        newTextViewModel.TbFontWeight = fontWeight == "Bold" ? FontWeights.Bold : FontWeights.Regular;
                        newTextViewModel.TbFontSize = (float)Convert.ToDouble(StrToDouble(fontSize));
                        newTextViewModel.FontColor = new BrushConverter().ConvertFromString(fontColor) as SolidColorBrush;

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

                        var textDecoration = textBlock.Descendants(Namespace + "TextDecoration").ToList();

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

                    var left = lbl.Attribute("Canvas.Left").Value;
                    var top = lbl.Attribute("Canvas.Top").Value;

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


        private static void GetImages(XElement canvas, Dictionary<UIElement, CanvasPosition> list)
        {
            var images = canvas.Descendants(Namespace + "Image").ToList();

            foreach (var img in images)
            {
                Image image = new Image();
                image.Name
            }
        }
    }
}
