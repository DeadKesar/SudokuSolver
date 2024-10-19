using System.Windows;
using System.Windows.Controls;

namespace SudokuSolver.Controls
{
    public partial class CellControl : UserControl
    {
        public CellControl()
        {
            InitializeComponent();
        }

        public Cell Cell
        {
            get { return (Cell)GetValue(CellProperty); }
            set { SetValue(CellProperty, value); }
        }

        public static readonly DependencyProperty CellProperty =
            DependencyProperty.Register("Cell", typeof(Cell), typeof(CellControl), new PropertyMetadata(null, OnCellChanged));

        private static void OnCellChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CellControl;
            if (control != null)
            {
                control.DataContext = control.Cell;
            }
        }
    }
}
