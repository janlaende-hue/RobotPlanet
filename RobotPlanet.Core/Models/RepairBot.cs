using RobotPlanet.Core.Interfaces;

namespace RobotPlanet.Core.Models;

public class RepairBot : Robot, IRepair, Ichargeable
{
    public RepairBot(string name, int Battery) : base(name, Battery)
    {
    }
    public string Repair()
    {
        Battery = Math.Max(0, Battery - 5);
        return $"{Name} is repairing a broken robot!";
    }
    public override string CrazyAction()
    {
        Battery = Math.Min(100, Battery + 30);
        return $"{Name} accidentally upgraded itself!";
    }
}