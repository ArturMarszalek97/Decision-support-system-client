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
using System.Windows.Controls.DataVisualization.Charting;
using System.Collections.ObjectModel;
using DecisionSupportSystemClient.Models;

namespace DecisionSupportSystemClient.Modules.Module_1.Visualizations
{
    /// <summary>
    /// Interaction logic for DrawChart2D.xaml
    /// </summary>
    public partial class DrawChart2D : Window
    {
        public List<double[]> data;

        //public DrawChart2D()
        //{
        //    InitializeComponent();

        //    CDFViewModel model = new CDFViewModel();
        //    ScatterSeries series;

        //    for (int i = 0; i < model.CDFPlotCollection.Count; i++)
        //    {
        //        series = new ScatterSeries();
        //        series.DependentValuePath = "Y";
        //        series.IndependentValuePath = "X";
        //        series.ItemsSource = model.CDFPlotCollection[i];
        //        chart1.Series.Add(series);
        //    }
        //}

        public DrawChart2D(List<double[]> data, List<string> columns)
        {
            InitializeComponent();
            this.data = data;

            ObservableCollection<SerieModel> test = new ObservableCollection<SerieModel>();

            if (data.Count == 2)
            {
                SerieModel serie1 = new SerieModel();
                serie1.column = columns[1];

                for (int i = 0; i < data[0].Length; i++)
                {
                    serie1.AddPoint(data[0][i], data[1][i]);
                }

                test.Add(serie1);
            }
            else if (data.Count == 3)
            {
                SerieModel serie1 = new SerieModel();
                SerieModel serie2 = new SerieModel();

                serie1.column = columns[1];
                serie2.column = columns[2];

                for (int i = 0; i < data[0].Length; i++)
                {
                    serie1.AddPoint(data[0][i], data[1][i]);
                    serie2.AddPoint(data[0][i], data[2][i]);
                }

                test.Add(serie1);
                test.Add(serie2);
            }

            ScatterSeries series;

            for (int i = 0; i < test.Count; i++)
            {
                series = new ScatterSeries();
                series.DependentValuePath = "Y";
                series.IndependentValuePath = "X";
                series.Title = test[i].column;
                series.ItemsSource = test[i];
                chart1.Series.Add(series);
            }
        }
    }

    public class CDFPlot : ObservableCollection<Point>
    {
        public CDFPlot(Random r, double delta)
        {
            for (int i = 0; i < 50; i++)
                Add(new Point { X = i, Y = delta + r.NextDouble() });
        }

        public CDFPlot(double x, double y)
        {
            Add(new Point { X = x, Y = y});
        }
    }

    public class CDFViewModel
    {
        public ObservableCollection<CDFPlot> CDFPlotCollection { get; set; }

        public CDFViewModel()
        {
            Random r = new Random();
            CDFPlotCollection = new ObservableCollection<CDFPlot>();

            //for (int i = 0; i < this.; i++)
            //{

            //}

            //CDFPlotCollection.Add(new CDFPlot());

            CDFPlotCollection.Add(new CDFPlot(r, 0));
            CDFPlotCollection.Add(new CDFPlot(r, 2));
        }
    }
}
