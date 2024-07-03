using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jahanak.HumansSimulation.HumansSimulationModels
{
    public class Family
    {
        public Family()
        {
            
        }
        public DateTime CreateDate { get; set; }
        public List<Act> Acts { get; set; }
        public List<Act> UndoneActs { get; set; }
        public Act DoingAct { get; set; }
        public DateTime LastLiveTime { get; set; }
        public List<Human> Childrens { get; set; } = new List<Human>();
        public Couple Parents { get; set; } = null!;

        public Family GoToDate(DateTime date)
        {
            return this;
        }

        public void AddChildren(Human c)
        {
            c.Name = GenerateRandomString();
            Childrens.Add(c);
        }

        string GenerateRandomString()
        {
            // حروف الفبای انگلیسی
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random random = new Random();

            // طول تصادفی بین 5 تا 20
            int length = random.Next(5, 21);

            // ایجاد رشته تصادفی
            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}
