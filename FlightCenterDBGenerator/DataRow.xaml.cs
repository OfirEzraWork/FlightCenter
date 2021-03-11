using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightCenterDBGenerator
{
    /// <summary>
    /// Interaction logic for DataRow.xaml
    /// </summary>
    public partial class DataRow : UserControl
    {
        public string RowText { get; set; }
        public string Value { get; set; }

        public DataRow()
        {
            InitializeComponent();
        }

    }
}
