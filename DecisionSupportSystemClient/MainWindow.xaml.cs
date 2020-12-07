using DecisionSupportSystemClient.Helpers;
using DecisionSupportSystemClient.Modules.Module_2;
using DecisionSupportSystemClient.Modules.Module_3;
using System.Windows;

namespace DecisionSupportSystemClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ClientHelper.InitHttpClient();
        }

        private void Module1_Click(object sender, RoutedEventArgs e)
        {
            var module1Window = new Module1();
            module1Window.Show();

            this.Close();
        }

        private void Module2_Click(object sender, RoutedEventArgs e)
        {
            var module2Window = new Module2();
            module2Window.Show();

            this.Close();
        }

        private void Module3_Click(object sender, RoutedEventArgs e)
        {
            var module3Window = new Module3();
            module3Window.Show();

            this.Close();
        }
    }
}
