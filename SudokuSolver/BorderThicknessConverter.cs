using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SudokuSolver.Converters
{
    public class BorderThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (int)value;
            int thickness = 1;

            int left = (index % 9 == 0) ? thickness * 2 : thickness;
            int top = (index / 9 == 0) ? thickness * 2 : thickness;
            int right = ((index + 1) % 9 == 0) ? thickness * 2 : thickness;
            int bottom = ((index / 9 + 1) % 3 == 0) ? thickness * 2 : thickness;

            return new Thickness(left, top, right, bottom);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
