using Sudoku.Controllers.Factories;
using Sudoku.Models.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

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
            IBoard board = _factory.CreateNormalBoard(size);
            board.SetBoardContent(EMPTY_BOARD_CONTENT);
            sudokuBoard = board;

            return board;
        }

        public IBoard CreateSamuraiBoard(int size = 9)
        {
            IBoard board = _factory.CreateSamuraiBoard(size);
            board.SetBoardContent(EMPTY_BOARD_CONTENT);
            sudokuBoard = board;

            return board;
        }

        public IBoard CreateJigsawBoard(int size = 9)
        {
            IBoard board = _factory.CreateJigsawBoard(size);
            board.SetBoardContent(EMPTY_BOARD_CONTENT);
            sudokuBoard = board;

            return board;
        }
    }
}
