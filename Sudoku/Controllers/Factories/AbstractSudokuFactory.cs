using Sudoku.Models.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Controllers.Factories
{
    public abstract class AbstractSudokuFactory
    {
        public abstract IBoard CreateFourByFourBoard();
        public abstract IBoard CreateSixBySixBoard();
        public abstract IBoard CreateNineByNineBoard();
    }
}
