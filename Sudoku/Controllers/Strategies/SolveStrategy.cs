using Sudoku.Models.Sections;

namespace Sudoku.Controllers.Strategies
{
    public abstract class SolveStrategy
    {
        public abstract void Solve(BoardSection board);
    }
}
