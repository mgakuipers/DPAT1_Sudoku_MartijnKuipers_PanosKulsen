using Sudoku.Controllers;
using Sudoku.Models.Boards;
using Sudoku.Models.Sections;

namespace UnitTests.NormalBoardTests
{
    public class ValidateValueTest : IDisposable
    {
        private IBoard iBoard;

        public ValidateValueTest()
        {
            // Create the board instance before each test
            iBoard = SudokuGameController.Instance.CreateNormalBoard();
        }

        public void Dispose()
        {
            // Clean up resources after each test
            iBoard = null;
        }

        [Fact]
        public void TestCellIsValid()
        {
            // Arrange
            // Get a reference to a cell on the board
            int row = 0;
            int col = 0;
            CellSection cell = iBoard.GetCell(row, col);

            // Set a valid value for the cell
            int validValue = 5;
            cell.Value = validValue;

            // Act
            bool isValid = cell.IsValidValue(cell);

            // Assert
            Assert.True(isValid, $"Value {validValue} should be valid in the board.");
        }

        [Fact]
        public void TestCellIsValidInRow()
        {
            // Arrange
            // Get a reference to a cell on the board
            int row = 0;
            int col = 0;
            CellSection cell = iBoard.GetCell(row, col);

            int row2 = 0;
            int col2 = 1;
            CellSection cell2 = iBoard.GetCell(row2, col2);

            // Set a valid value for the cell
            int validValue = 5;
            cell.Value = validValue;

            int validValue2 = 3;
            cell2.Value = validValue2;

            // Act
            bool isValid = cell.IsValidValue(cell);
            bool isValid2 = cell2.IsValidValue(cell2);

            // Assert
            Assert.True(isValid, $"Value {validValue} should be valid in the board.");
            Assert.True(isValid2, $"Value {validValue2} should be valid in the board.");
        }

        [Fact]
        public void TestCellIsInvalidInBlock()
        {
            // Arrange
            // Get a reference to a cell on the board
            int row = 0;
            int col = 0;
            CellSection cell = iBoard.GetCell(row, col);

            int row2 = 0;
            int col2 = 1;
            CellSection cell2 = iBoard.GetCell(row2, col2);

            // Set a invalid value for the cell
            int newValue = 5;
            cell.Value = newValue;

            cell2.Value = newValue;

            // Act
            bool isInvalid = cell2.IsValidValue(cell2);

            // Assert
            Assert.False(isInvalid, $"Value {newValue} should be invalid in the board.");
        }

        [Fact]
        public void TestCellIsInvalidInColumn()
        {
            // Arrange
            // Get a reference to a cell on the board
            int row = 0;
            int col = 0;
            CellSection cell = iBoard.GetCell(row, col);

            int row2 = 0;
            int col2 = 8;
            CellSection cell2 = iBoard.GetCell(row2, col2);

            // Set a invalid value for the cell
            int newValue = 5;
            cell.Value = newValue;

            cell2.Value = newValue;

            // Act
            bool isInvalid = cell2.IsValidValue(cell2);

            // Assert
            Assert.False(isInvalid, $"Value {newValue} should be invalid in the board.");
        }

        [Fact]
        public void TestCellIsInvalidInRow()
        {
            // Arrange
            // Get a reference to a cell on the board
            int row = 0;
            int col = 0;
            CellSection cell = iBoard.GetCell(row, col);

            int row2 = 8;
            int col2 = 0;
            CellSection cell2 = iBoard.GetCell(row2, col2);

            // Set a invalid value for the cell
            int newValue = 5;
            cell.Value = newValue;

            cell2.Value = newValue;

            // Act
            bool isInvalid = cell2.IsValidValue(cell2);

            // Assert
            Assert.False(isInvalid, $"Value {newValue} should be invalid in the board.");
        }
    }
}