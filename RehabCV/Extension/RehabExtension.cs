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
    }
}
