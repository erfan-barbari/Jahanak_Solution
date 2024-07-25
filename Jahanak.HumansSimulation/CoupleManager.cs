using Jahanak.HumansSimulation.HumansSimulationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jahanak.HumansSimulation
{
    public static class CoupleManager
    {
        public static List<Couple> Couples { get; set; } = [];

        public static void AddCouple(Couple couple)
        {
            Couples.Add(couple);
        }
    }
}
