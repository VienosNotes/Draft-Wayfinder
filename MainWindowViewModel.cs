using System;
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
        private static readonly string _numOfFlyers = "Number of Flyers";
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
            _numOfCreatures, _numOfFlyers, _avgCMC, _minCMC, _avgToughness, _maxToughness
        };

        private static readonly IReadOnlyCollection<string> _toughnessItems = new List<string>
        {
            _numOfCreatures, _numOfFlyers, _avgCMC, _minCMC, _avgPower, _maxPower
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

                RaisePropertyChanged(nameof(YAxisItems));
                RaisePropertyChanged(nameof(YAxis));
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
            }
        }

        private string _yAxis = _cmcItems.First();
        public string YAxis
        {
            get => _yAxis;
            set
            {
                if (_yAxis == value) { return; }
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
            }
        }
        #endregion

        #region Plot Source
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
        #endregion

        #region Query Parameters
        public IEnumerable<Rarity> Rarities => new List<Rarity> { Rarity.Common, Rarity.Uncommon, Rarity.Rare, Rarity.Mythic };
        public IEnumerable<Color> Colors => new List<Color> { Color.White, Color.Blue, Color.Black, Color.Red, Color.Green };
                
        #endregion
        #endregion

        public MainWindowViewModel()
        {
            SetItems = new ReadOnlyCollection<Set>(Set.AllSets().ToList());
            _set = SetItems.FirstOrDefault();
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
                if (value == _avgPower)
                {
                    UpdatePlotInner(PlotModelFactory.GetCMCAvgPower, cards);
                }
                else if (value == _numOfCreatures)
                {
                    UpdatePlotInner(PlotModelFactory.GetNumOfCreatures, cards);
                }
            }

            return;
        }

        private void UpdatePlotInner(Func<IEnumerable<Card>, IEnumerable<DataPoint>> func, IEnumerable<Card> pool)
        {
            var cards = pool.ToList();
            WhiteData = func.Invoke(Selector.Fetch(cards, new SelectorOptions {Colors = new[] {Color.White}, Rarities = Rarities})).ToList();
            BlueData = func.Invoke(Selector.Fetch(cards, new SelectorOptions {Colors = new[] {Color.Blue}, Rarities = Rarities})).ToList();
            BlackData = func.Invoke(Selector.Fetch(cards, new SelectorOptions {Colors = new[] {Color.Black}, Rarities = Rarities})).ToList();
            RedData = func.Invoke(Selector.Fetch(cards, new SelectorOptions {Colors = new[] {Color.Red}, Rarities = Rarities})).ToList();
            GreenData = func.Invoke(Selector.Fetch(cards, new SelectorOptions {Colors = new[] {Color.Green}, Rarities = Rarities})).ToList();
            GoldData = func.Invoke(Selector.Fetch(cards, new SelectorOptions { Colors = Colors, Rarities = Rarities, MultiOnly = true })).ToList();

            var points = new List<IEnumerable<DataPoint>> {WhiteData, BlueData, BlackData, RedData, GreenData, GoldData}.SelectMany(l => l).ToList();

            try
            {
                XMax = points.Max(p => p.X);
                YMax = points.Max(p => p.Y);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            TotalData = func.Invoke(Selector.Fetch(cards, new SelectorOptions { Colors = Colors, Rarities = Rarities })).ToList();            
        }
        
    }
}
