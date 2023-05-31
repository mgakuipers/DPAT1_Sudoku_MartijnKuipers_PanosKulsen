using Sudoku.Controllers.Strategies;
using Sudoku.Models.Sections;
using Sudoku.Models.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Models.Boards
{
    public class JigsawBoard : BoardSection, IBoard
    {
        public JigsawBoard(BoardState boardState, SolveStrategy solveStrategy, int size) : base(boardState, solveStrategy, size)
        {
        }

        public IList<int> possibleNumbersList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void ClearHintNumbers()
        {
            throw new NotImplementedException();
        }

        public void CreateBoard()
        {
            throw new NotImplementedException();
        }

        public void FillHintNumbers()
        {
            throw new NotImplementedException();
        }

        public string GetOriginalContent()
        {
            throw new NotImplementedException();
        }

        public bool IsSolved()
        {
            throw new NotImplementedException();
        }

        public void SetBoardContent(string content)
        {
            throw new NotImplementedException();
        }

        public void ValidateBoard()
        {
            throw new NotImplementedException();
        }
    }
}
