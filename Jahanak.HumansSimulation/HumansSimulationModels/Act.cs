using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jahanak.HumansSimulation.HumansSimulationModels
{
    public class Act
    {
        public Guid Id { get; set; }
        public Guid ExecutorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = DoneStatus.Undone.ToString();
        public Action<DateTime> Executor { get; set; } = null!;
        public DateTime Date { get; set; }
    }

    public enum DoneStatus {
        Failure,
        Done,
        Undone,
        Doing,
        GivenUp,
        Canceled,
        Delayed,
        MissedOpportunity
    }
}
