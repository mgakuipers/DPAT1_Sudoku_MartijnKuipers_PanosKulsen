using Sudoku.Models.Boards;
using Sudoku.Models.Sections;
using Sudoku.Models.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Controllers.Strategies
{
    public class BacktrackingSolve : SolveStrategy
    {
        public override void Solve(BoardSection board)
        {
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
            int size = board.GetSize();

            // Check row and column constraints
            for (int i = 0; i < size; i++)
            {
                if (board.GetCell(row, i).Value == num || board.GetCell(i, col).Value == num)
                    return false;
            }

            // Check grid constraints
            int gridSize = (int)Math.Sqrt(size);
            int startRow = row - (row % gridSize);
            int startCol = col - (col % gridSize);
            for (int i = startRow; i < startRow + gridSize; i++)
            {
                for (int j = startCol; j < startCol + gridSize; j++)
                {
                    if (board.GetCell(i, j).Value == num)
                        return false;
                }
            }

            return true;
        }
    }
}
