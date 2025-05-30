using System.Globalization;

namespace TodoApp.Converters
{
    public class PriorityToBgColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int priority)
            {
                return priority switch
                {
                    1 => new Color(255, 0, 0, 25), // Light Red
                    2 => new Color(255, 165, 0, 25), // Light Orange
                    3 => new Color(0, 128, 0, 25), // Light Green
                    _ => Colors.Transparent
                };
            }
            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}