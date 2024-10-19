using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public class Block
    {
        public List<Cell> Cells { get; }

        public Block(List<Cell> cells)
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
            // Ваш код для голых подмножеств
        }

        private void ApplyHiddenSubsets()
        {
            // Логика такая же, как в классе Line
            for (int subsetSize = 2; subsetSize <= 5; subsetSize++)
            {
                var allPotentialValues = Cells
                    .Where(c => c.CurrentValue == 0)
                    .SelectMany(c => c.PotentialValues)
                    .Distinct();

                var combinations = GetCombinations(allPotentialValues.ToList(), subsetSize);

                foreach (var combination in combinations)
                {
                    var cellsWithCombination = Cells
                        .Where(c => c.CurrentValue == 0 && c.PotentialValues.Intersect(combination).Any())
                        .ToList();

                    if (cellsWithCombination.Count == subsetSize)
                    {
                        var unionOfPotentialValues = cellsWithCombination
                            .SelectMany(c => c.PotentialValues)
                            .Distinct();

                        if (combination.All(v => unionOfPotentialValues.Contains(v)))
                        {
                            foreach (var cell in cellsWithCombination)
                            {
                                var valuesToRemove = cell.PotentialValues.Except(combination).ToList();
                                foreach (var value in valuesToRemove)
                                {
                                    cell.PotentialValues.Remove(value);
                                }
                            }

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
