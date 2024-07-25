using Jahanak.HumansSimulation.HumansSimulationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jahanak.HumansSimulation
{
    public class HumanSimulationCore
    {
        public HumanSimulationCore()
        {
            Humans = [];
            Couples = [];
            FamilyTree = [];
        }

        public List<Human> Humans { get; set; }
        public List<Couple> Couples { get; set; }
        public List<Family> FamilyTree { get; set; }

        public HumanSimulationCore GoToDate(DateTime date)
        {
            ActManager.ProcessActs(date);
            Humans = HumanManager.Humans;
            Couples = CoupleManager.Couples;
            return this;
        }
    }
}
