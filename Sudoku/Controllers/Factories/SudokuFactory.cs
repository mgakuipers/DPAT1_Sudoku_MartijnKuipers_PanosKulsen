using Sudoku.Models.Boards;

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

        public override IBoard CreateNormalBoard(int size)
        {
            return normalBoardFactory.CreateBoard(nameof(NormalBoard), size);
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
