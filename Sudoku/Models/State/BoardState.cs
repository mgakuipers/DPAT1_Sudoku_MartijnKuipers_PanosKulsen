

using Sudoku.Controllers;
using Sudoku.Models.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Models.State
{
    public abstract class BoardState
    {
        protected BoardSection BoardSection { get; set; } = (BoardSection)SudokuGameController.Instance.sudokuBoard;

        public abstract void CheckStateChange();

        public abstract void Handle();

        public abstract string GetStateName();
    }
}
