using Sudoku.Models.Visitors;
using System.Collections.Generic;

namespace Sudoku.Models.Sections
{
    public class RegionSection : ISectionComponent
    {
        private IList<ISectionComponent> _parentSections = new List<ISectionComponent>();
        public IList<ISectionComponent> parentSections => _parentSections;


        private IList<CellSection> cells = new List<CellSection>();
        public IList<CellSection> children => cells;

        public void Add(CellSection child)
        {
            cells.Add(child);
        }

        public void Remove(CellSection child)
        {
            cells.Remove(child);
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public CellSection GetCell(int index)
        {
            return children[index];
        }

        public bool IsUnique()
        {
            List<int> set = new List<int>();
            for (int row = 0; row < children.Count; row++)
            {
                CellSection cell = (CellSection)GetCell(row);
                int value = cell.Value;
                if (value != 0 && set.Contains(value))
                {
                    return false;
                }
                else
                {
                    set.Add(value);
                }
            }
            return true;
        }
    }
}
