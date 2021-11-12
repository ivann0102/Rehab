using RehabCV.Interfaces;
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
        public static async Task<IEnumerable<RehabViewModel>> GetRehabViewModel(this IEnumerable<Rehabilitation> rehabilitations,
                                                                    IEnumerable<Child> children,
                                                                    IGroup<Group> _group)
        {
            var rehabs = new List<RehabViewModel>();

            if (rehabilitations != null)
            {
                foreach (var value in rehabilitations)
                {
                    var child = children.FirstOrDefault(x => x.Id == value.ChildId);
                    var group = await _group.FindById(child.GroupId);
                    var rehab = new RehabViewModel
                    {
                        FirstNameOfChild = child.FirstName,
                        MiddleNameOfChild = child.MiddleName,
                        LastNameOfChild = child.LastName,
                        NameOfDisease = group.NameOfDisease,
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

        public static async Task AddChildToQueue(this Group group, 
                                                 IQueue<Queue> _queue, 
                                                 Rehabilitation rehabilitation, 
                                                 IGroup<Group> _group,
                                                 IRepository<Child> _child,
                                                 IReserv<Reserve> _reserv)
        {
            var queue = new Queue
            {
                Id = Guid.NewGuid().ToString(),
                RehabilitationId = rehabilitation.Id,
                GroupOfDisease = group.NameOfDisease
            };

            if (await _group.AreSeats(group.NameOfDisease, null))
            {
                await _queue.AddToQueue(queue);
            }
            else
            {
                await _reserv.AddToReserv(_child, rehabilitation.ChildId, queue.GroupOfDisease);
            }
            
        }

        public static async Task<bool> AreSeats(this IGroup<Group> _group, string nameOfDisease, string reserv)
        {
            var group = await _group.FindByName(nameOfDisease);
            var numberOfBusySeats = group.Children.Count;
            var numberOfAllSeats = group.NumberOfChildren;

            if (reserv != null)
            {
                return numberOfAllSeats > numberOfBusySeats;
            }

            return numberOfAllSeats >= numberOfBusySeats;
        }

        public static async Task ChangeDisease(this IGroup<Group> _group, 
                                               IRepository<Child> _child,
                                               Child child,
                                               string nameOfDisease)
        {
            var group = await _group.FindByName(nameOfDisease);

            child.GroupId = group.Id;
            child.Reserve = null;
            child.ReserveId = null;

            await _child.UpdateAsync(child.Id, child);
        }

        public static async Task AddToReserv(this IReserv<Reserve> _reserv, 
                                             IRepository<Child> _child,
                                             string childId,
                                             string groupOfDisease)
        {
            var child = await _child.FindById(childId);

            var reserv =  await _reserv.GetReserv();

            if (reserv == null)
            {
                reserv = new Reserve()
                {
                    Id = Guid.NewGuid().ToString(),
                    NumberInReserv = 1
                };
                
                await _reserv.AddToReserv(reserv);

                child.ReserveId = reserv.Id;
            }
            else
            {
                reserv.NumberInReserv = reserv.Children.Count + 1;

                await _reserv.UpdateReserv(reserv.Id, reserv);

                child.ReserveId = reserv.Id;
            }

            await _child.UpdateAsync(childId, child);
        }
    }
}
