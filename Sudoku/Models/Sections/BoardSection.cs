﻿using Sudoku.Controllers.Strategies;
using Sudoku.Models.State;
using Sudoku.Models.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Sudoku.Models.Sections
{
    public class BoardSection : ISectionComponent
    {
        private IList<ISectionComponent> _parentSections = new List<ISectionComponent>();

        public int size { get; private set; }

        public IList<ISectionComponent> parentSections { get => _parentSections; set => _parentSections = value; }

        public IList<CellSection> children => cells;

        public IList<CellSection> cells = new List<CellSection>();
        public IList<RegionSection> regions = new List<RegionSection>();
        public IList<RowSection> rows = new List<RowSection>();
        public IList<ColumnSection> cols = new List<ColumnSection>();

        private BoardState boardState;
        private SolveStrategy solveStrategy;

        public BoardSection(BoardState boardState, SolveStrategy solveStrategy, int size)
        {
            this.boardState = boardState;
            this.solveStrategy = solveStrategy;
            this.size = size;
        }

        public void SetBoardState(BoardState boardState)
        {
            this.boardState = boardState;
        }
        public void SetSolveStrategy(SolveStrategy solveStrategy)
        {
            this.solveStrategy = solveStrategy;
        }

        public CellSection GetCell(int row, int col)
        {
            if (IsValidPosition(row, col))
            {
                return cells.FirstOrDefault(c => c.Column == col && c.Row == row);
            }
            else
            {
                throw new ArgumentException("Invalid cell position. " + row + " " + col);
            }
        }

        public void SetCell(int row, int col, int value)
        {
            if (IsValidPosition(row, col) && IsValidValue(value))
            {
                GetCell(row, col).Value = value;
            }
            else
            {
                throw new ArgumentException("Invalid cell position or value.");
            }
        }

        private bool IsValidPosition(int row, int col)
        {
            return row >= 0 && row < size && col >= 0 && col < size;
        }

        private bool IsValidValue(int value)
        {
            return value >= 0 && value <= size;
        }

        public void Accept(IVisitor visitor)
        {
            foreach (RegionSection region in regions)
            {
                region.Accept(visitor);
            }
            foreach (RowSection row in rows)
            {
                row.Accept(visitor);
            }
            foreach (ColumnSection col in cols)
            {
                col.Accept(visitor);
            }
        }

        public void SolveBoard()
        {
            solveStrategy.Solve(this);
        }

        public int GetSize()
        {
            return size;
        }

        public bool IsUnique()
        {
            foreach (RegionSection block in regions)
            {
                if (!block.IsUnique())
                {
                    return false;
                }
            }
            foreach (RowSection row in rows)
            {
                if (!row.IsUnique())
                {
                    return false;
                }
            }
            foreach (ColumnSection col in cols)
            {
                if (!col.IsUnique())
                {
                    return false;
                }
            }
            return true;
        }
    }
}