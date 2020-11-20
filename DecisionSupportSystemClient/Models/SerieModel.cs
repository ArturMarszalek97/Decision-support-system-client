using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DecisionSupportSystemClient.Models
{
    public class SerieModel : ObservableCollection<Point>
    {
        public string column = String.Empty;

        public SerieModel()
        {
            
        }

        public void AddPoint(double x, double y)
        {
            Add(new Point { X = x, Y = y });
        }
    }
}
