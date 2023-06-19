using Newtonsoft.Json.Linq;
using Sudoku.Controllers;
using Sudoku.Models.Boards;
using Sudoku.Models.Sections;

namespace UnitTests.NormalBoardTests
{
    public class BoardTest: IDisposable
    {
        private NormalBoard norBoard;

        public BoardTest()
        {
            // Create the board instance before each test
            norBoard = (NormalBoard) SudokuGameController.Instance.CreateNormalBoard();
        }

        public void Dispose()
        {
            // Clean up resources after each test
            norBoard = null;
        }

        [Fact]
        public void TestEmptyBoard()
        {
            // Arrange

            // Act
            // Check if the cells are empty
            int value = 0;
            int expectedValue = 0;

            bool isValidEmptyBoard = true;
            foreach (CellSection cell in norBoard.cells)
            {
                if (cell.Value != 0)
                {
                    value = cell.Value;
                    isValidEmptyBoard = false;
                    break;
                }
            }

            // Assert
            Assert.True(isValidEmptyBoard, $"The board should be filled with empty values. Value {value} should have been {expectedValue}");
        }

        [Fact]
        public void TestBoardIsFilled()
        {
            // Arrange
            string inputString = "700509001000000000150070063003904100000050000002106400390040076000000000600201004";
            norBoard.SetBoardContent(inputString);

            // Act
            // Check if the cells have the correct corresponding values
            int value = 0;
            int expectedValue = 0;

            bool isValidFilledBoard = true;
            for (int i = 0; i < norBoard.cells.Count; i++)
            {
                CellSection cell = norBoard.cells[i];
                char expectedChar = inputString[i];

                if (cell.Value != int.Parse(expectedChar.ToString()))
                {
                    isValidFilledBoard = false;
                    value = cell.Value;
                    expectedValue = int.Parse(expectedChar.ToString());
                    break;
                }
            }

            // Assert
            Assert.True(isValidFilledBoard, $"The board should be filled with valid values. Value {value} should have been {expectedValue}");
        }
    }
}