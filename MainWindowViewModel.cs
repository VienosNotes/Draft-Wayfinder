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
            return;
        }
    }
}
