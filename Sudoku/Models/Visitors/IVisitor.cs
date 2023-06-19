using Sudoku.Models.Sections;

namespace Sudoku.Models.Visitors
{
    public interface IVisitor
    {
        void Visit(ISectionComponent element);
    }
}
