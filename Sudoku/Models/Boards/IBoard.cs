using Sudoku.Models.Sections;
using Sudoku.Models.State;
using System.Collections.Generic;

namespace Sudoku.Models.Boards
{
    public interface IBoard
    {
        void CreateBoard();
        BoardState GetBoardState();
        void SetBoardState(BoardState boardState);
        bool SetBoardContent(string content);
        void SetCell(int row, int col, int value);
        void SolveBoard();
        void ValidateBoard();
        bool IsValidBoard();
        void FillHintNumbers();
        void ClearHintNumbers();
        IList<int> possibleNumbersList { get; set; }
        CellSection GetCell(int row, int col);
        bool IsSolved();
        int GetSize();
        string GetOriginalContent();
    }
}
