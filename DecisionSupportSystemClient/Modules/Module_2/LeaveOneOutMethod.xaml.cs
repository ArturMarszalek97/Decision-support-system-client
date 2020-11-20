using DecisionSupportSystemClient.Helpers;
using DecisionSupportSystemClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DecisionSupportSystemClient.Modules.Module_2
{
    /// <summary>
    /// Interaction logic for LeaveOneOutMethod.xaml
    /// </summary>
    public partial class LeaveOneOutMethod : Window
    {
        HttpClient client;

        public LeaveOneOutMethod()
        {
            InitializeComponent();
            this.client = ClientHelper.GetClient();
        }

        private void Classify_Click(object sender, RoutedEventArgs e)
        {
            var metric = this.metric.Text;
            int metricNumber = 0;

            switch (metric)
            {
                case "Metryka Euklidesowa":
                    metricNumber = 1;
                    break;
                case "Metryka Manhattan":
                    metricNumber = 2;
                    break;
                case "Metryka Czebyszewa":
                    metricNumber = 3;
                    break;
                case "Metryka Mahalanobisa":
                    metricNumber = 4;
                    break;
                default:
                    break;
            }

            var path = "http://localhost:8080/classification/qualityRating?method=" + metricNumber.ToString();

            HttpResponseMessage response = client.GetAsync(path).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;

            var data = JsonConvert.DeserializeObject<List<LeaveOneOutDataModel>>(responseBody);

            var x = 1;

            DrawChart(data);
        }

        private void DrawChart(List<LeaveOneOutDataModel> data)
        {
            LineSeries series = new LineSeries();
            series.DependentValuePath = "quality";
            series.IndependentValuePath = "numberOfNeighbors";
            series.Title = "jakość";
            
            series.ItemsSource = data;
            chart1.Series.Add(series);

            this.nb.Content = "Ilość sąsiadów";
            this.quality.Content = "Jakość";

            this.dataGrid.ItemsSource = data;
        }
    }
}
