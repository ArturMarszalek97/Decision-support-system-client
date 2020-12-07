using DecisionSupportSystemClient.Helpers;
using DecisionSupportSystemClient.Modules.Module_1.DataOperations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace DecisionSupportSystemClient.Modules.Module_3
{
    /// <summary>
    /// Interaction logic for Module3.xaml
    /// </summary>
    public partial class Module3 : Window
    {
        HttpClient client;
        List<string> columns;

        public Module3()
        {
            InitializeComponent();
            this.client = ClientHelper.GetClient();
            this.columns = new List<string>();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            var loadFile = new LoadFile();
            var pathToFile = String.Empty;
            var separator = String.Empty;

            if (loadFile.ShowDialog() == true)
            {
                pathToFile = loadFile.fileName.Content.ToString();
                separator = loadFile.separator.Text;
            }

            pathToFile = pathToFile.Replace(@"\", "%2F%2F");
            pathToFile = pathToFile.Replace(@".", "%2E");

            switch (separator)
            {
                case "Spacja":
                    separator = "%20";
                    break;
                case "Średnik":
                    separator = "%3B";
                    break;
                case "Tabulator":
                    separator = "%20%20%20%20%20";
                    break;
            }

            var path = "http://localhost:8080/data/originalFile/?filePath=";
            path += $"{pathToFile}&separator={separator}";

            GetData(path);
        }

        public async Task GetData(String path)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(path);
                string responseBody = await response.Content.ReadAsStringAsync();

                AddDataToDataGrid(responseBody);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddDataToDataGrid(String responseBody)
        {
            this.dataGrid.ItemsSource = null;
            this.dataGrid.Columns.Clear();
            this.columns.Clear();

            var objects = JsonConvert.DeserializeObject<List<object>>(responseBody);
            var result = objects.Select(obj => JsonConvert.SerializeObject(obj)).ToArray();

            char[] charToTrims = { (char)124, '[', '\\', ']', (char)34 };
            string[] trimResult = new string[result.Length];
            string readyWord = String.Empty;

            for (int i = 0; i < result.Length; i++)
            {
                var temp = result[i].Trim(charToTrims).ToCharArray();
                foreach (var ch in temp)
                {
                    if (ch == (char)34 || ch == '[' || ch == '\\' || ch == ']')
                    {
                        continue;
                    }
                    else
                    {
                        readyWord += ch;
                    }
                }

                trimResult[i] = readyWord;
                readyWord = String.Empty;
            }

            var columnHeaders = trimResult[0].Split(',');
            var columnCount = columnHeaders.Length;

            for (int i = 0; i < columnCount; i++)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Width = new DataGridLength(150);
                column.Header = columnHeaders[i];
                this.columns.Add(columnHeaders[i]);
                column.Binding = new Binding(string.Format("[{0}]", i));
                dataGrid.Columns.Add(column);
            }

            List<string[]> lists = new List<string[]>();

            for (int i = 0; i < trimResult.Length - 1; i++)
            {
                var splittedRow = trimResult[i + 1].Split(',');
                lists.Add(splittedRow);
            }

            this.dataGrid.ItemsSource = lists;
        }

        private void Grouping_Click(object sender, RoutedEventArgs e)
        {
            var grouping = new Grouping();

            if (grouping.ShowDialog() == true)
            {
                WorkingFile_Click(new object(), new RoutedEventArgs());
            }
        }

        private async void WorkingFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var path = "http://localhost:8080/data/workingFile";

                HttpResponseMessage response = await client.GetAsync(path);
                string responseBody = await response.Content.ReadAsStringAsync();

                AddDataToDataGrid(responseBody);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
