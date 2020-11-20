using DecisionSupportSystemClient.Helpers;
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

namespace DecisionSupportSystemClient.Modules.Module_1.DataOperations
{
    /// <summary>
    /// Interaction logic for DataOperations.xaml
    /// </summary>
    public partial class DataOperations : Window
    {
        HttpClient client;
        List<string> columns;

        public DataOperations()
        {
            InitializeComponent();
            this.client = ClientHelper.GetClient();
            this.columns = new List<string>();
            this.dataGrid.CellEditEnding += DataGrid_CellEditEnding;
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var column = e.Column as DataGridBoundColumn;
                if (column != null)
                {
                    var bindingPath = (column.Binding as Binding).Path.Path;
                    if (bindingPath == "Col2")
                    {
                        int rowIndex = e.Row.GetIndex();
                        var el = e.EditingElement as TextBox;
                        // rowIndex has the row index
                        // bindingPath has the column's binding
                        // el.Text has the new, user-entered value
                    }
                }
            }
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

        private async void Normalization_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Normalization normalization = new Normalization(this.columns);
                var choosenColumn = String.Empty;
                var newColumnName = String.Empty;

                if (normalization.ShowDialog() == true)
                {
                    choosenColumn = normalization.columnsName.Text;
                    newColumnName = normalization.newColumnName.Text;
                }
                else
                {
                    return;
                }

                var json = JsonConvert.SerializeObject("");
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var path = "http://localhost:8080/data/normalization?columnName=";
                path += $"{choosenColumn}&&newColumnName={newColumnName}";

                HttpResponseMessage responseMessage = await client.PostAsync(path, content);
                string responseBody = await responseMessage.Content.ReadAsStringAsync();

                AddDataToDataGrid(responseBody);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Discretization_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Discretization discretization = new Discretization(this.columns);
                var choosenColumn = String.Empty;
                var newColumnName = String.Empty;
                int numberOfSections = 0;

                if (discretization.ShowDialog() == true)
                {
                    choosenColumn = discretization.columnsName.Text;
                    newColumnName = discretization.newColumnName.Text;
                    numberOfSections = Convert.ToInt32(discretization.numberOfsections.Text);
                }
                else
                {
                    return;
                }

                var json = JsonConvert.SerializeObject("");
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var path = "http://localhost:8080/data/discretization?columnName=";
                path += $"{choosenColumn}&newColumnName={newColumnName}&numberOfRanges={numberOfSections}";

                HttpResponseMessage responseMessage = await client.PostAsync(path, content);
                string responseBody = await responseMessage.Content.ReadAsStringAsync();

                AddDataToDataGrid(responseBody);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.dataGrid.ItemsSource = null;
            this.dataGrid.Columns.Clear();
        }

        private async void ChangeTextDataToNumeric_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangeTextDataToNumeric changeData = new ChangeTextDataToNumeric(this.columns);
                var choosenColumn = String.Empty;
                var newColumnName = String.Empty;
                bool? isAlphabeticaly = false;

                if (changeData.ShowDialog() == true)
                {
                    choosenColumn = changeData.columnsName.Text;
                    newColumnName = changeData.newColumnName.Text;
                    isAlphabeticaly = changeData.alphabeticaly.IsChecked;
                }
                else
                {
                    return;
                }

                if (isAlphabeticaly == null)
                {
                    isAlphabeticaly = false;
                }

                var json = JsonConvert.SerializeObject("");
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var path = "http://localhost:8080/data/textToNumeric?alphabetical=";
                path += $"{isAlphabeticaly}&columnName={choosenColumn}&newColumnName={newColumnName}";

                HttpResponseMessage responseMessage = await client.PostAsync(path, content);
                string responseBody = await responseMessage.Content.ReadAsStringAsync();

                AddDataToDataGrid(responseBody);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ChangeRangeOfValues_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChangeRangeOfValues changeRange = new ChangeRangeOfValues(this.columns);
                var choosenColumn = String.Empty;
                var newColumnName = String.Empty;
                var newMin = 0;
                var newMax = 0;

                if (changeRange.ShowDialog() == true)
                {
                    choosenColumn = changeRange.columnsName.Text;
                    newColumnName = changeRange.newColumnName.Text;
                    newMin = Convert.ToInt32(changeRange.newMin.Text);
                    newMax = Convert.ToInt32(changeRange.newMax.Text);
                }
                else
                {
                    return;
                }

                var json = JsonConvert.SerializeObject("");
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var path = "http://localhost:8080/data/newRange?columnName=";
                path += $"{choosenColumn}&newColumnName={newColumnName}&newMax={newMax}&newMin={newMin}";

                HttpResponseMessage responseMessage = await client.PostAsync(path, content);
                string responseBody = await responseMessage.Content.ReadAsStringAsync();

                AddDataToDataGrid(responseBody);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void MinMaxValue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MinMaxValue minMaxValue = new MinMaxValue(this.columns);
                var choosenColumn = String.Empty;
                var percentageValues = 0;
                var minOrMax = String.Empty;

                if (minMaxValue.ShowDialog() == true)
                {
                    choosenColumn = minMaxValue.columnsName.Text;
                    percentageValues = Convert.ToInt32(minMaxValue.percentageValues.Text);
                    minOrMax = minMaxValue.minOrMax.Text;
                }
                else
                {
                    return;
                }

                var path = "http://localhost:8080/data/percentageHighestOrSmallestValues?minOrMax=";
                path += $"{minOrMax}&percentageValues={percentageValues}&sortBy={choosenColumn}";

                HttpResponseMessage response = await client.GetAsync(path);
                string responseBody = await response.Content.ReadAsStringAsync();

                AddDataToDataGrid(responseBody);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            var module1Window = new Module1();
            module1Window.Show();

            this.Close();
        }
    }
}
