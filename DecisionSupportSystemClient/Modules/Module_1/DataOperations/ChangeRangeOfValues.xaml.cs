using System.Collections.Generic;
using System.Windows;

namespace DecisionSupportSystemClient.Modules.Module_1.DataOperations
{
    /// <summary>
    /// Interaction logic for ChangeRangeOfValues.xaml
    /// </summary>
    public partial class ChangeRangeOfValues : Window
    {
        public ChangeRangeOfValues()
        {
            InitializeComponent();
        }

        public ChangeRangeOfValues(List<string> columns)
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
