using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace OutlayManagerPortable.Converter
{
    public class DoubleToAmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueStr = value?.ToString() ?? String.Empty;
            string resultStr = String.Empty;

            if (Double.TryParse(valueStr,out double result))
                resultStr = $"{result} €";

            return resultStr;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
