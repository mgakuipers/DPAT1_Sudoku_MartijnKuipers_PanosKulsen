using Sudoku.Controllers.Factories;
using Sudoku.Models.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IBoard CreateFourByFourBoard()
        {
            sudokuBoard = _factory.CreateFourByFourBoard();
            sudokuBoard.SetBoardContent(EMPTY_BOARD_CONTENT);
            return sudokuBoard;
        }

        public IBoard CreateSixBySixBoard()
        {
            sudokuBoard = _factory.CreateSixBySixBoard();
            sudokuBoard.SetBoardContent(EMPTY_BOARD_CONTENT);
            return sudokuBoard;
        }

        public IBoard CreateNineByNineBoard()
        {
            sudokuBoard = _factory.CreateNineByNineBoard();
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
