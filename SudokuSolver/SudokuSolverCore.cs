using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SudokuSolver
{
    public class SudokuSolverCore
    {
        private readonly Cell[,] _grid;
        private readonly List<Line> _rows;
        private readonly List<Line> _columns;
        private readonly List<Block> _blocks;

        public SudokuSolverCore(Cell[,] grid)
        {
            _grid = grid;
            _rows = new List<Line>();
            _columns = new List<Line>();
            _blocks = new List<Block>();

            InitializeLinesAndBlocks();
            SubscribeToCellChanges();
        }

        private void InitializeLinesAndBlocks()
        {
            // Инициализация строк
            for (int i = 0; i < 9; i++)
            {
                var rowCells = new List<Cell>();
                for (int j = 0; j < 9; j++)
                {
                    rowCells.Add(_grid[i, j]);
                }
                _rows.Add(new Line(rowCells));
            }

            // Инициализация столбцов
            for (int j = 0; j < 9; j++)
            {
                var columnCells = new List<Cell>();
                for (int i = 0; i < 9; i++)
                {
                    columnCells.Add(_grid[i, j]);
                }
                _columns.Add(new Line(columnCells));
            }

            // Инициализация блоков 3x3
            for (int blockRow = 0; blockRow < 3; blockRow++)
            {
                for (int blockCol = 0; blockCol < 3; blockCol++)
                {
                    var blockCells = new List<Cell>();
                    for (int i = blockRow * 3; i < blockRow * 3 + 3; i++)
                    {
                        for (int j = blockCol * 3; j < blockCol * 3 + 3; j++)
                        {
                            blockCells.Add(_grid[i, j]);
                        }
                    }
                    _blocks.Add(new Block(blockCells));
                }
            }
        }

        private void SubscribeToCellChanges()
        {
            foreach (var cell in _grid)
            {
                cell.PropertyChanged += Cell_PropertyChanged;
            }
        }

        private void Cell_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Cell.CurrentValue))
            {
                UpdatePotentialValues();
            }
        }

        public void UpdatePotentialValues()
        {
            foreach (var line in _rows)
            {
                line.UpdatePotentialValues();
            }
            foreach (var column in _columns)
            {
                column.UpdatePotentialValues();
            }
            foreach (var block in _blocks)
            {
                block.UpdatePotentialValues();
            }
        }

        public void Solve()
        {
            bool updated;
            do
            {
                updated = false;
                UpdatePotentialValues();

                foreach (var cell in _grid)
                {
                    if (cell.CurrentValue == 0 && cell.PotentialValues.Count == 1)
                    {
                        cell.CurrentValue = cell.PotentialValues.First();
                        cell.PotentialValues.Clear();
                        updated = true;
                    }
                }

                // Проверяем, были ли изменения в потенциальных значениях
                foreach (var cell in _grid)
                {
                    if (cell.CurrentValue == 0 && cell.PotentialValues.Count == 0)
                    {
                        throw new InvalidOperationException("Невозможно решить Судоку: нет доступных потенциальных значений.");
                    }
                }
            } while (updated);
        }

    }
}
