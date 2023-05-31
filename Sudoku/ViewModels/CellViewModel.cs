using Sudoku.Models.Sections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Sudoku.ViewModels
{
    public class CellViewModel : ViewModelBase
    {
        private CellSection cellModel;
        private int _value;
        private IList<int> _possibleNumbers;
        private bool _isValid = true;
        private bool _isFixed = false;

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

        public IList<int> PossibleNumbers 
        { 
            get { return _possibleNumbers; }
            set 
            { 
                _possibleNumbers = value;
                OnPropertyChanged(nameof(PossibleNumbers));
            } 
        }

        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    cellModel.Value = value;

                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public bool IsFixed
        {
            get { return _isFixed; }
            set
            {
                if (_isFixed != value)
                {
                    _isFixed = value;
                    cellModel.IsFixed = value;

                    OnPropertyChanged(nameof(IsFixed));
                }
            }
        }

        public CellViewModel(CellSection cellModel)
        {
            this.cellModel = cellModel;
            cellModel.PropertyChanged += CellModel_PropertyChanged;

            this._isFixed = cellModel.IsFixed;
        }

        private void CellModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CellSection.IsValid))
            {
                IsValid = cellModel.IsValid;
            }
            if (e.PropertyName == nameof(CellSection.Value))
            {
                Value = cellModel.Value;
            }
            if (e.PropertyName == nameof(CellSection.PossibleNumbers))
            {
                PossibleNumbers = cellModel.PossibleNumbers;
            }
        }
    }
}
