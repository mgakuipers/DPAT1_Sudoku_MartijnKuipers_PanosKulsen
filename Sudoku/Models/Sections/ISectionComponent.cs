using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku.Models.Visitors;


namespace Sudoku.Models.Sections
{
    public interface ISectionComponent
    {
        IList<ISectionComponent> parentSections { get; }
        IList<CellSection> children { get; }
        void Accept(IVisitor visitor);
        bool IsUnique();
    }
}
