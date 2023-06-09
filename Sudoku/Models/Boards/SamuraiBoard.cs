using Sudoku.Controllers;
using Sudoku.Controllers.Strategies;
using Sudoku.Models.Enums;
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
                currentBoard.SamuraiPosition = (SamuraiPositionEnum)(boardIndex+1);
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
            SetLinkedCells();
        }

        private void SetLinkedCells()
        {
            BoardSection centerBoard = boards.Where(board => board.SamuraiPosition.Equals(SamuraiPositionEnum.CENTER)).First();
            foreach(BoardSection board in boards)
            {
                int centerBoardRegionIndex = 0;
                int boardRegionIndex = 0;
                switch(board.SamuraiPosition)
                {
                    case SamuraiPositionEnum.TOP_LEFT:
                        centerBoardRegionIndex = 0;
                        boardRegionIndex = 8;
                        break;

                    case SamuraiPositionEnum.TOP_RIGHT:
                        centerBoardRegionIndex = 2;
                        boardRegionIndex = 6;
                        break;

                    case SamuraiPositionEnum.BOTTOM_LEFT:
                        centerBoardRegionIndex = 6;
                        boardRegionIndex = 2;
                        break;

                    case SamuraiPositionEnum.BOTTOM_RIGHT:
                        centerBoardRegionIndex = 8;
                        boardRegionIndex = 0;
                        break;
                }

                for (var i = 0; i < board.regions[boardRegionIndex].children.Count; i++)
                {
                    CellSection cell = board.regions[boardRegionIndex].children[i];
                    centerBoard.regions[centerBoardRegionIndex].children[i].LinkedCell = cell;
                }
            }
        }

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
