using Google.OrTools.Sat;
using RehabCV.Models;
using RehabCV.Controllers;

namespace RehabCV.Extension
{
    public static class CpModelExtension
    {
        public static CpModel AddChildDayConstraint(
                this CpModel model, Child child, int MAX_WEEKS, 
                Dictionary<(Plan plan, int week, WorkingDays day, string timeslot), BoolVar> timetable, 
                int amount)
        {
            var sum = new List<BoolVar>();
            for (int week = 0; week < MAX_WEEKS; week++)
            {
                foreach (WorkingDays day in Enum.GetValues(typeof(WorkingDays)))
                {
                    var tmp = timetable.Where(el => el.Key.plan.Rehab.ChildId == child.Id
                            && el.Key.week == week && el.Key.day == day);
                    if (tmp.Count() == 0)
                    {
                        continue;
                    }
                    foreach (var el in tmp)
                    {
                        sum.Add(el.Value);
                    }
                    model.Add(LinearExpr.Sum(sum) <= amount);
                    sum.Clear();
                }
            }
            return model;
        }
    }
}