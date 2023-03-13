using System.Globalization;

namespace OutlayManagerPortableMaui.Converter
{
    public class DoubleToAmountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string euroValue = value?.ToString() ?? "NaN";

            return $"{euroValue} €";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
