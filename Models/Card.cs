using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace DraftWayfinder.Models
{
    [DataContract]
    public class Card
    {
        [DataMember(Name = "convertedManaCost")]
        public double CMC;
        [DataMember(Name = "power")]
        public string RawPower;

        private double _power = Double.NaN;
        public double Power
        {
            get
            {
                if (double.IsNaN(_power))
                {
                    if (double.TryParse(RawPower, out var nor))
                    {
                        _power = nor;
                    }
                    else
                    {
                        _power = 0;
                    }
                }
                return _power;
            }
        }

        [DataMember(Name = "toughness")]
        public string RawToughness;

        private double _toughness = Double.NaN;
        public double Toughness
        {
            get
            {
                if (double.IsNaN(_toughness))
                {
                    if (double.TryParse(RawToughness, out var nor))
                    {
                        _toughness = nor;
                    }
                    else
                    {
                        _toughness = 0;
                    }
                }
                return _toughness;
            }
        }

        [DataMember(Name = "manaCost")]
        public string Cost;
        [DataMember(Name="rarity")]
        public string RawRarity;

        private Rarity _rarity = Rarity.Unset;
        public Rarity Rarity
        {
            get
            {
                if (_rarity == Rarity.Unset)
                {
                    switch (RawRarity)
                    {
                        case "mythic rare":
                            _rarity = Rarity.Mythic;
                            break;
                        case "rare":
                            _rarity = Rarity.Rare;
                            break;
                        case "uncommon":
                            _rarity = Rarity.Uncommon;
                            break;
                        case "common":
                            _rarity = Rarity.Common;
                            break;
                        case "lands":
                            _rarity = Rarity.BasicLands;
                            break;
                        default:
                            _rarity = Rarity.Others;
                            break;
                    }
                }

                return _rarity;
            }
        }

        [DataMember(Name = "colors")]
        public List<string> RawColors;

        private List<Color> _colors = null;

        public List<Color> Colors
        {
            get
            {
                if (_colors == null)
                {
                    _colors = RawColors.Select(c =>
                    {
                        switch (c)
                        {
                            case "W":
                                return Color.White;
                            case "U":
                                return Color.Blue;
                            case "B":
                                return Color.Black;
                            case "R":
                                return Color.Red;
                            case "G":
                                return Color.Green;
                            default:
                                throw new ArgumentException($"{c} is unknown color.");
                        }
                    }).ToList();
                }

                return _colors;
            }
        }

        [DataMember(Name = "types")]
        public List<string> Types;
    }

    [DataContract]
    public class Cards
    {
        [DataMember(Name = "cards")]
        public List<Card> Body { get; set; }
    }

    public enum Rarity
    {
        Unset, Mythic, Rare, Uncommon, Common, BasicLands, Others
    }

    public enum Color
    {
        Colorless,
        White,
        Blue,
        Black,
        Red,
        Green,
    }
}
