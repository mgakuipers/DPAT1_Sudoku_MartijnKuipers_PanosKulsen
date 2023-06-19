using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sudoku.Controllers;
using Sudoku.Models.Visitors;


namespace Sudoku.Models.Sections
{
    public class CellSection : ISectionComponent, INotifyPropertyChanged
    {
        private bool _isValid = true;
        private int _value = 0;
        private CellSection _linkedCell;

        private IList<ISectionComponent> _parentSections = new List<ISectionComponent>();
        public IList<ISectionComponent> parentSections => _parentSections;

        private IList<int> _possibleNumbers = new List<int>();
        public IList<int> PossibleNumbers
        {
            get { return _possibleNumbers; }
            set
            {
                _possibleNumbers = value;
                OnPropertyChanged(nameof(PossibleNumbers));
            }
        }
        public CellSection LinkedCell
        {
            get { return _linkedCell; }
            set { _linkedCell = value; }
        }
        public bool IsFixed { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
                if (LinkedCell != null)
                {
                    if (LinkedCell.Value != value)
                    {
                        LinkedCell.Value = value;
                    }
                }
            }
        }
        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public IList<CellSection> children
        {
            get
            {
                List<CellSection> children = new List<CellSection>
                {
                    this
                };
                return children;
            }
        }

        public CellSection()
        {
            IsFixed = false;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public bool IsUnique()
        {
            return true;
        }

        public bool IsValidValue(CellSection cell)
        {
            foreach (ISectionComponent parentSection in parentSections)
            {
                RegionSection regionSection = parentSection as RegionSection;
                RowSection rowSection = parentSection as RowSection;
                ColumnSection colSection = parentSection as ColumnSection;

                if (regionSection != null)
                {
                    foreach (CellSection c in regionSection.children)
                    {
                        if (c != cell && c.Value == cell.Value)
                        {
                            return false;
                        }
                    }
                }
                else if (rowSection != null)
                {
                    foreach (CellSection c in rowSection.children)
                    {
                        if (c != cell && c.Value == cell.Value)
                        {
                            return false;
                        }
                    }
                }
                else if (colSection != null)
                {
                    foreach (CellSection c in colSection.children)
                    {
                        if (c != cell && c.Value == cell.Value)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public IList<int> GetPossibleNumbers()
        {
            IList<int> possibleNumbers = new List<int>();
            IList<int> possibleNumbersRegion = new List<int>();
            IList<int> possibleNumbersRow = new List<int>();
            IList<int> possibleNumbersCol = new List<int>();

            foreach (ISectionComponent parentSection in parentSections)
            {
                RegionSection regionSection = parentSection as RegionSection;
                RowSection rowSection = parentSection as RowSection;
                ColumnSection colSection = parentSection as ColumnSection;

                List<int> allPossibleNumbers = new List<int>();
                allPossibleNumbers.AddRange(SudokuGameController.Instance.sudokuBoard.possibleNumbersList);

                if (regionSection != null)
                {
                    IList<int> setNumbers = new List<int>();
                    foreach (CellSection c in regionSection.children)
                    {
                        if (c.Value != 0)
                            setNumbers.Add(c.Value);
                    }
                    possibleNumbersRegion = allPossibleNumbers.Except(setNumbers).ToList();
                }
                else if (rowSection != null)
                {
                    IList<int> setNumbers = new List<int>();
                    foreach (CellSection c in rowSection.children)
                    {
                        if (c.Value != 0)
                            setNumbers.Add(c.Value);
                    }
                    possibleNumbersRow = allPossibleNumbers.Except(setNumbers).ToList();
                }
                else if (colSection != null)
                {
                    IList<int> setNumbers = new List<int>();
                    foreach (CellSection c in colSection.children)
                    {
                        if (c.Value != 0)
                            setNumbers.Add(c.Value);
                    }
                    possibleNumbersCol = allPossibleNumbers.Except(setNumbers).ToList();
                }

            }
            possibleNumbers = possibleNumbersRegion.Intersect(possibleNumbersRow).ToList();
            possibleNumbers = possibleNumbers.Intersect(possibleNumbersCol).ToList();
            return possibleNumbers;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
