using Jahanak.HumansSimulation.HumansSimulationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jahanak.HumansSimulation
{
    public static class ActManager
    {

        static List<Act> UndoneActs = [];
        static List<Act> DoneActs = [];
        public static Act DoingAct = new();

        public static void AddAct(Act act)
        {
            int index = UndoneActs.FindIndex(a => a.Date > act.Date);
            if (index != -1) 
            {
                UndoneActs.Insert(index, act);
            }
            else
            {
                UndoneActs.Add(act);
            }
        }


        public static void RemoveAct(Act act)
        {
            DoneActs.Add(act);
            UndoneActs.Remove(act);
        }


        public static void ProcessActs(DateTime date)
        {
            var tempActs = UndoneActs.Where(a => a.Date <= date).ToList();

            while (tempActs.Any())
            {
                DoingAct = tempActs.First();
                tempActs.RemoveAt(0);
                if (DoingAct.Status == DoneStatus.Undone.ToString())
                {
                    DoingAct.Executor(DoingAct.Date);
                }
                
                if (DoingAct.Status != DoneStatus.Undone.ToString())
                {
                    RemoveAct(DoingAct);
                }

                tempActs = UndoneActs.Where(a => a.Date <= date).ToList();
            }
        }
    }
}
