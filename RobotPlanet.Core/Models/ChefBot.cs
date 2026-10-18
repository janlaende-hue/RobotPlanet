using System;
using System.Collections.Generic;
using System.Text;
using RobotPlanet.Core.Interfaces;

namespace RobotPlanet.Core.Models
{
    public class ChefBot: Robot, IScan, Ichargeable
    {
        public ChefBot(string name, int battery) : base(name, battery)
        {
        }
        public string Scan()
        {
            return $"{Name} is scanning the planet for edible plants!";
        }
        public override string CrazyAction()
        {
            return PerformCrazyAction(6, $"{Name} baked a pizza the size of a planet!");
        }
    }
}
