using Jahanak.HumansSimulation;
using Jahanak.HumansSimulation.HumansSimulationModels;

namespace Test
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Human Adam = new()
            {
                Gender = "male",
                Name = "adam",
                SkinColor = "white",
                HireColor = "black",
                EyeColor = "brown"

            };
            Human Hava = new()
            {
                Gender = "female",
                Name = "hava",
                SkinColor = "white",
                HireColor = "yellow",
                EyeColor = "blue"
            };

            Couple Couple = new Couple(Adam, Hava);
            HumanSimulationCore humanSimulation = new HumanSimulationCore();
            humanSimulation.Couples.Add(Couple);
            bool exit = false;
            while (!exit)
            {

                Console.WriteLine("go to date:");
                int yy = Convert.ToInt32(Console.ReadLine());
                int mm = Convert.ToInt32(Console.ReadLine());
                int dd = Convert.ToInt32(Console.ReadLine());
                humanSimulation = humanSimulation.GoToDate(new DateTime(yy, mm, dd));

                Console.ReadKey();
            }
        }


    }
}
