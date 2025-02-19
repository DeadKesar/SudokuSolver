using System;
using System.Globalization;
using System.Windows.Data;

namespace SudokuSolver.Converters
{
    public class ZeroToBooleanConverter : IValueConverter
    {
        public bool ZeroValue { get; set; } = false;
        public bool NonZeroValue { get; set; } = true;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue && intValue == 0)
            {
                return ZeroValue;
            }
            else
            {
                return NonZeroValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
