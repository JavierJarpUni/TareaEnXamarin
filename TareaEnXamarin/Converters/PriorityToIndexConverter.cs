using System.Globalization;

namespace TodoApp.Converters
{
    public class PriorityToIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int priority)
            {
                return priority - 1; // Adjust priority (1, 2, 3) to picker index (0, 1, 2)
            }
            return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
            {
                return index + 1; // Adjust picker index (0, 1, 2) back to priority (1, 2, 3)
            }
            return 2; // Default to Medium
        }
    }
}