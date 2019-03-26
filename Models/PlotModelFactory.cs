using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace DraftWayfinder.Models
{
    public static class PlotModelFactory
    {


        public static IEnumerable<DataPoint> GetCMCAvgPower(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.CMC)
                                .Select(g => new DataPoint(g.Key, g.Average(c => c.Power)));                                

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetNumOfCreatures(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.CMC)
                .Select(g => new DataPoint(g.Key, g.Count()));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> FillMissingValue(List<DataPoint> points)
        {
            if (!points.Any())
            {
                return points;
            }

            var xMax = points.Max(p => p.X);

            foreach (var i in Enumerable.Range(0, (int)xMax))
            {
                if (points.Any(p => p.X == i))
                {
                    continue;
                }

                points.Add(new DataPoint(i, 0));
            }

            return points.OrderBy(p => p.X);
        }
    }
}
