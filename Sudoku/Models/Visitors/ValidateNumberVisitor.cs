using Sudoku.Controllers;
using Sudoku.Models.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Models.Visitors
{
    public class ValidateNumberVisitor : IVisitor
    {
        public void Visit(ISectionComponent element)
        {
            foreach (CellSection child in element.children)
            {
                if (child.Value == 0)
                    continue;

                child.IsValid = (IsValidValue(child) && IsValueUnique(child));
            }
        }

        private bool IsValidValue(CellSection cell)
        {
            return SudokuGameController.Instance.sudokuBoard.possibleNumbersList.Contains(cell.Value);
        }

        private bool IsValueUnique(CellSection cell)
        {
            return cell.IsValidValue(cell);
        }
    }
}
