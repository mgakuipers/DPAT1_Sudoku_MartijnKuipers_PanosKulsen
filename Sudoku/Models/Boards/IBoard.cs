using Sudoku.Models.Sections;
using Sudoku.Models.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Models.Boards
{
    public interface IBoard
    {
        void CreateBoard();
        BoardState GetBoardState();
        void SetBoardState(BoardState boardState);
        void SetBoardContent(string content);
        void SetCell(int row, int col, int value);
        void SolveBoard();
        void ValidateBoard();
        void FillHintNumbers();
        void ClearHintNumbers();
        IList<int> possibleNumbersList { get; set; }
        CellSection GetCell(int row, int col);
        bool IsSolved();
        int GetSize();
        string GetOriginalContent();
    }
}
