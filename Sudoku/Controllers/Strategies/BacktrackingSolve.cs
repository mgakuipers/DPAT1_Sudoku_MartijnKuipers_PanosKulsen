using Sudoku.Models.Boards;
using Sudoku.Models.Sections;
using Sudoku.Models.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sudoku.Controllers.Strategies
{
    public class BacktrackingSolve : SolveStrategy
    {
        public override void Solve(BoardSection board)
        {
            // Don't solve the board when the board is invalid
            if (!SudokuGameController.Instance.sudokuBoard.IsValidBoard())
                return;

            SolveBoard(board);
        }

        private bool SolveBoard(BoardSection board)
        {
            // Find the next empty cell
            bool isFound = FindEmptyCell(board, out int row, out int col);

            // If no empty cell is found, the board is solved
            if (!isFound)
                return true;

            // Try different numbers for the empty cell
            for (int num = 1; num <= board.size; num++)
            {
                if (IsValidMove(board, row, col, num))
                {
                    // Assign the number to the cell
                    board.GetCell(row, col).Value = num;

                    // Recursively solve the remaining board
                    if (SolveBoard(board))
                        return true;

                    // If the board cannot be solved with the current number, backtrack
                    board.GetCell(row, col).Value = 0;
                }
            }
            // If no number can be assigned to the cell, backtrack
            return false;
        }

        /**
         * Trying to find an empty cell and changing the row and col index
         * because they are passed by reference
         */
        private bool FindEmptyCell(BoardSection board, out int row, out int col)
        {
            int size = board.GetSize();
            for (row = 0; row < size; row++)
            {
                for (col = 0; col < size; col++)
                {
                    if (board.GetCell(row, col).Value == 0)
                        return true;
                }
            }
            row = -1;
            col = -1;
            return false;
        }

        private bool IsValidMove(BoardSection board, int row, int col, int num)
        {
            // Check row constraints
            RowSection rowSection = board.rows[row];
            foreach (CellSection c in rowSection.children)
            {
                if (c.Value == num)
                {
                    return false;
                }
            }

            // Check col contraints
            ColumnSection colSection = board.cols[col];
            foreach (CellSection c in colSection.children)
            {
                if (c.Value == num)
                {
                    return false;
                }
            }

            // Check region constraints
            int regionSizeVertical = board.GetVerticalRegionSize();
            int regionSizeHorizontal = board.GetHorizontalRegionSize();
            int regionIndex = board.CalculateRegionIndex(regionSizeHorizontal, regionSizeVertical, row, col);

            RegionSection regionSection = board.regions[regionIndex];
            foreach (CellSection c in regionSection.children)
            {
                if (c.Value == num)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
