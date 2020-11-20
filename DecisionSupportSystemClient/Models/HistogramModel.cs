using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionSupportSystemClient.Models
{
    public class HistogramModel
    {
        public string X { get; set; }
        public double Y { get; set; }

        public HistogramModel()
        {

        }

        public HistogramModel(string x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
