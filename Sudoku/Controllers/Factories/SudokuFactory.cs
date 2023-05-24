using Sudoku.Models.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Controllers.Factories
{
    public class SudokuFactory : AbstractSudokuFactory
    {
        private NormalSudokuBoardFactory boardFactory;

        public SudokuFactory()
        {
            boardFactory = new NormalSudokuBoardFactory();
            boardFactory.AddBoardTypes(nameof(NormalBoard), typeof(NormalBoard));
        }

        public override IBoard CreateFourByFourBoard()
        {
            return boardFactory.CreateBoard(nameof(NormalBoard), 4);
        }
        public override IBoard CreateSixBySixBoard()
        {
            return boardFactory.CreateBoard(nameof(NormalBoard), 6);
        }
        public override IBoard CreateNineByNineBoard()
        {
            return boardFactory.CreateBoard(nameof(NormalBoard), 9);
        }
    }
}
