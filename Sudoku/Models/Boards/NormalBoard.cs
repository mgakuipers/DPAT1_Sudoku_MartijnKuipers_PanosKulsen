using Sudoku.Controllers.Strategies;
using Sudoku.Models.Sections;
using Sudoku.Models.State;
using Sudoku.Models.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Sudoku.Models.Boards
{
    public class NormalBoard : BoardSection, IBoard
    {
        private IList<int> _possibleNumbersList = new List<int>();
        public IList<int> possibleNumbersList
        {
            get { return _possibleNumbersList; }
            set
            {
                _possibleNumbersList = value;
            }
        }

        public NormalBoard(BoardState boardState, SolveStrategy solveStrategy, int size) : base(boardState, solveStrategy, size)
        {
            // Initialize possible numbers list
            for (var i = 1; i <= size; i++)
            {
                possibleNumbersList.Add(i);
            }
        }

        public void CreateBoard()
        {
            int blockSize = (int)Math.Sqrt(size);
            int blockRowCount = size / blockSize;
            int blockColCount = blockSize;

            // Initialize blocks list
            for (var i = 0; i < size; i++)
            {
                regions.Add(new RegionSection());
            }
            // Initialize rows list
            for (var i = 0; i < size; i++)
            {
                rows.Add(new RowSection());
            }
            // Initialize columns list
            for (var i = 0; i < size; i++)
            {
                cols.Add(new ColumnSection());
            }

            for (var i = 0; i < size; i++)
            {
                for(var  j = 0; j < size; j++)
                {
                    CellSection cell = new CellSection();
                    cell.Row = i;
                    cell.Column = j;

                    cell.possibleNumbers = possibleNumbersList;

                    cells.Add(cell);

                    // Create corresponding sections for blocks, rows, and columns
                    int blockIndex = (i / blockRowCount) * blockColCount + (j / blockSize);

                    RegionSection region = regions[blockIndex];
                    region.Add(cell);
                    cell.parentSections.Add(region);

                    RowSection row = rows[i];
                    row.Add(cell);
                    cell.parentSections.Add(row);

                    ColumnSection col = cols[j];
                    col.Add(cell);
                    cell.parentSections.Add(col);
                }
            }
        }

        public void SetBoardContent(string content)
        {
            if (content.Length != GetSize() * GetSize())
            {
                throw new ArgumentException("Invalid content length for setting board state.");
            }

            CreateBoard();

            for (int row = 0; row < GetSize(); row++)
            {
                for (int col = 0; col < GetSize(); col++)
                {
                    char cellChar = content[row * GetSize() + col];
                    int cellValue;

                    if (char.IsDigit(cellChar))
                    {
                        cellValue = int.Parse(cellChar.ToString());
                    }
                    else
                    {
                        cellValue = 0; // Treat non-digit characters as empty cells
                    }

                    SetCell(row, col, cellValue);
                    if(cellValue > 0)
                    {
                        GetCell(row, col).IsFixed = true;
                    }
                }
            }
        }

        public bool IsSolved()
        {
            // Check if all cells have non-zero values
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if (GetCell(row, col).Value == 0)
                    {
                        return false;
                    }
                }
            }

            // Check if all rows, columns, and regions contain unique values
            for (int i = 0; i < size; i++)
            {
                if (!IsUnique())
                {
                    return false;
                }
            }

            return true;
        }

        public void ValidateBoard()
        {
            Accept(new ValidateNumberVisitor());
        }
    }
}
