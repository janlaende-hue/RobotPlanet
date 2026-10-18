using RobotPlanet.Core.Interfaces;

namespace RobotPlanet.Core.Models;

public class CleanerBot : Robot, Ichargeable
{
    public CleanerBot(string name, int Battery) : base(name, Battery)
    {
    }
    
    public override string CrazyAction()
    {
        return PerformCrazyAction(15, $"{Name} is cleaning the entire planet!");
    }
}