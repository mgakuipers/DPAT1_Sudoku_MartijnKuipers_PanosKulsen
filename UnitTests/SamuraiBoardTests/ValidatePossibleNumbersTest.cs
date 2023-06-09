using Sudoku.Controllers;
using Sudoku.Models.Boards;
using Sudoku.Models.Sections;

namespace UnitTests.SamuraiBoardTests
{
    public class ValidatePossibleNumbersTest: IDisposable
    {
        private SamuraiBoard samNumBoard;

        public ValidatePossibleNumbersTest()
        {
            // Create the board instance before each test
            samNumBoard = (SamuraiBoard) SudokuGameController.Instance.CreateSamuraiBoard();
        }

        public void Dispose()
        {
            // Clean up resources after each test
            samNumBoard = null;
        }

        [Fact]
        public void TestPossibleNumbers()
        {
            // Arrange
            // Get references to three cells on the board
            int row = 0;
            int col = 0;
            CellSection cell = samNumBoard.GetCell(row, col);

            int row2 = 0;
            int col2 = 1;
            CellSection cell2 = samNumBoard.GetCell(row2, col2);

            int row3 = 0;
            int col3 = 2;
            CellSection cell3 = samNumBoard.GetCell(row3, col3);

            // Set valid values for the first two cells
            int validValue = 5;
            cell.Value = validValue;

            int validValue2 = 3;
            cell2.Value = validValue2;

            // Act
            // Get the possible numbers for the third cell
            IList<int> validPossibleNumbers = new List<int> { 1, 2, 4, 6, 7, 8, 9 };
            IList<int> cell3PossibleNumbers = cell3.GetPossibleNumbers();

            // Check if the two lists are equal
            bool isValidPossibleNumbers = validPossibleNumbers.SequenceEqual(cell3PossibleNumbers);

            // Assert
            Assert.True(isValidPossibleNumbers, "The possible numbers are not valid. They should be: '1, 2, 4, 6, 7, 8, 9'");
        }
    }
}