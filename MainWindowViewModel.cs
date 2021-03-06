﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using DraftWayfinder.Models;
using Livet;
using Livet.Commands;
using OxyPlot;
using Color = DraftWayfinder.Models.Color;
using Selector = DraftWayfinder.Models.Selector;


namespace DraftWayfinder
{
    public class MainWindowViewModel : ViewModel
    {
        #region UI string resources
        private static readonly string _cmc = "CMC";
        private static readonly string _power = "Power";
        private static readonly string _toughness = "Toughness";
        private static readonly string _numOfCreatures = "Number of Creatures";
        private static readonly string _numOfFlyers = "Number of Flyers (Experimental)";
        private static readonly string _avgPower = "Average Power";
        private static readonly string _avgToughness = "Average Toughness";
        private static readonly string _maxPower = "Max Power";
        private static readonly string _maxToughness = "Max Toughness";
        private static readonly string _avgRatio = "Average Mana Ratio";
        private static readonly string _avgCMC = "Average CMC";
        private static readonly string _minCMC = "Min CMC";

        private static readonly IReadOnlyCollection<string> _cmcItems = new List<string>
        {
            _numOfCreatures, _numOfFlyers, _avgPower, _avgToughness, _maxPower, _maxToughness, _avgRatio
        };

        private static readonly IReadOnlyCollection<string> _powerItems = new List<string>
        {
            _numOfCreatures, _numOfFlyers, _avgCMC, _minCMC, _avgToughness, _maxToughness, _avgRatio
        };

        private static readonly IReadOnlyCollection<string> _toughnessItems = new List<string>
        {
            _numOfCreatures, _numOfFlyers, _avgCMC, _minCMC, _avgPower, _maxPower, _avgRatio
        };

        #endregion

        #region Binding Properties
        #region Data Selector
        private readonly IReadOnlyCollection<string> _xAxisItems = new ReadOnlyCollection<string>(new List<string>{ _cmc, _power, _toughness });
        public IReadOnlyCollection<string> XAxisItems => _xAxisItems;

