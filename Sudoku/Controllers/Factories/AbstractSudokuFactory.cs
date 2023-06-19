using Sudoku.Models.Boards;

namespace Sudoku.Controllers.Factories
{
    public abstract class AbstractSudokuFactory
    {
        public abstract IBoard CreateNormalBoard(int size);
        public abstract IBoard CreateSamuraiBoard(int size);
        public abstract IBoard CreateJigsawBoard(int size);
    }
}
