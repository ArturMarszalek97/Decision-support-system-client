using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for Grouping.xaml
    /// </summary>
    public partial class Grouping : Window
    {
        public Grouping()
        {
            InitializeComponent();
        }

        private async void Ok_Click(object sender, RoutedEventArgs e)
        {
            try
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

                //var json = JsonConvert.SerializeObject(values);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var path = $"http://localhost:8080/classification?neighbors=";
                path += $"{numberOfNb}&method={metricNumber}";

                HttpResponseMessage responseMessage = client.PostAsync(path, content).Result;
                string responseBody = responseMessage.Content.ReadAsStringAsync().Result;

                MessageBox.Show($"Obiekt należy do klasy {responseBody}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
