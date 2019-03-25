using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraftWayfinder.Models
{
    public class Card
    {
        public readonly double CMC;
        public readonly double Power;
        public readonly double Toughness;
        public readonly string Cost;
        public readonly Rarity Rarity;
        public readonly Color Color;

        public Card(string cost, int power, int toughness, Rarity rarity)
        {
            CMC = CalcCMC(cost);
            Cost = cost;
            Power = power;
            Toughness = toughness;
            Rarity = rarity;
        }

        public static double CalcCMC(string cost)
        {
            return 0;
        }

        public static Color CalcColor(string cost)
        {
            return Color.Colorless;
        }
    }

    public enum Rarity
    {
        Mythic, Rare, Uncommon, Common, BasicLands, Others
    }

    public enum Color
    {
        Colorless = 0,
        White = 1,
        Blue = 1 >> 1,
        Black = 1 >> 2,
        Red = 1 >> 3,
        Green = 1 >> 4
    }
}
