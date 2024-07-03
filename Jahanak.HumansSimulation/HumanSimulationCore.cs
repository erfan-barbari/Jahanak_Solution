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
            foreach (Couple couple in Couples)
            {
                couple.GoToDate(date);
                if (couple.OrderPregnancy == 1)
                {
                    FamilyFormation(couple, couple.PregnantDate);
                }
            }
            return this;
        }

        public void FamilyFormation(Couple couple, DateTime createDate)
        {
            FamilyTree.Add(new Family()
            {
                Parents = couple,
                CreateDate = createDate
            });
        }
    }
}
