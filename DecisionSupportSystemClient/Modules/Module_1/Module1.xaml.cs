using DecisionSupportSystemClient.Modules.Module_1.DataOperations;
using DecisionSupportSystemClient.Modules.Module_1.Visualizations;
using System.Windows;

namespace DecisionSupportSystemClient
{
    /// <summary>
    /// Interaction logic for Module1.xaml
    /// </summary>
    public partial class Module1 : Window
    {
        public Module1()
        {
            InitializeComponent();
        }

        private void DataOperations_Click(object sender, RoutedEventArgs e)
        {
            var dataOperationsView = new DataOperations();
            dataOperationsView.Show();

            this.Close();
        }

        private void Visualizations_Click(object sender, RoutedEventArgs e)
        {
            var visualizations = new Visualizations();
            visualizations.Show();

            this.Close();
        }
    }
}
