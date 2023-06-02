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
        public abstract IBoard CreateNormalBoard(int size);
        public abstract IBoard CreateSamuraiBoard(int size);
        public abstract IBoard CreateJigsawBoard(int size);
    }
}
