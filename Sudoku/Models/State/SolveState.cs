﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Models.State
{
    public class SolveState : BoardState
    {
        public override void CheckStateChange()
        {
            throw new NotImplementedException();
        }

        public override string GetStateName()
        {
            return "SolveState";
        }

        public override void Handle()
        {
            throw new NotImplementedException();
        }
    }
}
