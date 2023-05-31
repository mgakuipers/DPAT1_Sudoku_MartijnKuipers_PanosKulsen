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
        private NormalSudokuBoardFactory normalBoardFactory;
        private SpecialSudokuBoardFactory specialSudokuBoardFactory;

        public SudokuFactory()
        {
            normalBoardFactory = new NormalSudokuBoardFactory();
            normalBoardFactory.AddBoardTypes(nameof(NormalBoard), typeof(NormalBoard));

            specialSudokuBoardFactory = new SpecialSudokuBoardFactory();
            specialSudokuBoardFactory.AddBoardTypes(nameof(SamuraiBoard), typeof(SamuraiBoard));
            specialSudokuBoardFactory.AddBoardTypes(nameof(JigsawBoard), typeof(JigsawBoard));
        }

        //TODO: Misschien aanpassen naar 1 functie die gwn een size als param aanneemt. Dit is duplicate code die nauwelijks verschilt;
        public override IBoard CreateFourByFourBoard()
        {
            return normalBoardFactory.CreateBoard(nameof(NormalBoard), 4);
        }
        public override IBoard CreateSixBySixBoard()
        {
            return normalBoardFactory.CreateBoard(nameof(NormalBoard), 6);
        }
        public override IBoard CreateNineByNineBoard()
        {
            return normalBoardFactory.CreateBoard(nameof(NormalBoard), 9);
        }
        public override IBoard CreateSamuraiBoard(int size)
        {
            return specialSudokuBoardFactory.CreateBoard(nameof(SamuraiBoard), size);
        }
        public override IBoard CreateJigsawBoard(int size)
        {
            return specialSudokuBoardFactory.CreateBoard(nameof(JigsawBoard), size);
        }
    }
}
