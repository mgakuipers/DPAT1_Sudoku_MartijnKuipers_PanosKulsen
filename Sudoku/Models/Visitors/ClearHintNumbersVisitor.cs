using Sudoku.Models.Sections;
using System.Collections.Generic;

namespace Sudoku.Models.Visitors
{
    public class ClearHintNumbersVisitor : IVisitor
    {
        public void Visit(ISectionComponent element)
        {
            foreach (CellSection child in element.children)
            {
                ClearPossibleNumbers(child);
            }
        }

        private void ClearPossibleNumbers(CellSection cell)
        {
            cell.PossibleNumbers = new List<int>();
        }
    }
}
