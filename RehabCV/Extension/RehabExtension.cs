using RehabCV.Models;
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
            var rehab = new RehabViewModel();

            if (rehabilitations != null)
            {
                foreach (var value in rehabilitations)
                {
                    rehab.FirstNameOfChild = children.FirstOrDefault(x => x.Id == value.ChildId).FirstName;
                    rehab.MiddleNameOfChild = children.FirstOrDefault(x => x.Id == value.ChildId).MiddleName;
                    rehab.LastNameOfChild = children.FirstOrDefault(x => x.Id == value.ChildId).LastName;
                    rehab.Form = value.Form;
                    rehab.Duration = value.Duration;
                    rehab.DateOfRehab = value.DateOfRehab;

                    rehabs.Add(rehab);
                }
            }

            return rehabs;
        }
    }
}
