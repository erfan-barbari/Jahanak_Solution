using Jahanak.HumansSimulation;
using Jahanak.HumansSimulation.HumansSimulationModels;

namespace Test
{
    internal class Program
    {

        static void Main(string[] args)
        {
            string relativePath = "report.txt"; // آدرس نسبی فایل
            string currentDirectory = Directory.GetCurrentDirectory(); // دایرکتوری فعلی
            string absolutePath = Path.Combine(currentDirectory, relativePath); // ساخت آدرس مطلق

            if (File.Exists(absolutePath))
            {
                File.WriteAllText(absolutePath, "");
            }

            Human Adam = new("adam", new DateTime(1, 1, 1))
            {
                Gender = "male",
                SkinColor = "white",
                HireColor = "black",
                EyeColor = "brown"

            };
            Human Hava = new("hava", new DateTime(1, 1, 1))
            {
                Gender = "female",
                SkinColor = "white",
                HireColor = "yellow",
                EyeColor = "blue"
            };

            HumanManager.AddHuman(Adam);
            HumanManager.AddHuman(Hava);

            HumanSimulationCore humanSimulation = new HumanSimulationCore();
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
