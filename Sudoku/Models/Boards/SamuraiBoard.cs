using Sudoku.Controllers;
using Sudoku.Controllers.Strategies;
using Sudoku.Models.Sections;
using Sudoku.Models.State;
using Sudoku.Models.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Models.Boards
{
    public class SamuraiBoard : BoardSection, IBoard
    {
        private readonly int AMOUNT_OF_BOARDS = 5;
        public IList<BoardSection> boards;
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

        public SamuraiBoard(BoardState boardState, SolveStrategy solveStrategy, int size) : base(boardState, solveStrategy, size)
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
            boards = new List<BoardSection>();
            regions = new List<RegionSection>();
            rows = new List<RowSection>();
            cols = new List<ColumnSection>();

            // Create corresponding sections for blocks, rows, and columns
            int regionSizeHorizontal = GetHorizontalRegionSize();
            int regionSizeVertical = GetVerticalRegionSize();
            for(var boardIndex = 0; boardIndex < AMOUNT_OF_BOARDS; boardIndex++)
            {
                boards.Add(new BoardSection(boardState, solveStrategy, size));
                BoardSection currentBoard = boards[boardIndex];
                for(var i = 0; i < size; i++)
                {
                    currentBoard.regions.Add(new RegionSection());
                    currentBoard.rows.Add(new RowSection());
                    currentBoard.cols.Add(new ColumnSection());
                }
                for (var rowIndex = 0; rowIndex < size; rowIndex++)
                {
                    for (var colIndex = 0; colIndex < size; colIndex++)
                    {
                        CellSection cell = new CellSection();
                        cell.Row = rowIndex;
                        cell.Column = colIndex;
                        cell.PossibleNumbers = possibleNumbersList;

                        cells.Add(cell);
                        currentBoard.cells.Add(cell);

                        int regionIndex = CalculateRegionIndex(regionSizeHorizontal, regionSizeVertical, rowIndex, colIndex);

                        RegionSection region = currentBoard.regions[regionIndex];
                        region.Add(cell);
                        cell.parentSections.Add(region);

                        RowSection row = currentBoard.rows[rowIndex];
                        row.Add(cell);
                        cell.parentSections.Add(row);

                        ColumnSection col = currentBoard.cols[colIndex];
                        col.Add(cell);
                        cell.parentSections.Add(col);
                    }
                }
                ((List<RegionSection>)regions).AddRange(currentBoard.regions);
                ((List<RowSection>)rows).AddRange(currentBoard.rows);
                ((List<ColumnSection>)cols).AddRange(currentBoard.cols);
            }
            
            //TODO: Maybe change to not use 5 different boards and just create long lists of cols, rows, regions.
            // With empty cells in between.
            // since setting a reference to a cell from a different board is impossible as they would have the wrong col and row index
            // Another solution may be to keep a seperate map of cells that should be the same and if one gets updated, also update the other.
            // Last solution is probably the easiest and cleanest.

        }

        //WIP might not work
        /*public void CreateBoard()
        {
            IList<BoardSection> boardSections = new List<BoardSection>();

            for(var i = 0; i < AMOUNT_OF_BOARDS; i++)
            {
                //boardSections.Add(CreateBoardSection(new BoardSection(boardState, solveStrategy, size)));
            }
        }*/

        /*public BoardSection CreateBoardSection(BoardSection boardSection)
        {
            boardSection.cells = new List<CellSection>();
            boardSection.regions = new List<RegionSection>();
            boardSection.rows = new List<RowSection>();
            boardSection.cols = new List<ColumnSection>();

            int blockSize = (int)Math.Sqrt(size);

            // Initialize blocks list
            for (var i = 0; i < size; i++)
            {
                boardSection.regions.Add(new RegionSection());
            }
            // Initialize rows list
            for (var i = 0; i < size; i++)
            {
                boardSection.rows.Add(new RowSection());
            }
            // Initialize columns list
            for (var i = 0; i < size; i++)
            {
                boardSection.cols.Add(new ColumnSection());
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
                    cell.PossibleNumbers = possibleNumbersList;

                    boardSection.cells.Add(cell);

                    int regionIndex = CalculateRegionIndex(regionSizeHorizontal, regionSizeVertical, rowIndex, colIndex);

                    RegionSection region = boardSection.regions[regionIndex];
                    region.Add(cell);
                    cell.parentSections.Add(region);

                    RowSection row = boardSection.rows[rowIndex];
                    row.Add(cell);
                    cell.parentSections.Add(row);

                    ColumnSection col = boardSection.cols[colIndex];
                    col.Add(cell);
                    cell.parentSections.Add(col);
                }
            }
            return boardSection;
        }*/

        public void SetBoardContent(string content)
        {
            if (content.Equals(SudokuGameController.EMPTY_BOARD_CONTENT))
            {
                content = new string('0', GetSize() * GetSize());
            }
            else if (content.Length != GetSize() * GetSize())
            {
                throw new ArgumentException("Invalid content length for setting board state.");
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
