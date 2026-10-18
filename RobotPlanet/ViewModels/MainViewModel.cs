using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using RobotPlanet.Core.Models;
using RobotPlanet.Core.Interfaces;
using RobotPlanet.Commands;
using RobotPlanet.Services;

namespace RobotPlanet.ViewModels
{
    /// <summary>
    /// Main ViewModel for the RobotPlanet application.
    /// Manages the Robot repository, user interactions, and UI state.
    /// Implements MVVM pattern with INotifyPropertyChanged and ICommand.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Repository<Robot> repository = new();
        private readonly RobotService robotService = new();

        public ObservableCollection<Robot> Robots { get; } = new();
        public ObservableCollection<Robot> DisplayedRobots { get; } = new();
        public ObservableCollection<string> Log { get; } = new();
        public ObservableCollection<string> GroupSummary { get; } = new();

        public ObservableCollection<string> Types { get; } = new();

        private string selectedType = "CleanerBot";
        public string SelectedType
        {
            get => selectedType;
            set { selectedType = value; OnPropertyChanged(nameof(SelectedType)); }
        }

        private string newRobotName = string.Empty;
        public string NewRobotName
        {
            get => newRobotName;
            set { newRobotName = value; OnPropertyChanged(nameof(NewRobotName)); }
        }

        private string filterText = string.Empty;
        public string FilterText
        {
            get => filterText;
            set { filterText = value; OnPropertyChanged(nameof(FilterText)); }
        }

        private Robot? selectedRobot;
        public Robot? SelectedRobot
        {
            get => selectedRobot;
            set { selectedRobot = value; OnPropertyChanged(nameof(SelectedRobot)); }
        }

        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }
        public ICommand PerformTaskCommand { get; }
        public ICommand CrazyCommand { get; }
        public ICommand ScanCommand { get; }
        public ICommand RepairCommand { get; }
        public ICommand ChargeCommand { get; }
        public ICommand ApplyFilterCommand { get; }
        public ICommand SortCommand { get; }
        public ICommand GroupCommand { get; }

        public MainViewModel()
        {
            foreach (var type in robotService.GetAvailableRobotTypes())
                Types.Add(type);

            AddCommand = new RelayCommand(_ => AddRobot(), _ => !string.IsNullOrWhiteSpace(NewRobotName));
            RemoveCommand = new RelayCommand(_ => RemoveRobot(), _ => SelectedRobot != null);
            PerformTaskCommand = new RelayCommand(_ => PerformTask(), _ => SelectedRobot != null);
            CrazyCommand = new RelayCommand(_ => CrazyAction(), _ => SelectedRobot != null);
            ScanCommand = new RelayCommand(_ => Scan(), _ => SelectedRobot != null);
            RepairCommand = new RelayCommand(_ => Repair(), _ => SelectedRobot != null);
            ChargeCommand = new RelayCommand(_ => Charge(), _ => SelectedRobot != null);
            ApplyFilterCommand = new RelayCommand(_ => ApplyFilter());
            SortCommand = new RelayCommand(_ => ApplySort());
            GroupCommand = new RelayCommand(_ => ApplyGroup());

            repository.Added += r =>
            {
                Robots.Add(r);
                if (MatchesFilter(r)) DisplayedRobots.Add(r);
                Log.Add($"{r.Name} added to Robo planet!");
            };
            repository.Removed += r =>
            {
                Robots.Remove(r);
                DisplayedRobots.Remove(r);
                Log.Add($"{r.Name} removed from Robo planet!");
            };
            repository.CrazyActionPerformed += (actor, message) =>
            {
                Log.Add($"{actor.Name} did crazy action: {message}");
                OnPropertyChanged(nameof(SelectedRobot));
            };
            repository.ReactionOccurred += (reactor, message) =>
            {
                Log.Add($"{reactor.Name}: {message}");
                OnPropertyChanged(nameof(SelectedRobot));
            };

            Log.Add("Robot Planet is ready. Add robots to get started!");
        }

        private bool MatchesFilter(Robot r)
        {
            if (string.IsNullOrWhiteSpace(FilterText))
                return true;
            return r.Name.IndexOf(FilterText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void AddRobot()
        {
            Robot robot = robotService.CreateRobot(SelectedType, NewRobotName.Trim(), 100);
            repository.Add(robot);
            NewRobotName = string.Empty;
        }

        private void RemoveRobot()
        {
            if (SelectedRobot == null) return;
            var toRemove = SelectedRobot;
            repository.Remove(toRemove);
            SelectedRobot = null;
        }

        private void PerformTask()
        {
            if (SelectedRobot == null) return;
            Log.Add(SelectedRobot.Work());
            OnPropertyChanged(nameof(SelectedRobot));
        }

        private void CrazyAction()
        {
            if (SelectedRobot == null) return;
            var msg = SelectedRobot.CrazyAction();
            repository.TriggerCrazyAction(SelectedRobot, msg);
        }

        private void Scan()
        {
            if (SelectedRobot == null) return;
            if (SelectedRobot is IScan s)
                Log.Add(s.Scan());
            else
                Log.Add($"{SelectedRobot.Name} cannot scan.");
        }

        private void Repair()
        {
            if (SelectedRobot == null) return;
            if (SelectedRobot is IRepair r)
                Log.Add(r.Repair());
            else
                Log.Add($"{SelectedRobot.Name} cannot repair.");
        }

        private void Charge()
        {
            if (SelectedRobot == null) return;
            if (SelectedRobot is Ichargeable c)
                Log.Add(c.Charge(20));
            else
                Log.Add($"{SelectedRobot.Name} cannot be charged.");

            OnPropertyChanged(nameof(SelectedRobot));
        }

        private void ApplyFilter()
        {
            DisplayedRobots.Clear();
            foreach (var r in Robots.Where(r => MatchesFilter(r)))
                DisplayedRobots.Add(r);
        }

        private void ApplySort()
        {
            var sorted = DisplayedRobots.OrderByDescending(r => r.Battery).ToList();
            DisplayedRobots.Clear();
            foreach (var r in sorted) DisplayedRobots.Add(r);
        }

        private void ApplyGroup()
        {
            GroupSummary.Clear();
            var groups = Robots.GroupBy(r => r.GetType().Name)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count);

            foreach (var g in groups)
                GroupSummary.Add($"{g.Type}: {g.Count}");
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}