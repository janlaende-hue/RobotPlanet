using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using RobotPlanet.Core.Models;

namespace RobotPlanet.Services
{
    public class SimulationService
    {
        public async Task<(double result, long durationMs)> RunSimulationAsync(
            Robot robot,
            long workload,
            CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();

            if (robot.GetType().Name == "FailBot")
            {
                stopwatch.Stop();
                throw new InvalidOperationException($"{robot.Name} (FailBot) encountered a fatal system error during simulation!");
            }

            var result = await Task.Run(() =>
            {
                double value = 0;

                for (long i = 0; i < workload; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    value += Math.Sqrt(i) * Math.Sin(i * 0.001);

                    if (i % 100000 == 0)
                    {
                        Thread.Sleep(1);
                    }
                }

                return value;
            }, cancellationToken).ConfigureAwait(false);

            stopwatch.Stop();
            return (result, stopwatch.ElapsedMilliseconds);
        }
    }
}