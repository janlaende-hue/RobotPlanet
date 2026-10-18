using System;
using System.Collections.Generic;
using RobotPlanet.Core.Models;

namespace RobotPlanet.Services
{
    public class RobotService
    {
        public Robot CreateRobot(string robotType, string name, int battery)
        {
            return robotType switch
            {
                "CleanerBot" => new CleanerBot(name, battery),
                "ExplorerBot" => new ExplorerBot(name, battery),
                "RepairBot" => new RepairBot(name, battery),
                "BuilderBot" => new BuilderBot(name, battery),
                "GuardBot" => new GuardBot(name, battery),
                "ChefBot" => new ChefBot(name, battery),
                "MinerBot" => new MinerBot(name, battery),
                "FarmerBot" => new FarmerBot(name, battery),
                _ => throw new ArgumentException($"Unknown robot type: {robotType}")
            };
        }

        public IEnumerable<string> GetAvailableRobotTypes()
        {
            return new[]
            {
                "CleanerBot",
                "ExplorerBot",
                "RepairBot",
                "GuardBot",
                "BuilderBot",
                "ChefBot",
                "MinerBot",
                "FarmerBot"
            };
        }
    }
}
