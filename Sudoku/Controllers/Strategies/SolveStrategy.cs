using Sudoku.Models.Boards;
using Sudoku.Models.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Controllers.Strategies
{
    public abstract class SolveStrategy
    {
        public abstract void Solve(BoardSection board);
    }
}
