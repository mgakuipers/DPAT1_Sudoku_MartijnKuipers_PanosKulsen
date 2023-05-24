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
        IList<int> possibleNumbersList { get; set; }
        void CreateBoard();
        void SetBoardContent(string content);
        int GetSize();
        void SetCell(int row, int col, int value);
        CellSection GetCell(int row, int col);
        bool IsSolved();
        void SolveBoard();
        void ValidateBoard();
    }
}
