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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DecisionSupportSystemClient.Modules.Module_2
{
    /// <summary>
    /// Interaction logic for KNNMethod.xaml
    /// </summary>
    public partial class KNNMethod : Window
    {
        HttpClient client;
        int textBoxesCount;

        public KNNMethod()
        {
            InitializeComponent();
            this.client = ClientHelper.GetClient();

            var headers = GetHeaders().Result;
            textBoxesCount = headers.Length;
            InitFieldsDynamicaly(headers);
        }

        private async Task<string[]> GetHeaders()
        {
            try
            {
                var path = "http://localhost:8080/classification/numericHeaders";

                HttpResponseMessage response = client.GetAsync(path).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<string[]>(responseBody);
            }
            catch (Exception ex)
            {
                
            }

            return null;
        }

        private void InitFieldsDynamicaly(string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                TextBlock txtBlock = new TextBlock();
                txtBlock.Text = headers[i];
                txtBlock.Margin = new Thickness(5);
                textBoxes.Children.Add(txtBlock);

                TextBox txtNumber = new TextBox();
                txtNumber.Name = $"txt{i}";
                txtNumber.Margin = new Thickness(5);
                textBoxes.Children.Add(txtNumber);
                textBoxes.RegisterName(txtNumber.Name, txtNumber);
            }
        }

        private void Classify_Click(object sender, RoutedEventArgs e)
        {
            string[] values = new string[textBoxesCount];
            int i = 0;

            
            foreach (var control in textBoxes.Children.OfType<TextBox>())
            {
                values[i] = control.Text;
                i++;
            }

            var metric = this.metric.Text;
            var numberOfNb = this.numberOfNeighbours.Text;
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

            var json = JsonConvert.SerializeObject(values);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var path = $"http://localhost:8080/classification?neighbors=";
            path += $"{numberOfNb}&method={metricNumber}";

            HttpResponseMessage responseMessage = client.PostAsync(path, content).Result;
            string responseBody = responseMessage.Content.ReadAsStringAsync().Result;

            MessageBox.Show($"Obiekt należy do klasy {responseBody}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            DialogResult = true;
        }
    }
}
