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
                Description = "Have sex to potentially become pregnant",
                Type = "Reproductive",
                Status = DoneStatus.Undone.ToString(),
                Executor = HaveSex,
                Date = RandomDay()
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

            DoingAct.Status = DoneStatus.Done.ToString();

            AddAct(new Act
            {
                Id = Guid.NewGuid(),
                Name = "Sex",
                Description = "Have sex to potentially become pregnant",
                Type = "Reproductive",
                Status = DoneStatus.Undone.ToString(),
                Executor = HaveSex,
                Date = RandomDay(dateTime)
            });

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

            LastLiveTime = dateTime;
        }


        public void GivingBirth(DateTime dateTime)
        {
            Random generator = new();
            Human baby = new Human
            {
                BirthDate = dateTime,
                Gender = generator.Next(0, 2) == 0 ? "male" : "female",
                Dad = Husband,
                Mom = Spouse,
                EyeColor = generator.Next(0, 2) == 0 ? Husband.EyeColor : Spouse.EyeColor,
                SkinColor = generator.Next(0, 2) == 0 ? Husband.SkinColor : Spouse.SkinColor,
                HireColor = generator.Next(0, 2) == 0 ? Husband.HireColor : Spouse.HireColor
            };

            Act birthAct = UndoneActs.First(a => a.Name == "GivingBirth" && a.Status == DoneStatus.Undone.ToString() && a.Date == dateTime);
            birthAct.Status = DoneStatus.Done.ToString();

            ProducedBabies.Add(baby);
            LastLiveTime = dateTime;
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
