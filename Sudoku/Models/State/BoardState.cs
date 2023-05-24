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
        protected BoardSection BoardSection { get; set; }

        public abstract void CheckStateChange();
    }
}
