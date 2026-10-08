using RobotPlanet.Core.Interfaces;

namespace RobotPlanet.Core.Models
{
    public class GuardBot: Robot, IScan, Ichargeable
    {
        public GuardBot(string name, int Battery) : base(name, Battery)
        {
        }
        public string Scan()
        {
            return $"{Name} is scanning the planet for intruders!";
        }
        public override string CrazyAction()
        {
            if (Battery < 20)
            {
                return $"{Name} fell asleep while on duty!";
            }
            return $"{Name} arrested a vending machine!";
        }
    }
}