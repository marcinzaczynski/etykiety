using OwnBarcode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace LabelCreator.Helpers
{
    public enum BarcodeTypes
    {

    }

    public class BarcodeHandler
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string SavedCodesDirectory
        {
            get
            {
                return Path.Combine(AssemblyDirectory, "SavedCodes");
            }
        }

        public static BitmapImage GenerateCode(BarcodeFormat codeFormat, string codeText, int width, int height, bool pureCode, int margin = 6)
        {
            var barcodeWriter = new BarcodeWriter
            {
                Renderer = new BitmapRenderer(),
                Format = codeFormat,
                Options = new EncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = margin,
                    PureBarcode = pureCode,
                }
            };

            barcodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
            barcodeWriter.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "utf-8");
            //barcodeWriter.Options.Hints.Add(EncodeHintType.MARGIN, 0);

            return GetBitmap(barcodeWriter, codeText);
        }

        private static BitmapImage GetBitmap(BarcodeWriter barcodeWriter, string value)
        {
            using (var bitmap = barcodeWriter.Write(value))
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                stream.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = stream;
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.EndInit();

                return bi;
            };
        }


        public static BitmapImage GenerateBarcode(TYPE codeType, string codeText, double width, double height, bool pureCode)
        {
            Barcode b = new Barcode();

            b.IncludeLabel = !pureCode;

            return ImgToBitmap(b.Encode(codeType, codeText, (int)width, (int)height));
        }

        private static BitmapImage ImgToBitmap(Image img)
        {
            using (var memory = new MemoryStream())
            {
                img.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        public static string BitmapToFile(BitmapImage image, string fileName)
        {
            string filePath = Path.Combine(SavedCodesDirectory);

            if(!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            filePath = Path.Combine(filePath, fileName + ".png");

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(fileStream);
            }

            return filePath;
        }
    }
}
