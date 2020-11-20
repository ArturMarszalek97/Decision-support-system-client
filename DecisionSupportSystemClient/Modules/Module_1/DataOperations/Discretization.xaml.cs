using System.Collections.Generic;
using System.Windows;

namespace DecisionSupportSystemClient.Modules.Module_1.DataOperations
{
    /// <summary>
    /// Interaction logic for Discretization.xaml
    /// </summary>
    public partial class Discretization : Window
    {
        public Discretization()
        {
            InitializeComponent();
        }

        public Discretization(List<string> columns)
        {
            InitializeComponent();

            foreach (var item in columns)
            {
                this.columnsName.Items.Add(item);
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
