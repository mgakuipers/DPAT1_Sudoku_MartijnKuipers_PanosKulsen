using Sudoku.Controllers;
using Sudoku.Models.Boards;
using Sudoku.Models.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
