using System;
using System.Collections.Generic;
using System.Text;
using RobotPlanet.Core.Interfaces;

namespace RobotPlanet.Core.Models
{
    public class BuilderBot : Robot, Ichargeable
    {
        public BuilderBot(string name, int battery) : base(name, battery)
        {
        }

        public override string CrazyAction()
        {
            return $"{Name} built a giant banana-shaped castle on the Moon!";
        }
    }
}
