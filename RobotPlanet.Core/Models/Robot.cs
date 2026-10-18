
using RobotPlanet.Core.Interfaces;

namespace RobotPlanet.Core.Models;

public abstract class Robot : Ichargeable
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

    protected string PerformCrazyAction(int batteryCost, string action)
    {
        if (Battery < batteryCost)
            return $"{Name} doesn't have enough battery ({Battery}%) to perform crazy action (needs {batteryCost}%).";

        Battery = Math.Max(0, Battery - batteryCost);
        return action + $" Battery now {Battery}%";
    }
    public override string ToString()
    {
        return $"{Name} ({GetType().Name})";
    }
    public virtual string Charge(int amount)
    {
        if (amount < 0)
            return $"{Name} cannot charge with a negative amount.";
        Battery = Math.Min(100, Battery + amount);
        return $"{Name} charged its battery by {amount}%. Battery now {Battery}%";
    }

    public virtual string OnOtherAdded(Robot added)
    {
        Battery = Math.Max(0, Battery - 1);
        return $"{Name} greets {added.Name} and loses 1 battery (now {Battery}%).";
    }

    public virtual string OnOtherRemoved(Robot removed)
    {
        Battery = Math.Max(0, Battery - 2);
        return $"{Name} says goodbye to {removed.Name} and loses 2 battery ({Battery}%).";
    }

    public virtual string OnOtherCrazyAction(Robot actor, string actionDescription)
    {
        Battery = Math.Max(0, Battery - 5);
        return $"{Name} reacted to {actor.Name}'s CrazyAction!";
    }
}