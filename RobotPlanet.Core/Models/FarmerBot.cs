using System;
using System.Collections.Generic;
using System.Text;
using RobotPlanet.Core.Interfaces;

namespace RobotPlanet.Core.Models
{
    public class FarmerBot: Robot, IScan, Ichargeable
    {
        public FarmerBot(string name, int battery) : base(name, battery)
        {
        }

        public string Scan()
        {
            return $"{Name} is scanning the planet for fertile land!";
        }

        public override string CrazyAction()
        {
            return PerformCrazyAction(8, $"{Name} planted a field on an asteroid!");
        }
    }
}