        private string _xAxis = _cmc;
        public string XAxis
        {
            get => _xAxis;
            set
            {
                if (_xAxis == value) { return; }
                _xAxis = value;
                RaisePropertyChanged();

                if (value == _cmc)
                {
                    YAxisItems = _cmcItems;
                    YAxis = _cmcItems.First();
                }
                else if (value == _power)
                {
                    YAxisItems = _powerItems;
                    YAxis = _powerItems.First();
                }
                else if (value == _toughness)
                {
                    YAxisItems = _toughnessItems;
                    YAxis = _toughnessItems.First();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private IReadOnlyCollection<string> _yAxisItems = _cmcItems;
        public IReadOnlyCollection<string> YAxisItems
        {
            get => _yAxisItems;
            private set
            {
                if (_yAxisItems == value) { return; }
                _yAxisItems = value;
                RaisePropertyChanged();
            }
        }

        private string _yAxis = _cmcItems.First();
        public string YAxis
        {
            get => _yAxis;
            set
            {
                _yAxis = value;
                UpdatePlot(_xAxis, value);
                RaisePropertyChanged();
            }
        }

        public IReadOnlyCollection<Set> SetItems { get; }

        private Set _set;
        public Set Set
        {
            get => _set;
            set
            {
                if (_set == value) { return; }
                _set = value;
                RaisePropertyChanged();
                LoadPoolImpl();
            }
        }
        #endregion

        #region Plot Source

        #region White
        private IEnumerable<DataPoint> _whiteData = new List<DataPoint>();
        public IEnumerable<DataPoint> WhiteData
        {
            get => _whiteData;
            set
            {
                if (_whiteData == value) { return; }
                _whiteData = value;
                RaisePropertyChanged();
            }
        }

        private List<Card> _whiteSamples = new List<Card>();
        public IEnumerable<Card> WhiteSamples
        {
            get => _whiteSamples;
            set
            {
                if (_whiteSamples == value) { return; }
                _whiteSamples = value.ToList();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Blue
        private IEnumerable<DataPoint> _blueData = new List<DataPoint>();
        public IEnumerable<DataPoint> BlueData
        {
            get => _blueData;
            set
            {
                if (_blueData == value) { return; }
                _blueData = value;
                RaisePropertyChanged();
            }
        }

        private List<Card> _blueSamples = new List<Card>();
        public IEnumerable<Card> BlueSamples
        {
            get => _blueSamples;
            set
            {
                if (_blueSamples == value) { return; }
                _blueSamples = value.ToList();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Black
        private IEnumerable<DataPoint> _blackData = new List<DataPoint>();
        public IEnumerable<DataPoint> BlackData
        {
            get => _blackData;
            set
            {
                if (_blackData == value) { return; }
                _blackData = value;
                RaisePropertyChanged();
            }
        }

        private List<Card> _blackSamples = new List<Card>();
        public IEnumerable<Card> BlackSamples
        {
            get => _blackSamples;
            set
            {
                if (_blackSamples == value) { return; }
                _blackSamples = value.ToList();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Red
        private IEnumerable<DataPoint> _redData = new List<DataPoint>();
        public IEnumerable<DataPoint> RedData
        {
            get => _redData;
            set
            {
                if (_redData == value) { return; }
                _redData = value;
                RaisePropertyChanged();
            }
        }

        private List<Card> _redSamples = new List<Card>();
        public IEnumerable<Card> RedSamples
        {
            get => _redSamples;
            set
            {
                if (_redSamples == value) { return; }
                _redSamples = value.ToList();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Green
        private IEnumerable<DataPoint> _greenData = new List<DataPoint>();
        public IEnumerable<DataPoint> GreenData
        {
            get => _greenData;
            set
            {
                if (_greenData == value) { return; }
                _greenData = value;
                RaisePropertyChanged();
            }
        }

        private List<Card> _greenSamples = new List<Card>();
        public IEnumerable<Card> GreenSamples
        {
            get => _greenSamples;
            set
            {
                if (_greenSamples == value) { return; }
                _greenSamples = value.ToList();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Gold (Multicolored)
        private IEnumerable<DataPoint> _goldData = new List<DataPoint>();
        public IEnumerable<DataPoint> GoldData
        {
            get => _goldData;
            set
            {
                if (_goldData == value) { return; }
                _goldData = value;
                RaisePropertyChanged();
            }
        }

        private List<Card> _goldSamples = new List<Card>();
        public IEnumerable<Card> GoldSamples
        {
            get => _goldSamples;
            set
            {
                if (_goldSamples == value) { return; }
                _goldSamples = value.ToList();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Brown (Colorless)
        private IEnumerable<DataPoint> _brownData = new List<DataPoint>();

        public IEnumerable<DataPoint> BrownData
        {
            get => _brownData;
            set
            {
                if (_brownData == value) { return; }
                _brownData = value;
                RaisePropertyChanged();
            }
        }

        private List<Card> _brownSamples = new List<Card>();
        public IEnumerable<Card> BrownSamples
        {
            get => _brownSamples;
            set
            {
                if (_brownSamples == value) { return; }
                _brownSamples = value.ToList();
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Total
        private IEnumerable<DataPoint> _totalData = new List<DataPoint>();
        public IEnumerable<DataPoint> TotalData
        {
            get => _totalData;
            set
            {
                if (_totalData == value) { return; }
                _totalData = value;
                RaisePropertyChanged();
            }
        }

        private List<Card> _totalSamples = new List<Card>();
        public IEnumerable<Card> TotalSamples
        {
            get => _totalSamples;
            set
            {
                if (_totalSamples == value) { return; }
                _totalSamples = value.ToList();
                RaisePropertyChanged();
            }
        }
        #endregion

        private double _yMax = 10;
        public double YMax
        {
            get => _yMax;
            set
            {
                if (_yMax == value) { return; }
                _yMax = value;
                RaisePropertyChanged();
            }
        }

        private double _xMax = 10;
        public double XMax
        {
            get => _xMax;
            set
            {
                if (_xMax == value) { return; }
                _xMax = value;
                RaisePropertyChanged();
            }

        }

        private double _totalXMax = 10;
        public double TotalXMax
        {
            get => _totalXMax;
            set
            {
                if (_totalXMax == value) { return; }
                _totalXMax = value;
                RaisePropertyChanged();
            }
        }

        private double _totalYMax = 10;
        public double TotalYMax
        {
            get => _totalYMax;
            set
            {
                if (_totalYMax == value) { return; }
                _totalYMax = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Query Parameters
        public IEnumerable<Rarity> Rarities
        {
            get
            {
                var result = new List<Rarity>();

                if (_mythicCheck)
                {
                    result.Add(Rarity.Mythic);
                }

                if (_rareCheck)
                {
                    result.Add(Rarity.Rare);
                }

                if (_uncommonCheck)
                {
                    result.Add(Rarity.Uncommon);
                }

                if (_commonCheck)
                {
                    result.Add(Rarity.Common);
                }

                return result;
            }
        }

        public IEnumerable<Color> Colors
        {
            get
            {
                var result = new List<Color>();

                if (_whiteCheck)
                {
                    result.Add(Color.White);
                }

                if (_blueCheck)
                {
                    result.Add(Color.Blue);
                }

                if (_blackCheck)
                {
                    result.Add(Color.Black);
                }

                if (_redCheck)
                {
                    result.Add(Color.Red);
                }

                if (_greenCheck)
                {
                    result.Add(Color.Green);
                }

                return result;
            }
        }
        //public IEnumerable<Color> Colors => new List<Color> { Color.White, Color.Blue, Color.Black, Color.Red, Color.Green };

        private bool _mythicCheck = false;
        public bool MythicCheck
        {
            get => _mythicCheck;
            set
            {
                if (_mythicCheck == value) { return; }
                _mythicCheck = value;
                RaisePropertyChanged();
                LoadPoolImpl();
            }
        }

        private bool _rareCheck = false;
        public bool RareCheck
        {
            get => _rareCheck;
            set
            {
                if (_rareCheck == value) { return; }
                _rareCheck = value;
                RaisePropertyChanged();
                LoadPoolImpl();
            }
        }

        private bool _uncommonCheck = false;
        public bool UncommonCheck
        {
            get => _uncommonCheck;
            set
            {
                if (_uncommonCheck == value) { return; }
                _uncommonCheck = value;
                RaisePropertyChanged();
                LoadPoolImpl();
            }
        }

        private bool _commonCheck = true;
        public bool CommonCheck
        {
            get => _commonCheck;
            set
            {
                if (_commonCheck == value) { return; }
                _commonCheck = value;
                RaisePropertyChanged();
                LoadPoolImpl();
            }
        }

        private bool _whiteCheck = true;
        public bool WhiteCheck
        {
            get => _whiteCheck;
            set
            {
                if (_whiteCheck == value) { return; }
                _whiteCheck = value;
                RaisePropertyChanged();
                LoadPoolImpl();
            }
        }

        private bool _blueCheck = true;
        public bool BlueCheck
        {
            get => _blueCheck;
            set
            {
                if (_blueCheck == value) { return; }
                _blueCheck = value;
                RaisePropertyChanged();
                LoadPoolImpl();
            }
        }

        private bool _blackCheck = true;
        public bool BlackCheck
        {
            get => _blackCheck;
            set
            {
                if (_blackCheck == value) { return; }
                _blackCheck = value;
                RaisePropertyChanged();
                LoadPoolImpl();
            }
        }

        private bool _redCheck = true;
        public bool RedCheck
        {
            get => _redCheck;
            set
            {
                if (_redCheck == value) { return; }
                _redCheck = value;
                RaisePropertyChanged();
                LoadPoolImpl();
            }
        }

        private bool _greenCheck = true;
        public bool GreenCheck
        {
            get => _greenCheck;
            set
            {
                if (_greenCheck == value) { return; }
                _greenCheck = value;
                RaisePropertyChanged();
                LoadPoolImpl();
            }
        }
        #endregion
        #endregion

        public MainWindowViewModel()
        {
            SetItems = new ReadOnlyCollection<Set>(Set.AllSets().ToList());
            Set = SetItems.FirstOrDefault();
            LoadPoolImpl();
        }

        private ViewModelCommand _loadPoolCommand;
        public ViewModelCommand LoadPoolCommand => _loadPoolCommand ?? (_loadPoolCommand = new ViewModelCommand(LoadPoolImpl));

        private void LoadPoolImpl()
        {
            var cards = Set.GetCards();
            Console.WriteLine($"LOAD {cards.Count()} cards");
            UpdatePlot(_xAxis, _yAxis);
            return;
        }

        private void UpdatePlot(string xAxis, string value)
        {
            var cards = Set.GetCards();
            if (xAxis == _cmc)
            {
                if (value == _numOfCreatures)
                {
                    UpdatePlotInner(PlotModelFactory.GetCMCNumOfCreatures, cards);
                }
                else if (value == _numOfFlyers)
                {
                    UpdatePlotInner(PlotModelFactory.GetCMCNumOfFlyers, cards);
                }
                else if (value == _avgPower)
                {
                    UpdatePlotInner(PlotModelFactory.GetCMCAvgPower, cards);
                }
                else if (value == _avgToughness)
                {
                    UpdatePlotInner(PlotModelFactory.GetCMCAvgToughness, cards);
                }
                else if (value == _maxPower)
                {
                    UpdatePlotInner(PlotModelFactory.GetCMCMaxPower, cards);
                }
                else if (value == _maxToughness)
                {
                    UpdatePlotInner(PlotModelFactory.GetCMCMaxToughness, cards);
                }
                else if (value == _avgRatio)
                {
                    UpdatePlotInner(PlotModelFactory.GetCMCAvgRatio, cards);
                }
            }
            else if (xAxis == _power)
            {
                if (value == _numOfCreatures)
                {
                    UpdatePlotInner(PlotModelFactory.GetPowerNumOfCreatures, cards);
                }
                else if (value == _numOfFlyers)
                {
                    UpdatePlotInner(PlotModelFactory.GetPowerNumOfFlyers, cards);
                }
                else if (value == _avgCMC)
                {
                    UpdatePlotInner(PlotModelFactory.GetPowerAvgCMC, cards);
                }
                else if (value == _minCMC)
                {
                    UpdatePlotInner(PlotModelFactory.GetPowerMinCMC, cards);
                }
                else if (value == _maxToughness)
                {
                    UpdatePlotInner(PlotModelFactory.GetPowerMaxToughness, cards);
                }
                else if (value == _avgToughness)
                {
                    UpdatePlotInner(PlotModelFactory.GetPowerAvgToughness, cards);
                }
                else if (value == _avgRatio)
                {
                    UpdatePlotInner(PlotModelFactory.GetPowerAvgRatio, cards);
                }
            }
            else if (xAxis == _toughness)
            {
                if (value == _numOfCreatures)
                {
                    UpdatePlotInner(PlotModelFactory.GetToughnessNumOfCreatures, cards);
                }
                else if (value == _numOfFlyers)
                {
                    UpdatePlotInner(PlotModelFactory.GetToughnessNumOfFlyers, cards);
                }
                else if (value == _avgCMC)
                {
                    UpdatePlotInner(PlotModelFactory.GetToughnessAvgCMC, cards);
                }
                else if (value == _minCMC)
                {
                    UpdatePlotInner(PlotModelFactory.GetToughnessMinCMC, cards);
                }
                else if (value == _maxToughness)
                {
                    UpdatePlotInner(PlotModelFactory.GetToughnessMaxPower, cards);
                }
                else if (value == _avgToughness)
                {
                    UpdatePlotInner(PlotModelFactory.GetToughnessAvgPower, cards);
                }
                else if (value == _avgRatio)
                {
                    UpdatePlotInner(PlotModelFactory.GetToughnessAvgRatio, cards);
                }
            }

            return;
        }

        private void UpdatePlotInner(Func<IEnumerable<Card>, IEnumerable<DataPoint>> func, IEnumerable<Card> pool)
        {
            var cards = pool.ToList();
            WhiteSamples = Selector.Fetch(cards, new SelectorOptions { Colors = new[] { Color.White }, Rarities = Rarities }).ToList();
            WhiteData = func.Invoke(WhiteSamples).ToList();

            BlueSamples = Selector.Fetch(cards, new SelectorOptions { Colors = new[] { Color.Blue }, Rarities = Rarities });
            BlueData = func.Invoke(BlueSamples).ToList();

            BlackSamples = Selector.Fetch(cards, new SelectorOptions { Colors = new[] { Color.Black }, Rarities = Rarities });
            BlackData = func.Invoke(BlackSamples).ToList();

            RedSamples = Selector.Fetch(cards, new SelectorOptions { Colors = new[] { Color.Red }, Rarities = Rarities });
            RedData = func.Invoke(RedSamples).ToList();

            GreenSamples = Selector.Fetch(cards, new SelectorOptions { Colors = new[] { Color.Green }, Rarities = Rarities });
            GreenData = func.Invoke(GreenSamples).ToList();

            BrownSamples = Selector.Fetch(cards, new SelectorOptions { Colors = new[] { Color.Colorless }, Rarities = Rarities });
            BrownData = func.Invoke(BrownSamples).ToList();

            GoldSamples = Selector.Fetch(cards, new SelectorOptions { Colors = Colors, Rarities = Rarities, MultiOnly = true });
            GoldData = func.Invoke(GoldSamples).ToList();

            var points = new List<IEnumerable<DataPoint>> {WhiteData, BlueData, BlackData, RedData, GreenData, BrownData, GoldData}.SelectMany(l => l).ToList();

            try
            {
                XMax = points.Max(p => p.X);
                YMax = points.Max(p => p.Y);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            var totalColors = Colors.ToList();
            totalColors.Add(Color.Colorless);
            TotalSamples = Selector.Fetch(cards, new SelectorOptions { Colors = totalColors, Rarities = Rarities });
            TotalData = func.Invoke(TotalSamples).ToList();

            try
            {
                TotalXMax = TotalData.Max(p => p.X);
                TotalYMax = TotalData.Max(p => p.Y);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
    }
}
