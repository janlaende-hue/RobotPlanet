using RobotPlanet.Core.Interfaces;

namespace RobotPlanet.Core.Models;

public class ExplorerBot : Robot, IScan, Ichargeable
{
    public ExplorerBot(string name, int Battery) : base(name, Battery)
    {
    }

    public string Scan()
    {
        return $"{Name} is scanning the planet for resources!";
    }

    public override string CrazyAction()
    {
        return $"{Name} is exploring the entire planet!";
    }
}