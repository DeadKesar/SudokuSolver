using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SudokuSolver.Converters
{
    public class ZeroToVisibilityConverter : IValueConverter
    {
        public Visibility ZeroVisibility { get; set; } = Visibility.Visible;
        public Visibility NonZeroVisibility { get; set; } = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue && intValue == 0)
            {
                return ZeroVisibility;
            }
            else
            {
                return NonZeroVisibility;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
