using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using RobotPlanet.Commands;
using RobotPlanet.Core.Models;
using RobotPlanet.Models;
using RobotPlanet.Services;

namespace RobotPlanet.ViewModels
{
    public class SimulationViewModel : INotifyPropertyChanged
    {
        private readonly SimulationService simulationService = new();
        private CancellationTokenSource? cancellationTokenSource;

        public ObservableCollection<Robot> AvailableRobots { get; }
        public ObservableCollection<SimulationResult> SimulationResults { get; } = new();

        private Robot? simulationRobot1;
        public Robot? SimulationRobot1
        {
            get => simulationRobot1;
            set { simulationRobot1 = value; OnPropertyChanged(nameof(SimulationRobot1)); }
        }

        private Robot? simulationRobot2;
        public Robot? SimulationRobot2
        {
            get => simulationRobot2;
            set { simulationRobot2 = value; OnPropertyChanged(nameof(SimulationRobot2)); }
        }

        private Robot? simulationRobot3;
        public Robot? SimulationRobot3
        {
            get => simulationRobot3;
            set { simulationRobot3 = value; OnPropertyChanged(nameof(SimulationRobot3)); }
        }

        private string simulationWorkloadInput = "100000000";
        public string SimulationWorkloadInput
        {
            get => simulationWorkloadInput;
            set { simulationWorkloadInput = value; OnPropertyChanged(nameof(SimulationWorkloadInput)); }
        }

        public ICommand RunSequentialCommand { get; }
        public ICommand RunParallelCommand { get; }
        public ICommand SimulationCancelCommand { get; }

        private Action<string> logAction;

        public SimulationViewModel(ObservableCollection<Robot> robots, Action<string> logAction)
        {
            AvailableRobots = robots;
            this.logAction = logAction;

            RunSequentialCommand = new RelayCommand(_ => RunSequential(), _ => CanRunSimulation());
            RunParallelCommand = new RelayCommand(_ => RunParallel(), _ => CanRunSimulation());
            SimulationCancelCommand = new RelayCommand(_ => CancelSimulation(), _ => cancellationTokenSource != null);

            SimulationResults.CollectionChanged += (s, e) => OnPropertyChanged(nameof(SimulationResults));
        }

        private bool CanRunSimulation()
        {
            return SimulationRobot1 != null && SimulationRobot2 != null && SimulationRobot3 != null &&
                   !string.IsNullOrWhiteSpace(SimulationWorkloadInput) &&
                   long.TryParse(SimulationWorkloadInput, out _) &&
                   cancellationTokenSource == null;
        }

        private void RunSequential()
        {
            _ = RunSequentialAsync();
        }

        private async Task RunSequentialAsync()
        {
            cancellationTokenSource = new CancellationTokenSource();
            var robots = new[] { SimulationRobot1!, SimulationRobot2!, SimulationRobot3! };

            SimulationResults.Clear();

            foreach (var robot in robots)
                SimulationResults.Add(new SimulationResult(robot.Name));

            OnPropertyChanged(nameof(SimulationResults));

            var stopwatch = Stopwatch.StartNew();

            try
            {
                if (long.TryParse(SimulationWorkloadInput, out long workload))
                {
                    logAction($"Starting sequential simulation with {robots.Length} robots...");
                    for (int i = 0; i < robots.Length; i++)
                    {
                        var robot = robots[i];
                        var result = SimulationResults[i];

                        try
                        {
                            var (value, durationMs) = await simulationService.RunSimulationAsync(
                                robot,
                                workload,
                                cancellationTokenSource.Token);

                            if (cancellationTokenSource.Token.IsCancellationRequested)
                            {
                                result.Status = "Cancelled";
                            }
                            else
                            {
                                result.Status = "Success";
                                result.Result = value;
                                result.DurationMs = durationMs;
                            }
                        }
                        catch (Exception ex)
                        {
                            result.Status = "Failed";
                            result.Error = ex.Message;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                logAction("Sequential simulation was cancelled.");
            }
            finally
            {
                stopwatch.Stop();
                var successCount = SimulationResults.Count(r => r.Status == "Success");
                var failedCount = SimulationResults.Count(r => r.Status == "Failed");
                var cancelledCount = SimulationResults.Count(r => r.Status == "Cancelled");
                logAction($"Sequential: {successCount} succeeded, {failedCount} failed, {cancelledCount} cancelled. Total time: {stopwatch.ElapsedMilliseconds}ms");
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
        }

        private void RunParallel()
        {
            _ = RunParallelAsync();
        }

        private async Task RunParallelAsync()
        {
            cancellationTokenSource = new CancellationTokenSource();
            var robots = new[] { SimulationRobot1!, SimulationRobot2!, SimulationRobot3! };

            SimulationResults.Clear();

            foreach (var robot in robots)
                SimulationResults.Add(new SimulationResult(robot.Name));

            OnPropertyChanged(nameof(SimulationResults));

            var stopwatch = Stopwatch.StartNew();

            try
            {
                if (long.TryParse(SimulationWorkloadInput, out long workload))
                {
                    logAction($"Starting parallel simulation with {robots.Length} robots...");
                    var tasks = robots.Select((robot, index) =>
                        RunRobotSimulationAsync(robot, index, workload, cancellationTokenSource.Token))
                        .ToArray();

                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                logAction($"Parallel simulation error: {ex.Message}");
            }
            finally
            {
                stopwatch.Stop();
                var successCount = SimulationResults.Count(r => r.Status == "Success");
                var failedCount = SimulationResults.Count(r => r.Status == "Failed");
                var cancelledCount = SimulationResults.Count(r => r.Status == "Cancelled");
                logAction($"Parallel: {successCount} succeeded, {failedCount} failed, {cancelledCount} cancelled. Total time: {stopwatch.ElapsedMilliseconds}ms");
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
        }

        private async Task RunRobotSimulationAsync(Robot robot, int resultIndex, long workload, CancellationToken cancellationToken)
        {
            var result = SimulationResults[resultIndex];

            try
            {
                var (value, durationMs) = await simulationService.RunSimulationAsync(
                    robot,
                    workload,
                    cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    result.Status = "Cancelled";
                }
                else
                {
                    result.Status = "Success";
                    result.Result = value;
                    result.DurationMs = durationMs;
                }
            }
            catch (Exception ex)
            {
                result.Status = "Failed";
                result.Error = ex.Message;
            }
        }

        private void CancelSimulation()
        {
            cancellationTokenSource?.Cancel();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
