using Newtonsoft.Json.Linq;
using Sudoku.Controllers;
using Sudoku.Models.Boards;
using Sudoku.Models.Sections;

namespace UnitTests.SamuraiBoardTests
{
    public class BoardTest: IDisposable
    {
        private SamuraiBoard samBoard;

        public BoardTest()
        {
            // Create the board instance before each test
            samBoard = (SamuraiBoard) SudokuGameController.Instance.CreateSamuraiBoard();
        }

        public void Dispose()
        {
            // Clean up resources after each test
            samBoard = null;
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
            foreach (CellSection cell in samBoard.cells)
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
            string inputString = "800000700003050206700300095000091840000007002000062000000000000609080000002903000149000000000091000000060000007120008000000340405008067000000000000007020000050003000000000000008000000004000010600005030070080800005010000900000000800000000000000900060000030400000000000000390800407065000000200037600000080000000190000000000914000402800000080902000000000000610000400800000098750000670008001901060700002000009";
            samBoard.SetBoardContent(inputString);

            // Act
            // Check if the cells have the correct corresponding values
            int value = 0;
            int expectedValue = 0;

            bool isValidFilledBoard = true;
            for (int i = 0; i < samBoard.cells.Count; i++)
            {
                CellSection cell = samBoard.cells[i];
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