using Sudoku.Models.Boards;
using Sudoku.Models.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Models.Visitors
{
    public class CheckHintNumbersVisitor : IVisitor
    {
        private readonly NormalBoard normalBoard;
        public CheckHintNumbersVisitor(NormalBoard normalBoard)
        {
            this.normalBoard = normalBoard;
        }

        public void Visit(ISectionComponent element)
        {
            foreach(CellSection child in element.children)
            {
                if(child.Value != 0)
                    continue;

                SetPossibleNumbers(child, normalBoard.possibleNumbersList);
            }
        }

        private void SetPossibleNumbers(CellSection cell, IList<int> possibleNumbersList)
        {
            cell.PossibleNumbers = cell.GetPossibleNumbers(possibleNumbersList);
        }
    }
}
