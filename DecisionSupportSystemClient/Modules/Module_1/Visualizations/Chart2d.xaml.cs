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
    /// Interaction logic for Chart2d.xaml
    /// </summary>
    public partial class Chart2d : Window
    {
        public Chart2d()
        {
            InitializeComponent();
        }

        public Chart2d(List<string> columns)
        {
            InitializeComponent();

            foreach (var item in columns)
            {
                this.column1.Items.Add(item);
                this.column2.Items.Add(item);
                this.column3.Items.Add(item);
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
