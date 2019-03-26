using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftWayfinder.Models
{
    public static class Selector
    {
        public static IEnumerable<Card> Fetch(IEnumerable<Card> pool, SelectorOptions options)
        {
            if (options.MultiOnly)
            {
                return pool.Where(c => c.Colors.All(col => options.Colors.Contains(col)) &&
                                       c.Colors.Count() >= 2 &&
                                       options.Rarities.Contains(c.Rarity)).ToList();
            }

            return pool.Where(c => c.Colors.All(col => options.Colors.Contains(col)) &&
                                   options.Rarities.Contains(c.Rarity)).ToList();
        }
    }

    public class SelectorOptions
    {
        public IEnumerable<Color> Colors;
        public IEnumerable<Rarity> Rarities;
        public bool MultiOnly = false;
    }
}
