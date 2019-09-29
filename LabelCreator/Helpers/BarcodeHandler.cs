using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;

namespace LabelCreator.Helpers
{
    public class BarcodeHandler
    {
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


        public static void BarcodeLib()
        {
            //Barcode b = new Barcode();
        }
    }
}
