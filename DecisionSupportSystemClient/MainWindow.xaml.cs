using DecisionSupportSystemClient.Helpers;
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
    }
}
