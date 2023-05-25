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
            sudokuBoard.SetBoardContent("0000000000000000");
            return sudokuBoard;
        }

        public IBoard CreateSixBySixBoard()
        {
            sudokuBoard = _factory.CreateSixBySixBoard();
            sudokuBoard.SetBoardContent("000000000000000000000000000000000000");
            return sudokuBoard;
        }

        public IBoard CreateNineByNineBoard()
        {
            sudokuBoard = _factory.CreateNineByNineBoard();
            sudokuBoard.SetBoardContent("000000000000000000000000000000000000000000000000000000000000000000000000000000000");
            return sudokuBoard;
        }
    }
}
