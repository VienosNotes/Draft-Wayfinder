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

        #region CMC Calc
        public static IEnumerable<DataPoint> GetCMCAvgPower(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.CMC)
                                .Select(g => new DataPoint(g.Key, g.Any() ? g.Average(c => c.Power) : 0));  

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetCMCAvgToughness(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.CMC)
                .Select(g => new DataPoint(g.Key, g.Any() ? g.Average(c => c.Toughness) : 0));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetCMCMaxPower(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.CMC)
                .Select(g => new DataPoint(g.Key, g.Any() ? g.Max(c => c.Power) : 0));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetCMCMaxToughness(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.CMC)
                .Select(g => new DataPoint(g.Key, g.Any() ? g.Max(c => c.Toughness) : 0));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetCMCAvgRatio(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.CMC)
                .Select(g => new DataPoint(g.Key, g.Key != 0 ? ( g.Any() ? g.Average(c => ((c.Power+c.Toughness)/2)/g.Key) : 0) : 0));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetCMCNumOfCreatures(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.CMC)
                .Select(g => new DataPoint(g.Key, g.Count()));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetCMCNumOfFlyers(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.CMC)
                .Select(g => new DataPoint(g.Key, g.Count(c => c.Text?.Contains("Flying") ?? false)));

            return FillMissingValue(groups.ToList());
        }
        #endregion

        #region Toughness Calc
        public static IEnumerable<DataPoint> GetToughnessNumOfCreatures(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.Toughness)
                .Select(g => new DataPoint(g.Key, g.Count()));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetToughnessNumOfFlyers(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.Toughness)
                .Select(g => new DataPoint(g.Key, g.Count(c => c.Text?.Contains("Flying") ?? false)));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetToughnessAvgCMC(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.Toughness)
                .Select(g => new DataPoint(g.Key, g.Any() ? g.Average(c => c.CMC) : 0));

            return FillMissingValue(groups.ToList());
        }


        public static IEnumerable<DataPoint> GetToughnessMinCMC(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.Toughness)
                .Select(g => new DataPoint(g.Key, g.Any() ? g.Min(c => c.CMC) : 0));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetToughnessMaxPower(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.Toughness)
                .Select(g => new DataPoint(g.Key, g.Any() ? g.Max(c => c.Power) : 0));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetToughnessAvgPower(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.Power)
                .Select(g => new DataPoint(g.Key, g.Any() ? g.Average(c => c.Power) : 0));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetToughnessAvgRatio(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.Toughness)
                .Select(g => new DataPoint(g.Key, g.Key != 0 ? (g.Any() ? g.Average(c => ((c.Power + c.Toughness) / 2) / g.Key) : 0) : 0));

            return FillMissingValue(groups.ToList());
        }
        #endregion

        #region Power Calc
        public static IEnumerable<DataPoint> GetPowerNumOfCreatures(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.Power)
                .Select(g => new DataPoint(g.Key, g.Count()));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetPowerNumOfFlyers(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.Power)
                .Select(g => new DataPoint(g.Key, g.Count(c => c.Text?.Contains("Flying") ?? false)));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetPowerAvgCMC(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.Power)
                .Select(g => new DataPoint(g.Key, g.Any() ? g.Average(c => c.CMC) : 0));

            return FillMissingValue(groups.ToList());
        }


        public static IEnumerable<DataPoint> GetPowerMinCMC(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.Power)
                .Select(g => new DataPoint(g.Key, g.Any() ? g.Min(c => c.CMC) : 0));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetPowerMaxToughness(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.Power)
                .Select(g => new DataPoint(g.Key, g.Any() ? g.Max(c => c.Toughness) : 0));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetPowerAvgToughness(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.Power)
                .Select(g => new DataPoint(g.Key, g.Any() ? g.Average(c => c.Toughness) : 0));

            return FillMissingValue(groups.ToList());
        }

        public static IEnumerable<DataPoint> GetPowerAvgRatio(IEnumerable<Card> samples)
        {
            var groups = samples.GroupBy(s => s.Power)
                .Select(g => new DataPoint(g.Key, g.Key != 0 ? (g.Any() ? g.Average(c => ((c.Power + c.Toughness) / 2) / g.Key) : 0) : 0));

            return FillMissingValue(groups.ToList());
        }
        #endregion



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
