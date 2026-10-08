
namespace RobotPlanet.Core.Models;

public abstract class Robot
{
    public string Name { get; }
    public int Battery { get; protected set; }
    protected Robot(string name, int battery)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Robot name cannot be empty.");

        Name = name;
        Battery = battery;
    }
    public virtual String Work()
    {
        if (Battery <= 0)
            return $"{Name} has too little battery left to work.";

        Battery -= 10;
        return $"{Name} worked, battery level: {Battery}";
    }
    public abstract string CrazyAction();
    public override string ToString()
    {
        return $"{Name} ({GetType().Name})";
    }
    public string Charge(int amount)
    {
        Battery = Math.Min(100, Battery + amount);
        return $"{Name} charged its battery.";
    }
}