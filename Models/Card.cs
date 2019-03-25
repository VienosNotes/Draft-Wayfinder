using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DraftWayfinder.Models
{
    [DataContract]
    public class Card
    {
        [DataMember(Name = "convertedManaCost")]
        public double CMC;
        [DataMember(Name = "power")]
        public string RawPower;
        [DataMember(Name = "toughness")]
        public string RawToughness;
        [DataMember(Name = "manaCost")]
        public string Cost;
        [DataMember(Name="rarity")]
        public string Rarity;
        [DataMember(Name = "colors")]
        public List<string> Colors;
        [DataMember(Name = "type")]
        public readonly string Type;
    }

    [DataContract]
    public class Cards
    {
        [DataMember(Name = "cards")]
        public List<Card> Body { get; set; }
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
