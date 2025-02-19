using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SudokuSolver
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Cell> Cells { get; set; }
        private SudokuSolverCore _solver;

        private bool _isNotesMode;
        public bool IsNotesMode
        {
            get => _isNotesMode;
            set
            {
                if (_isNotesMode != value)
                {
                    _isNotesMode = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Инициализация коллекции клеток
            Cells = new ObservableCollection<Cell>();

            for (int i = 0; i < 81; i++)
            {
                var cell = new Cell
                {
                    CellIndex = i
                };
                Cells.Add(cell);
            }

            // Создание 2D массива клеток
            var grid = new Cell[9, 9];
            for (int i = 0; i < 81; i++)
            {
                int row = i / 9;
                int col = i % 9;
                grid[row, col] = Cells[i];
            }

            // Инициализация решателя Судоку
            _solver = new SudokuSolverCore(grid);
        }

        private void SolveButton_Click(object sender, RoutedEventArgs e)
        {
            _solver.Solve();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var cell in Cells)
            {
                cell.CurrentValue = 0;
                cell.PotentialValues = new ObservableCollection<int>(Enumerable.Range(1, 9));
            }
        }

        private void RecalculateButton_Click(object sender, RoutedEventArgs e)
        {
            // Пересчитываем PotentialValues на основе текущей ситуации
            foreach (var cell in Cells)
            {
                if (cell.CurrentValue == 0)
                {
                    cell.PotentialValues = new ObservableCollection<int>(Enumerable.Range(1, 9));
                }
                else
                {
                    cell.PotentialValues.Clear();
                }
            }

            _solver.UpdatePotentialValues();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
