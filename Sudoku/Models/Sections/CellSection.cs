using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sudoku.Models.Visitors;


namespace Sudoku.Models.Sections
{
    public class CellSection : ISectionComponent, System.ComponentModel.INotifyPropertyChanged
    {
        private bool _isValid = true;
        private int _value = 0;

        private IList<ISectionComponent> _parentSections = new List<ISectionComponent>();
        public IList<ISectionComponent> parentSections => _parentSections;

        public IList<int> possibleNumbers = new List<int>();

        public bool IsFixed { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int Value {
            get { return _value; } 
            set
            {
                if(_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            } 
        }
        public bool IsValid
        {
            get { return _isValid; }
            set 
            { 
                if(_isValid != value)
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
                    foreach(CellSection c in regionSection.children)
                    {
                        if(c != cell && c.Value == cell.Value)
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
