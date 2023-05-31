using Sudoku.Models.Boards;
using Sudoku.Models.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Models.State
{
    public class HelperState : BoardState
    {
        public override void CheckStateChange()
        {
            throw new NotImplementedException();
        }

        public override string GetStateName()
        {
            return "HelperState";
        }

        public override void Handle()
        {   
            if(this.BoardSection != null) 
                ((IBoard)this.BoardSection).FillHintNumbers();
        }
    }
}
