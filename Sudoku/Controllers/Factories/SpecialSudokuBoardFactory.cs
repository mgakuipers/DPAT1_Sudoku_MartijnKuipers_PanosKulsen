﻿using Sudoku.Controllers.Strategies;
using Sudoku.Models.Boards;
using Sudoku.Models.State;
using System;
using System.Collections.Generic;

namespace Sudoku.Controllers.Factories
{
    public class SpecialSudokuBoardFactory
    {
        private Dictionary<string, Type> _types = new Dictionary<string, Type>();

        internal void AddBoardTypes(string name, Type type)
        {
            _types[name] = type;
        }

        internal IBoard CreateBoard(string name, int size)
        {
            Type t = _types[name];
            IBoard board = (IBoard)Activator.CreateInstance(t, new NormalState(), new BacktrackingSolve(), size);

            return board;
        }
    }
}
