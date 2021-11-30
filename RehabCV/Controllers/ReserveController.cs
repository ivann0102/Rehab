using Microsoft.AspNetCore.Mvc;
using RehabCV.Interfaces;
using RehabCV.Models;
using RehabCV.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Controllers
{
    public class ReserveController : Controller
    {
        private readonly IReserve<Reserve> _reserve;
        private readonly IClild<Child> _child;
        private readonly IGroup<Group> _group;
        private readonly IRehabilitation<Rehabilitation> _rehabilitation;

        public ReserveController(IReserve<Reserve> reserve,
                                IClild<Child> child,
                                IGroup<Group> group,
                                IRehabilitation<Rehabilitation> rehabilitation)
        {
            _reserve = reserve;
            _child = child;
            _group = group;
            _rehabilitation = rehabilitation;
        }

        public async Task<IActionResult> Index()
        {
            var reserves = await _reserve.GetReserve();

            var children = reserves?.Children.OrderBy(x => x.DateOfReserv).ToList();

            var reserveList = new List<ReservViewModel>();

            if (reserves != null)
            {
                foreach (var child in children)
                {
                    var rehab = await _rehabilitation.FindByChildId(child.Id);

                    var group = await _group.FindById(child.GroupId);

                    var reservViewModel = new ReservViewModel
                    {
                        FirstName = child.FirstName,
                        MiddleName = child.MiddleName,
                        LastName = child.LastName,
                        NameOfDisease = group.NameOfDisease,
                        DateOfRehab = rehab.DateOfRehab,
                        DateOfReserv = child.DateOfReserv
                    };

                    reserveList.Add(reservViewModel);
                }
            }

            return View(reserveList);
        }
    }
}
