using System.Collections.Generic;
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
