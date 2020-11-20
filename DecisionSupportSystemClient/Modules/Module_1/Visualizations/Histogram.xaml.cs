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
using System.Windows.Shapes;

namespace DecisionSupportSystemClient.Modules.Module_1.Visualizations
{
    /// <summary>
    /// Interaction logic for Histogram.xaml
    /// </summary>
    public partial class Histogram : Window
    {
        public Histogram(List<string> columns)
        {
            InitializeComponent();
            this.numberOfRanges.IsReadOnly = true;

            foreach (var item in columns)
            {
                this.columnsName.Items.Add(item);
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void continousVariable_Checked(object sender, RoutedEventArgs e)
        {
            this.numberOfRanges.IsReadOnly = false;
        }

        private void discreteVariable_Checked(object sender, RoutedEventArgs e)
        {
            this.numberOfRanges.IsReadOnly = true;
        }
    }
}
