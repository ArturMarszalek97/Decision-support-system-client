using DecisionSupportSystemClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DecisionSupportSystemClient.Modules.Module_1.Visualizations
{
    /// <summary>
    /// Interaction logic for DrawHistogram.xaml
    /// </summary>
    public partial class DrawHistogram : Window
    {
        public DrawHistogram(List<double[]> data, string columnName)
        {
            InitializeComponent();

            List<Point> points = new List<Point>();

            for (int i = 0; i < data[0].Length; i++)
            {
                points.Add(new Point { X = data[0][i], Y = data[1][i] });
            }

            ColumnSeries series;

            series = new ColumnSeries();
            series.DependentValuePath = "Y";
            series.IndependentValuePath = "X";
            series.Title = columnName;
            series.ItemsSource = points;
            chart1.Series.Add(series);
        }

        public DrawHistogram(string[] x, double[] y, string columnName)
        {
            InitializeComponent();

            List<HistogramModel> points = new List<HistogramModel>();

            for (int i = 0; i < x.Length; i++)
            {
                points.Add(new HistogramModel { X = x[i], Y = y[i] });
            }

            ColumnSeries series;

            series = new ColumnSeries();
            series.DependentValuePath = "Y";
            series.IndependentValuePath = "X";
            series.Title = columnName;
            series.ItemsSource = points;
            chart1.Series.Add(series);
        }
    }
}
