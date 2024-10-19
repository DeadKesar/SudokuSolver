using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SudokuSolver
{
    public class Cell : INotifyPropertyChanged
    {
        private int _currentValue;

        public int CurrentValue
        {
            get => _currentValue;
            set
            {
                if (_currentValue != value)
                {
                    _currentValue = value;
                    OnPropertyChanged();

                    if (_currentValue != 0)
                    {
                        PotentialValues.Clear();
                    }
                    else
                    {
                        PotentialValues = new ObservableCollection<int>(Enumerable.Range(1, 9));
                    }
                }
            }
        }

        private ObservableCollection<int> _potentialValues;

        public ObservableCollection<int> PotentialValues
        {
            get => _potentialValues;
            set
            {
                if (_potentialValues != value)
                {
                    _potentialValues = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CellIndex { get; set; } // Для границ блоков 3x3

        public Cell()
        {
            CurrentValue = 0;
            PotentialValues = new ObservableCollection<int>(Enumerable.Range(1, 9));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
