using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace OutlayManagerPortable.Converter
{
    public class TypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueType = value?.ToString() ?? String.Empty;

            switch (valueType)
            {
                case "SPENDING":
                    return "expenseArrow.png";

                case "INCOMING":
                    return "incomingArrow.png";
                    
                default:
                    return String.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
