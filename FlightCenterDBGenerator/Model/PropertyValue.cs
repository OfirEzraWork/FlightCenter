using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlightCenterDBGenerator
{
    public class PropertyValue : IDataErrorInfo, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        private int amount;
        public int Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
            }
        }

        private bool isRandom;
        public bool IsRandom
        {
            get
            {
                return isRandom;
            }
            set
            {
                isRandom = value;
                if (!isRandom)
                {
                    Range = "";
                    OnPropertyChanged("Range");
                }
                else
                {
                    Amount = 0;
                    OnPropertyChanged("Amount");
                }
            }
        }
        private string range;
        public string Range
        {
            get
            {
                return range;
            }
            set
            {
                range = value;
            }
        }
        public string Name { get; set; }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Amount":
                        if (!IsRandom)
                        {
                            if (Amount < 2)
                            {
                                return "Low Number";
                            }
                            else if (Amount > 100)
                            {
                                return "High Number";
                            }
                        }
                        return string.Empty;
                    case "Range":
                        if (IsRandom)
                        {
                            Regex regex = new Regex(@"[0-9]+?-[0-9]+");
                            if (Range == null || Range == "" || regex.Match(Range) == Match.Empty)
                            {
                                return "Invalid Range";
                            }
                        }
                        return string.Empty;
                }
                return string.Empty;
            }
        }

        public PropertyValue(string name)
        {
            Name = name;
        }

    }
}
