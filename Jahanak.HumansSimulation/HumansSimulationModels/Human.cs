using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jahanak.HumansSimulation.HumansSimulationModels
{
    public class Human
    {
        public Human(string name, DateTime birthDate)
        {
            Id = Guid.NewGuid();
            Acts = [];
            UndoneActs = [];
            DoingAct = new();
            Name = name;
            BirthDate = birthDate;
            AddAct(new Act
            {
                Id = Guid.NewGuid(),
                ExecutorId = this.Id,
                Name = "FindRel",
                Description = "FindRel for :" + Name,
                Type = "FindRel",
                Status = DoneStatus.Undone.ToString(),
                Executor = FindRel,
                Date = RandomDay(BirthDate.Date.AddYears(15))
            });
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Human Dad { get; set; }
        public Human Mom { get; set; }

        public DateTime BirthDate { get; set; }
        public DateTime DeathDate { get; set; }
        public string SkinColor { get; set; } = string.Empty;
        public string HireColor { get; set; } = string.Empty;
        public string EyeColor { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;


        public List<Act> Acts { get; set; }
        public List<Act> UndoneActs { get; set; }
        public Act DoingAct { get; set; }
        public DateTime LastLiveTime { get; set; }

        public bool InRel { get; set; }
        public DateTime RelDate { get; set; }


        public void FindRel(DateTime dateTime)
        {
            List<Human> Suggestions = HumanManager.Humans.Where(a => (dateTime.Year - BirthDate.Year) >= 15 && a.Gender != Gender && !a.InRel).ToList();
            if (Suggestions.Count != 0)
            {
                Human love = Suggestions.First();
                //Human husband = Gender == "male" ? this : love;
                //Human spouse = Gender == "female" ? this : love;
                love.InRel = InRel = true;
                love.RelDate = RelDate = dateTime;
                CoupleManager.AddCouple(new Couple(Gender == "male" ? this : love, Gender == "female" ? this : love));
                ActManager.DoingAct.Status = DoneStatus.Done.ToString();
                DoingAct = ActManager.DoingAct;
                ActManager.DoingAct.Description = Name + "+" + love.Name;
                CancelFindRelAct(love);
            }
            else
            {
                DelayFindRelAct(this);
            }
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

        public void CancelFindRelAct(Human love)
        {
            Act canceled = love.UndoneActs.Where(a => a.Name == "FindRel").First();
            canceled.Status = DoneStatus.Canceled.ToString();
            love.UndoneActs.Remove(canceled);
            love.Acts.Add(canceled);
        }

        public void DelayFindRelAct(Human me)
        {
            Act delayed = me.UndoneActs.Where(a => a.Name == "FindRel").First();
            delayed.Status = DoneStatus.Delayed.ToString();
            me.UndoneActs.Remove(delayed);
            me.Acts.Add(delayed);

            delayed.Date = RandomDay(delayed.Date);
            delayed.Status = DoneStatus.Undone.ToString();
            Act newAct = delayed;
            AddAct(newAct);
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
