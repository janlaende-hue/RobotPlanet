using System;
using System.Collections.Generic;
using System.Text;
using RobotPlanet.Core.Interfaces;

namespace RobotPlanet.Core.Models
{
    public class MinerBot: Robot, IScan, IRepair, Ichargeable
    {
        public MinerBot(string name, int Battery) : base(name, Battery)
        {
        }
        public string Scan()
        {
            return $"{Name} is scanning the planet for minerals!";
        }
        public string Repair()
        {
            return $"{Name} is repairing the mining equipment!";
        }
        public override string CrazyAction()
        {
            return PerformCrazyAction(10, $"{Name} drilled through a tiny moon!");
        }
    }
}
