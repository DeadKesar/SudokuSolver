using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public class Line
    {
        public List<Cell> Cells { get; }

        public Line(List<Cell> cells)
        {
            Cells = cells;
        }

        public void UpdatePotentialValues()
        {
            // Удаляем уже существующие значения из потенциальных значений
            var existingValues = Cells
                .Where(c => c.CurrentValue != 0)
                .Select(c => c.CurrentValue)
                .ToList();

            foreach (var cell in Cells.Where(c => c.CurrentValue == 0))
            {
                var valuesToRemove = cell.PotentialValues.Intersect(existingValues).ToList();
                foreach (var value in valuesToRemove)
                {
                    cell.PotentialValues.Remove(value);
                }
            }

            // Применяем техники
            ApplyNakedSubsets();
            ApplyHiddenSubsets();
        }

        private void ApplyNakedSubsets()
        {
            // Ваш код для голых подмножеств (уже реализовано ранее)
        }

        private void ApplyHiddenSubsets()
        {
            // Проверяем для размеров подмножеств от 2 до 5
            for (int subsetSize = 2; subsetSize <= 5; subsetSize++)
            {
                var allPotentialValues = Cells
                    .Where(c => c.CurrentValue == 0)
                    .SelectMany(c => c.PotentialValues)
                    .Distinct();

                // Генерируем комбинации потенциальных значений
                var combinations = GetCombinations(allPotentialValues.ToList(), subsetSize);

                foreach (var combination in combinations)
                {
                    // Находим клетки, которые содержат любые из этих значений
                    var cellsWithCombination = Cells
                        .Where(c => c.CurrentValue == 0 && c.PotentialValues.Intersect(combination).Any())
                        .ToList();

                    // Если количество таких клеток равно размеру подмножества
                    if (cellsWithCombination.Count == subsetSize)
                    {
                        // Проверяем, что каждое число из комбинации появляется хотя бы в одной из этих клеток
                        var unionOfPotentialValues = cellsWithCombination
                            .SelectMany(c => c.PotentialValues)
                            .Distinct();

                        if (combination.All(v => unionOfPotentialValues.Contains(v)))
                        {
                            // Удаляем эти числа из других потенциальных значений этих клеток
                            foreach (var cell in cellsWithCombination)
                            {
                                var valuesToRemove = cell.PotentialValues.Except(combination).ToList();
                                foreach (var value in valuesToRemove)
                                {
                                    cell.PotentialValues.Remove(value);
                                }
                            }

                            // Удаляем эти числа из потенциальных значений других клеток
                            foreach (var cell in Cells.Except(cellsWithCombination))
                            {
                                if (cell.CurrentValue == 0)
                                {
                                    var valuesToRemove = cell.PotentialValues.Intersect(combination).ToList();
                                    foreach (var value in valuesToRemove)
                                    {
                                        cell.PotentialValues.Remove(value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private List<List<int>> GetCombinations(List<int> list, int length)
        {
            if (length == 1)
                return list.Select(t => new List<int> { t }).ToList();

            return GetCombinations(list, length - 1)
                .SelectMany(t => list.Where(e => e > t.Last()),
                    (t1, t2) => new List<int>(t1) { t2 })
                .ToList();
        }
    }
}
