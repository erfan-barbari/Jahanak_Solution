using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jahanak.HumansSimulation.HumansSimulationModels
{
    public class Human
    {
        public Human()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Human Dad { get; set; }
        public Human Mom { get; set; }

        public DateTime BirthDate { get; set; } = DateTime.Now;
        public DateTime DeathDate { get; set; }
        public string SkinColor { get; set; } = string.Empty;
        public string HireColor { get; set; } = string.Empty;
        public string EyeColor { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

    }
}
