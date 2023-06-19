using Sudoku.Controllers;
using Sudoku.Models.Sections;

namespace Sudoku.Models.Visitors
{
    public class ValidateNumberVisitor : IVisitor
    {
        public void Visit(ISectionComponent element)
        {
            foreach (CellSection child in element.children)
            {
                child.IsValid = (IsValidValue(child) && IsValueUnique(child));
            }
        }

        private bool IsValidValue(CellSection cell)
        {
            if(cell.Value == 0) return true;
            return SudokuGameController.Instance.sudokuBoard.possibleNumbersList.Contains(cell.Value);
        }

        private bool IsValueUnique(CellSection cell)
        {
            if (cell.Value == 0) return true;
            return cell.IsValidValue(cell);
        }
    }
}
