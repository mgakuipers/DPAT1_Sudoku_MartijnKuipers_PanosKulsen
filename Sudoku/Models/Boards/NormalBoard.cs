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

        private string orignalContent;

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
            cells = new List<CellSection>();
            regions = new List<RegionSection>();
            rows = new List<RowSection>();
            cols = new List<ColumnSection>();

            int blockSize = (int)Math.Sqrt(size);

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

            for (var rowIndex = 0; rowIndex < size; rowIndex++)
            {
                for (var colIndex = 0; colIndex < size; colIndex++)
                {
                    CellSection cell = new CellSection();
                    cell.Row = rowIndex;
                    cell.Column = colIndex;

                    cell.possibleNumbers = possibleNumbersList;

                    cells.Add(cell);

                    // Create corresponding sections for blocks, rows, and columns
                    int blockSizeVertical = (int)Math.Sqrt(size);
                    int blockSizeHorizontal = size / blockSizeVertical;

                    int blockIndex = (rowIndex / blockSizeVertical) * blockSizeHorizontal + (colIndex / blockSizeHorizontal);
                    int blockRow = blockIndex / blockSizeHorizontal;
                    int blockCol = blockIndex % blockSizeHorizontal;
                    blockIndex = blockRow * blockSizeVertical + blockCol;

                    RegionSection region = regions[blockIndex];
                    region.Add(cell);
                    cell.parentSections.Add(region);

                    RowSection row = rows[rowIndex];
                    row.Add(cell);
                    cell.parentSections.Add(row);

                    ColumnSection col = cols[colIndex];
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

            this.orignalContent = content;
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
                    if (cellValue > 0)
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

        public string GetOriginalContent()
        {
            return this.orignalContent;
        }
    }
}
