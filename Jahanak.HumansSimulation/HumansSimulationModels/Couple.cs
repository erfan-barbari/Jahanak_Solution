using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Jahanak.HumansSimulation.HumansSimulationModels
{
    public class Couple
    {
        public Couple(Human husband, Human spouse)
        {

            LastLiveTime = new DateTime(1, 1, 1);
            Husband = husband;
            Spouse = spouse;
            ProducedBabies = [];
            Acts = [];
            UndoneActs = [];
            DoingAct = new();
            AddAct(new Act
            {
                Id = Guid.NewGuid(),
                Name = "Sex",
                Description = "Have sex to potentially become pregnant" + " --> " + husband.Name + " + " + spouse.Name,
                Type = "Reproductive",
                Status = DoneStatus.Undone.ToString(),
                Executor = HaveSex,
                Date = RandomDay(husband.RelDate)
            });
        }

        public Human Husband { get; set; } = null!;
        public Human Spouse { get; set; } = null!;
        public bool IsPregnant { get; set; }
        public DateTime PregnantDate { get; set; }
        public int OrderPregnancy { get; set; }
        public List<Act> Acts { get; set; }
        public List<Act> UndoneActs { get; set; }
        public Act DoingAct { get; set; }
        public DateTime LastLiveTime { get; set; }
        public List<Human> ProducedBabies { get; set; }

        public Couple GoToDate(DateTime date)
        {
            // استفاده از لیست موقت برای جلوگیری از تغییرات همزمان در لیست اصلی
            List<Act> tempUndoneActs = UndoneActs.Where(a => a.Date <= date).ToList();

            while (tempUndoneActs.Any())
            {
                DoingAct = tempUndoneActs.First();
                tempUndoneActs.RemoveAt(0);
                DoingAct.Executor(DoingAct.Date);

                if (DoingAct.Status == DoneStatus.Done.ToString())
                {
                    Acts.Add(DoingAct);
                    UndoneActs.Remove(DoingAct);
                }

                // به‌روزرسانی لیست موقت در صورت اضافه شدن اعمال جدید
                tempUndoneActs = UndoneActs.Where(a => a.Date <= date).ToList();
            }

            LastLiveTime = date;
            return this;
        }

        public void HaveSex(DateTime dateTime)
        {
            Random generator = new();

            UndoneActs.Remove(ActManager.DoingAct);
            ActManager.DoingAct.Status = DoneStatus.Done.ToString();


            IsPregnant = generator.Next(0, 2) == 1;
            if (IsPregnant)
            {
                PregnantDate = dateTime.AddMonths(9);
                AddAct(new Act
                {
                    Id = Guid.NewGuid(),
                    Name = "GivingBirth",
                    Description = "Giving birth to a child",
                    Type = "Reproductive",
                    Status = DoneStatus.Undone.ToString(),
                    Executor = GivingBirth,
                    Date = PregnantDate
                });
                OrderPregnancy++;
            }

            AddAct(new Act
            {
                Id = Guid.NewGuid(),
                Name = "Sex",
                Description = "Have sex to potentially become pregnant" + " --> " + Husband.Name + " + " + Spouse.Name,
                Type = "Reproductive",
                Status = DoneStatus.Undone.ToString(),
                Executor = HaveSex,
                Date = RandomDay(dateTime)
            });

            Acts.Add(ActManager.DoingAct);
            LastLiveTime = dateTime;
        }


        public void GivingBirth(DateTime dateTime)
        {
            Random generator = new();

            Human baby = new Human(GenerateRandomString(),dateTime)
            {
                BirthDate = dateTime,
                Gender = generator.Next(0, 2) == 0 ? "male" : "female",
                Dad = Husband,
                Mom = Spouse,
                EyeColor = generator.Next(0, 2) == 0 ? Husband.EyeColor : Spouse.EyeColor,
                SkinColor = generator.Next(0, 2) == 0 ? Husband.SkinColor : Spouse.SkinColor,
                HireColor = generator.Next(0, 2) == 0 ? Husband.HireColor : Spouse.HireColor
            };

            UndoneActs.Remove(ActManager.DoingAct);
            ActManager.DoingAct.Status = DoneStatus.Done.ToString();
            ActManager.DoingAct.Description = baby.Dad.Name + "+" + baby.Mom.Name + "=" + baby.Name + "/" + baby.Gender;
            Acts.Add(ActManager.DoingAct);

            ProducedBabies.Add(baby);
            HumanManager.AddHuman(baby);
            LastLiveTime = dateTime;
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


        DateTime RandomDay()
        {
            Random generator = new();
            DateTime start = new DateTime(DateTime.Now.Day);
            int range = (DateTime.Now.AddMonths(12) - DateTime.Today).Days;
            return start.AddDays(generator.Next(range));
        }

        DateTime RandomDay(DateTime startDateTime)
        {
            Random generator = new();
            DateTime start = startDateTime;
            int range = (startDateTime.AddMonths(12) - startDateTime).Days;
            return start.AddDays(generator.Next(range));
        }



        private void AddAct(Act act)
        {

            // فقط آیتم‌های جدید یا آیتم‌هایی که بعد از LastLiveTime هستند را مرتب می‌کنیم
            if (act.Date > LastLiveTime)
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
                ActManager.AddAct(act);
            }
            else
            {
                // اگر تاریخ act جدید قبل از LastLiveTime باشد
                act.Status = DoneStatus.MissedOpportunity.ToString();

                // پیدا کردن اندیس مناسب برای درج act جدید
                int index = Acts.FindIndex(a => a.Date > act.Date);
                if (index != -1)
                {
                    Acts.Insert(index, act);
                }
                else
                {
                    Acts.Add(act);
                }
            }
        }

    }
}
