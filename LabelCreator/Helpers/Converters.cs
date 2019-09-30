using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LabelCreator.Helpers
{
    public class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }

            string[] parameters = parameter.ToString().Split(',');

            int beforComa = System.Convert.ToInt32(parameters[0]);
            int afterComa = System.Convert.ToInt32(parameters[1]);

            var parseOk = ConvertObjectToDouble(value, out double d);

            d = Math.Round(d, afterComa, MidpointRounding.AwayFromZero);

            return d.ToString("F" + System.Convert.ToInt32(parameters[1], CultureInfo.CurrentCulture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] parameters = parameter.ToString().Split(',');

            int beforComa = System.Convert.ToInt32(parameters[0]);
            int afterComa = System.Convert.ToInt32(parameters[1]);

            var parseOk = ConvertObjectToDouble(value, out double d);

            //var d = System.Convert.ToDouble(val, CultureInfo.CurrentCulture);

            if (!parseOk || Math.Abs(d) >= Math.Pow(10, Math.Abs(beforComa)))
            {
                return null;
            }

            //Double.Parse(value.ToString(),CultureInfo.CurrentCulture);
            var v = Math.Round(d, afterComa, MidpointRounding.ToEven);

            return v;
        }

        public bool ConvertObjectToDouble(object value, out double doubleVal)
        {
            var decSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            var val = value.ToString().Replace(".", decSeparator).Replace(",", decSeparator);

            return double.TryParse(val, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out doubleVal);
        }
    }
}
