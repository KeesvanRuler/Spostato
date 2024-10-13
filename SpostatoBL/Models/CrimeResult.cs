using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpostatoBL.Models
{
    public class CrimeResult
    {
        public string ResultMessage { get; set; } = "";
        public double ProgressionGained { get; set; }
        public int MoneyGained { get; set; }
        public double ShootingSkillGained { get; set; }
        public double DrivingSkillGained { get; set; }
        public double BreakoutSkillGained { get; set; }
        public string? CarImagePath { get; set; }
    }
}
