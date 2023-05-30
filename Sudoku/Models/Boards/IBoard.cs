using Sudoku.Models.Sections;
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
        void SetBoardContent(string content);
        void SetCell(int row, int col, int value);
        void SolveBoard();
        void ValidateBoard();
        void FillHintNumbers();
        IList<int> possibleNumbersList { get; set; }
        CellSection GetCell(int row, int col);
        bool IsSolved();
        int GetSize();
        string GetOriginalContent();
    }
}
