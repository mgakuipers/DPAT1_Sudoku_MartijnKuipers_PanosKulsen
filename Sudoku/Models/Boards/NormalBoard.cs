using Sudoku.Controllers;
using Sudoku.Controllers.Strategies;
using Sudoku.Models.Sections;
using Sudoku.Models.State;
using Sudoku.Models.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private string originalContent;

        public NormalBoard(BoardState boardState, SolveStrategy solveStrategy, int size) : base(boardState, solveStrategy, size)
        {
            // Initialize possible numbers list
            for (var i = 1; i <= size; i++)
            {
                possibleNumbersList.Add(i);
            }
        }

        public BoardState GetBoardState()
        {
            return this.boardState;
        }

        public void SetBoardState(BoardState boardState)
        {
            this.boardState = boardState;
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

            // Create corresponding sections for blocks, rows, and columns
            int regionSizeHorizontal = GetHorizontalRegionSize();
            int regionSizeVertical = GetVerticalRegionSize();
            for (var rowIndex = 0; rowIndex < size; rowIndex++)
            {
                for (var colIndex = 0; colIndex < size; colIndex++)
                {
                    CellSection cell = new CellSection();
                    cell.Row = rowIndex;
                    cell.Column = colIndex;
                    if(GetBoardState().GetStateName() == (new HelperState()).GetStateName())
                        cell.PossibleNumbers = possibleNumbersList;

                    cells.Add(cell);

                    int regionIndex = CalculateRegionIndex(regionSizeHorizontal, regionSizeVertical, rowIndex, colIndex);

                    RegionSection region = regions[regionIndex];
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

        public bool SetBoardContent(string content)
        {
            if (content.Equals(SudokuGameController.EMPTY_BOARD_CONTENT))
            {
                content = new string('0', GetSize() * GetSize());
            }
            else if (content.Length != GetSize() * GetSize())
            {
                return false;
            }

            this.originalContent = content;
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

            return true;
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
            StateHandle();
        }

        public bool IsValidBoard()
        {
            foreach(CellSection child in this.children)
            {
                if(!child.IsValid)
                {
                    return false;
                }
            }
            return true;
        }

        public void FillHintNumbers()
        {
            Accept(new CheckHintNumbersVisitor());
        }

        public void ClearHintNumbers()
        {
            Accept(new ClearHintNumbersVisitor());
        }

        public string GetOriginalContent()
        {
            return this.originalContent;
        }
    }
}
