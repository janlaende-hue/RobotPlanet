using RobotPlanet.Core.Interfaces;

namespace RobotPlanet.Core.Models;

public class FailBot : Robot, Ichargeable
{
    public FailBot(string name, int battery) : base(name, battery)
    {
    }

    public override string CrazyAction()
    {
        return PerformCrazyAction(100, $"{Name} broke down spectacularly!");
    }
}
