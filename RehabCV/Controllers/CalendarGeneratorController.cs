using System.Text.Encodings.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RehabCV.Database;
using RehabCV.DTO;
using RehabCV.Interfaces;
using RehabCV.Models;
using Google.OrTools.Sat;
using System.Text;

namespace RehabCV.Controllers
{
    enum WorkingDays
    {
        Monday = 1,
        Tuesday,
        Wednesday,
        Thursday,
        Friday
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CalendarGeneratorController : Controller
    {

        private const string policy = "RequireAdminRole";

        private readonly IChild<Child> _child;
        private readonly IGroup<Group> _group;
        private readonly IPlan<Plan> _plan;
        private readonly IRehabilitation<Rehabilitation> _rehab;
        private readonly ITherapist<Therapist> _therapist;
        private readonly IEvent<Event> _event;
        private readonly List<string> timeslots = new List<string> { "08:20", "09:10", "10:00", "10:50", "11:40", "12:30", "13:20" };
        private readonly int MAX_WEEKS = 8;

        public CalendarGeneratorController(IPlan<Plan> plan, IRehabilitation<Rehabilitation> rehab,
                                            ITherapist<Therapist> therapist, IChild<Child> child,
                                            IEvent<Event> @event)
        {
            _plan = plan;
            _rehab = rehab;
            _child = child;
            _therapist = therapist;
            _event = @event;
        }

        [HttpGet("{date}")]
        public async Task<IActionResult> Get(string date)
        {
            var plans = await _plan.FindByRehabDate(DateTime.Parse(date));
            var children = await _child.FindByRehabDate(DateTime.Parse(date));
            var therapists = await _therapist.FindAll();
            var timetable = new Dictionary<(Plan plan, int week, WorkingDays day, string timeslot), BoolVar>();

            CpModel model = new CpModel();
            int startDay = (int)DateTime.Parse(date).DayOfWeek;
            foreach (Plan plan in plans)
            {
                var rehab = await _rehab.FindById(plan.RehabId);
                int duration = 0;
                switch (rehab.Duration)
                {
                    case ("8 тижнів"):
                        duration = 8;
                        break;
                    case ("4 тижні"):
                        duration = 4;
                        break;
                    case ("2 тижні"):
                        duration = 2;
                        break;
                    default:
                        return BadRequest();
                }
                for (int week = 0; week < duration; week++)
                {

                    foreach (WorkingDays day in Enum.GetValues(typeof(WorkingDays)))
                    {
                        if ((int)day < startDay && week == 0)
                        {
                            continue;
                        }
                        foreach (string timeslot in timeslots)
                        {
                            (Plan plan, int week, WorkingDays day, string timeslot) key = (plan, week, day, timeslot);
                            timetable.Add(key, model.NewBoolVar($"x{plan.Id}{week}{day}{timeslot}"));
                        }
                    }
                }
            }

            var sum = new List<BoolVar>();

            // Child can't have more than 1 activity in 1 timeslot
            foreach (Child child in children)
            {
                for (int week = 0; week < MAX_WEEKS; week++)
                {
                    foreach (WorkingDays day in Enum.GetValues(typeof(WorkingDays)))
                    {
                        foreach (var timeslot in timeslots)
                        {
                            var tmp = timetable.Where(el => el.Key.plan.Rehab.ChildId == child.Id
                                    && el.Key.week == week && el.Key.day == day
                                    && el.Key.timeslot == timeslot);
                            if (tmp.Count() == 0)
                            {
                                continue;
                            }
                            foreach (var el in tmp)
                            {
                                sum.Add(el.Value);
                            }
                            model.Add(LinearExpr.Sum(sum) <= 1);
                            sum.Clear();
                        }
                    }
                }
            }



            // Therapist can't have more than 1 activity in 1 timeslot
            foreach (Therapist therapist in therapists)
            {
                for (int week = 0; week < MAX_WEEKS; week++)
                {
                    foreach (WorkingDays day in Enum.GetValues(typeof(WorkingDays)))
                    {
                        foreach (var timeslot in timeslots)
                        {
                            var tmp = timetable.Where(el => el.Key.plan.TherapistId == therapist.Id
                                    && el.Key.week == week && el.Key.day == day
                                    && el.Key.timeslot == timeslot);
                            if (tmp.Count() == 0)
                                continue;
                            foreach (var el in tmp)
                            {
                                sum.Add(el.Value);
                            }
                            model.Add(LinearExpr.Sum(sum) <= 1);
                            sum.Clear();
                        }
                    }
                }
            }

            foreach (Child child in children)
            {
                int age = (int)((DateTime.Today - child.Birthday).TotalDays / 365.25);
                if (age < 3)
                {
                    for (int week = 0; week < MAX_WEEKS; week++)
                    {
                        foreach (WorkingDays day in Enum.GetValues(typeof(WorkingDays)))
                        {
                            foreach (var timeslot in timeslots)
                            {
                                var tmp = timetable.Where(el => el.Key.plan.Rehab.ChildId == child.Id
                                        && el.Key.week == week && el.Key.day == day
                                        && el.Key.timeslot == timeslot);
                                if (tmp.Count() == 0)
                                    continue;
                                foreach (var el in tmp)
                                {
                                    sum.Add(el.Value);
                                }

                            }
                            if (sum.Count() != 0)
                            {

                                model.Add(LinearExpr.Sum(sum) <= 3);
                                sum.Clear();
                            }
                        }
                    }
                }
                else if (age < 5)
                {
                    for (int week = 0; week < MAX_WEEKS; week++)
                    {
                        foreach (WorkingDays day in Enum.GetValues(typeof(WorkingDays)))
                        {
                            foreach (var timeslot in timeslots)
                            {
                                var tmp = timetable.Where(el => el.Key.plan.Rehab.ChildId == child.Id
                                        && el.Key.week == week && el.Key.day == day
                                        && el.Key.timeslot == timeslot);
                                if (tmp.Count() == 0)
                                    continue;
                                foreach (var el in tmp)
                                {
                                    sum.Add(el.Value);
                                }

                            }
                            if (sum.Count() != 0)
                            {
                                model.Add(LinearExpr.Sum(sum) <= 4);
                                sum.Clear();
                            }
                        }
                    }
                }
            }

            // adhere to plan
            foreach (Plan plan in plans)
            {
                foreach (Child child in children)
                {
                    var tmp = timetable.Where(el => el.Key.plan.Id == plan.Id && el.Key.plan.Rehab.ChildId == child.Id);
                    if (tmp.Count() == 0)
                        continue;
                    foreach (var el in tmp)
                    {
                        sum.Add(el.Value);
                    }
                    model.Add(LinearExpr.Sum(sum) == plan.NumberOfAppointments);
                    sum.Clear();
                }
            }

            IntVar objective = model.NewIntVar(0, 1000000, "objective");
            for (int week = 0; week < MAX_WEEKS; week++)
            {
                foreach (WorkingDays day in Enum.GetValues(typeof(WorkingDays)))
                {
                    foreach (var child in children)
                    {
                        var tmp = timetable.Where(el => el.Key.week == week && el.Key.day == day && el.Key.plan.Rehab.ChildId == child.Id).ToList();
                        if (tmp.Count() == 0)
                            continue;
                        for (int i = 0; i < tmp.Count() - 1; i++)
                        {
                            BoolVar prod = model.NewBoolVar("prod");
                            model.AddMultiplicationEquality(prod, new List<BoolVar> { tmp[i].Value, tmp[i + 1].Value });
                            sum.Add(prod);
                        }
                    }

                }
            }
            if (sum.Count() != 0)
            {
                model.Maximize(LinearExpr.Sum(sum));
                sum.Clear();
            }
            CpSolver solver = new CpSolver();
            CpSolverStatus status = solver.Solve(model);

            Console.WriteLine(model.Validate());
            DateTime startOfWeek = DateTime.Parse(date);
            while (startOfWeek.DayOfWeek != DayOfWeek.Monday)
                startOfWeek = startOfWeek.AddDays(-1);
            startOfWeek = startOfWeek.Date;
            List<Event> events = new List<Event>();
            foreach (var item in timetable)
            {
                if (solver.BooleanValue(item.Value) == true)
                {
                    Console.WriteLine(item.Key.ToString());

                    var @event = new Event()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Subject = $"Generated",
                        Description = $"{item.Key.plan.Description}; Therapist: {item.Key.plan.Rehab.Child.FirstName} {item.Key.plan.Rehab.Child.LastName}; Child:  {item.Key.plan.Therapist.FirstName} {item.Key.plan.Therapist.LastName};",
                        TherapistId = item.Key.plan.TherapistId,
                        ChildId = item.Key.plan.Rehab.ChildId,
                        Start = startOfWeek.AddDays((item.Key.week) * 7).AddDays((int)item.Key.day - 1).Add(TimeSpan.Parse(item.Key.timeslot)),
                        End = startOfWeek.AddDays((item.Key.week) * 7).AddDays((int)item.Key.day - 1).Add(TimeSpan.Parse(item.Key.timeslot)).AddMinutes(40),
                    };
                    await _event.CreateAsync(@event);
                }
            }
            return Ok(status);
        }
    }
}