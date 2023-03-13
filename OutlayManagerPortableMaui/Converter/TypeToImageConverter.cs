using System.Globalization;

namespace OutlayManagerPortableMaui.Converter
{
    public class TypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueType = value?.ToString() ?? String.Empty;

            switch (valueType)
            {
                case "SPENDING":
                    return "expenses.png";

                case "INCOMING":
                    return "incoming.png";
                    
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
