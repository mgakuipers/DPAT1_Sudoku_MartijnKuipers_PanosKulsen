using Sudoku.Controllers.Factories;
using Sudoku.Models.Boards;

namespace Sudoku.Controllers
{
    public class SudokuGameController
    {
        private static SudokuGameController _instance;
        private SudokuFactory _factory;
        public static readonly string EMPTY_BOARD_CONTENT = "EMPTY";

        public IBoard sudokuBoard;

        private SudokuGameController() {
            _factory = new SudokuFactory();
        }

        public static SudokuGameController Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new SudokuGameController();
                return _instance;
            }
        }

        public IBoard CreateNormalBoard(int size = 9)
        {
            sudokuBoard = _factory.CreateNormalBoard(size);
            sudokuBoard.SetBoardContent(EMPTY_BOARD_CONTENT);
            return sudokuBoard;
        }

        public IBoard CreateSamuraiBoard(int size = 9)
        {
            sudokuBoard = _factory.CreateSamuraiBoard(size);
            sudokuBoard.SetBoardContent(EMPTY_BOARD_CONTENT);
            return sudokuBoard;
        }

        public IBoard CreateJigsawBoard(int size = 9)
        {
            sudokuBoard = _factory.CreateJigsawBoard(size);
            sudokuBoard.SetBoardContent(EMPTY_BOARD_CONTENT);
            return sudokuBoard;
        }
    }
}
