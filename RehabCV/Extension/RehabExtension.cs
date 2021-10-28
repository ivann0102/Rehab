using RehabCV.Models;
using RehabCV.Repositories;
using RehabCV.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Extension
{
    public static class RehabExtension
    {
        public static IEnumerable<RehabViewModel> GetRehabViewModel(this IEnumerable<Rehabilitation> rehabilitations,
                                                                    IEnumerable<Child> children)
        {
            var rehabs = new List<RehabViewModel>();

            if (rehabilitations != null)
            {
                foreach (var value in rehabilitations)
                {
                    var rehab = new RehabViewModel
                    {
                        FirstNameOfChild = children.FirstOrDefault(x => x.Id == value.ChildId).FirstName,
                        MiddleNameOfChild = children.FirstOrDefault(x => x.Id == value.ChildId).MiddleName,
                        LastNameOfChild = children.FirstOrDefault(x => x.Id == value.ChildId).LastName,
                        Form = value.Form,
                        Duration = value.Duration,
                        DateOfRehab = value.DateOfRehab
                    };

                    rehabs.Add(rehab);
                }
            }

            return rehabs;
        }

        public static Group DivisionChildrenInGroups(this Group group, int numberOfChildren)
        {
            switch (group.NameOfDisease)
            {
                case "Неврологія":
                    group.NumberOfChildren = (int)Math.Round(numberOfChildren * 0.4, MidpointRounding.AwayFromZero);
                    break;
                case "Ортопедія":
                    group.NumberOfChildren = (int)Math.Round(numberOfChildren * 0.08, MidpointRounding.AwayFromZero);
                    break;
                case "Інше":
                    group.NumberOfChildren = (int)Math.Round(numberOfChildren * 0.02, MidpointRounding.AwayFromZero);
                    break;
                case "Генетика":
                    group.NumberOfChildren = (int)Math.Round(numberOfChildren * 0.2, MidpointRounding.AwayFromZero);
                    break;
                case "Психіатрія":
                    group.NumberOfChildren = (int)Math.Round(numberOfChildren * 0.3, MidpointRounding.AwayFromZero);
                    break;
            }

            return group;
        }

        public static void AddChildToQueue(this Group group, IQueue<Queue> queue)
        {
            var numberOfChildren = group.Children.Count();
            var numberOfAllSeats = group.NumberOfChildren;
        }
    }
}
