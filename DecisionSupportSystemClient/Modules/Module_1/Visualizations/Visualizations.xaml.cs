using DecisionSupportSystemClient.Helpers;
using DecisionSupportSystemClient.Modules.Module_1.DataOperations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DecisionSupportSystemClient.Modules.Module_1.Visualizations
{
    /// <summary>
    /// Interaction logic for Visualizations.xaml
    /// </summary>
    public partial class Visualizations : Window
    {
        HttpClient client;
        List<string> columns;

        public Visualizations()
        {
            InitializeComponent();
            this.client = ClientHelper.GetClient();
            this.columns = new List<string>();
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

        private async void Chart2d_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Chart2d chart2d = new Chart2d(this.columns);
                var column1 = String.Empty;
                var column2 = String.Empty;
                var column3 = String.Empty;
                var choosenColumns = new List<string>();

                if (chart2d.ShowDialog() == true)
                {
                    column1 = chart2d.column1.Text;
                    column2 = chart2d.column2.Text;
                    column3 = chart2d.column3.Text;
                }
                else
                {
                    return;
                }

                var path = "http://localhost:8080/data/2dChart?firstColumnName=";
                path += $"{column1}&secondColumnName={column2}";

                choosenColumns.Add(column1);
                choosenColumns.Add(column2);

                if (!string.IsNullOrEmpty(column3))
                {
                    path += $"&thirdColumnName={column3}";
                    choosenColumns.Add(column3);
                }

                HttpResponseMessage response = await client.GetAsync(path);
                string responseBody = await response.Content.ReadAsStringAsync();

                //var data = GetRawData(responseBody);

                var data = JsonConvert.DeserializeObject<List<List<double>>>(responseBody);

                DrawChart2D drawChart2D = new DrawChart2D(data, choosenColumns);
                drawChart2D.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public List<double[]> GetRawData(string responseBody)
        {
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

            List<double[]> lists = new List<double[]>();

            for (int i = 0; i < trimResult.Length; i++)
            {
                var splittedRow = trimResult[i].Split(',');
                double[] splitterRowInDouble = new double[splittedRow.Length];

                for (int j = 0; j < splittedRow.Length; j++)
                {
                    var convertedValue = double.Parse(splittedRow[j], System.Globalization.CultureInfo.InvariantCulture);
                    splitterRowInDouble[j] = convertedValue;
                }
                lists.Add(splitterRowInDouble);
            }

            return lists;
        }

        private void Chart3d_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Histogram_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Histogram histogram = new Histogram(this.columns);
                var columnName = String.Empty;
                bool? discreteVariable = false;
                var numberOfRanges = 0;

                var path = "http://localhost:8080/data/histogram?columnName=";
                

                if (histogram.ShowDialog() == true)
                {
                    columnName = histogram.columnsName.Text;
                    discreteVariable = histogram.discreteVariable.IsChecked;

                    if (discreteVariable == false)
                    {
                        numberOfRanges = Convert.ToInt32(histogram.numberOfRanges.Text);
                        path += $"{columnName}&discreteVariable={discreteVariable}&numberOfRanges={numberOfRanges}";
                    }
                    else
                    {
                        path += $"{columnName}&discreteVariable={discreteVariable}";
                    }
                }
                else
                {
                    return;
                }

                HttpResponseMessage response = await client.GetAsync(path);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (discreteVariable == true)
                {
                    var data = GetRawData(responseBody);

                    DrawHistogram drawHistogram = new DrawHistogram(data, columnName);
                    drawHistogram.Show();
                }
                else
                {
                    var data = GetRawDataForHistogram(responseBody);
                    var x = GetX(data);
                    var y = GetY(data);

                    DrawHistogram drawHistogram = new DrawHistogram(x, y, columnName);
                    drawHistogram.Show();
                }
 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string[] GetRawDataForHistogram(string responseBody)
        {
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

            return trimResult;
        }

        public string[] GetX(string[] trimResult)
        {
            return trimResult[0].Split(',');
        }

        public double[] GetY(string[] trimResult)
        {
            double[] splitterRowInDouble = null;

            for (int i = 0; i < trimResult.Length - 1; i++)
            {
                var splittedRow = trimResult[1].Split(',');
                splitterRowInDouble = new double[splittedRow.Length];

                for (int j = 0; j < splittedRow.Length; j++)
                {
                    var convertedValue = double.Parse(splittedRow[j], System.Globalization.CultureInfo.InvariantCulture);
                    splitterRowInDouble[j] = convertedValue;
                }
            }

            return splitterRowInDouble;
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

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            var module1Window = new Module1();
            module1Window.Show();

            this.Close();
        }
    }
}
