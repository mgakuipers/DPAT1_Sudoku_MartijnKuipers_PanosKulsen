﻿using Sudoku.Controllers;
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
        public void Visit(ISectionComponent element)
        {
            foreach(CellSection child in element.children)
            {
                if(child.Value != 0)
                {
                    child.PossibleNumbers = new List<int>();
                    continue;
                }
                SetPossibleNumbers(child);
            }
        }

        private void SetPossibleNumbers(CellSection cell)
        {
            cell.PossibleNumbers = cell.GetPossibleNumbers();
        }
    }
}
