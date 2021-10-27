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

        public static Group DivisionChildrenInGroups(this Group group, int countOfChildren)
        {
            switch (group.NameOfDisease)
            {
                case "Неврологія":
                    group.CountOfChildren = (int)Math.Round(countOfChildren * 0.4, MidpointRounding.AwayFromZero);
                    break;
                case "Ортопедія":
                    group.CountOfChildren = (int)Math.Round(countOfChildren * 0.08, MidpointRounding.AwayFromZero);
                    break;
                case "Інше":
                    group.CountOfChildren = (int)Math.Round(countOfChildren * 0.02, MidpointRounding.AwayFromZero);
                    break;
                case "Генетика":
                    group.CountOfChildren = (int)Math.Round(countOfChildren * 0.2, MidpointRounding.AwayFromZero);
                    break;
                case "Психіатрія":
                    group.CountOfChildren = (int)Math.Round(countOfChildren * 0.3, MidpointRounding.AwayFromZero);
                    break;
            }

            return group;
        }
    }
}
