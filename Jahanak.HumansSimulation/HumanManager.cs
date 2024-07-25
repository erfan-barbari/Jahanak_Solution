using Jahanak.HumansSimulation.HumansSimulationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jahanak.HumansSimulation
{
    public static class HumanManager
    {
        public static List<Human> Humans { get; set; } = [];
        public static void AddHuman(Human human)
        {
            Humans.Add(human);
        }
    }
}
