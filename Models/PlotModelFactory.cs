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
        private static double NormalizePT(string raw)
        {
            if (double.TryParse(raw, out var nor))
            {
                return nor;
            }

            return 0;
        }

        public static IEnumerable<DataPoint> GetCMCAvgPower(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.CMC)
                                .Select(g => new DataPoint(g.Key, g.Average(c => NormalizePT(c.RawPower))))
                                .OrderBy(dp => dp.X);

            return groups.ToList();
        }

        public static IEnumerable<DataPoint> GetNumOfCreatures(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.CMC)
                .Select(g => new DataPoint(g.Key, g.Count()))
                .OrderBy(dp => dp.X);

            return groups.ToList();
        }
    }
}
