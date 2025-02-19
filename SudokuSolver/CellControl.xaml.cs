using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public bool IsNotesMode
        {
            get { return (bool)GetValue(IsNotesModeProperty); }
            set { SetValue(IsNotesModeProperty, value); }
        }

        public static readonly DependencyProperty IsNotesModeProperty =
            DependencyProperty.Register("IsNotesMode", typeof(bool), typeof(CellControl), new PropertyMetadata(false));

        private static void OnCellChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CellControl;
            if (control != null)
            {
                control.DataContext = control.Cell;
            }
        }

        private void PotentialValue_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Cell == null || Cell.CurrentValue != 0)
                return;

            if (sender is TextBlock textBlock)
            {
                if (int.TryParse(textBlock.Text, out int value))
                {
                    if (Cell.PotentialValues.Contains(value))
                    {
                        Cell.PotentialValues.Remove(value);
                    }
                }
            }
        }
    }
}
