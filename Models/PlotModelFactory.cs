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
                                .Select(g => new DataPoint(g.Key, g.Average(c => c.Power)))
                                .OrderBy(dp => dp.X);

            return groups.ToList();
        }

        public static IEnumerable<DataPoint> GetNumOfCreatures(IEnumerable<Card> samples)
        {
            Console.WriteLine(samples.Count());
            var groups = samples.GroupBy(s => s.CMC)
                .Select(g => new DataPoint(g.Key, g.Count()))
                .OrderBy(dp => dp.X);

            return groups.ToList();
        }
    }
}
