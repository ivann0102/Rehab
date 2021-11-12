using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RehabCV.DTO;
using RehabCV.Extension;
using RehabCV.Interfaces;
using RehabCV.Models;
using RehabCV.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Controllers
{
    public class RehabController : Controller
    {
        private readonly IRehabilitation<Rehabilitation> _rehabilitation;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Child> _child;
        private readonly IQueue<Queue> _queue;
        private readonly IGroup<Group> _group;
        private readonly IReserv<Reserve> _reserv;
        private readonly IEvent<Event> _event;

        public RehabController(IRehabilitation<Rehabilitation> rehabilitation, 
                               UserManager<User> userManager,
                               IRepository<Child> child,
                               IQueue<Queue> queue,
                               IGroup<Group> group,
                               IReserv<Reserve> reserv,
                               IEvent<Event> @event)
        {
            _rehabilitation = rehabilitation;
            _userManager = userManager;
            _child = child;
            _queue = queue;
            _group = group;
            _reserv = reserv;
            _event = @event;
        }

        public async Task<IActionResult> Parent()
        {
            var parents = await _userManager.GetUsersInRoleAsync("Parent");

            return View(parents);
        }

        public async Task<IActionResult> Index(string id)
        {

            var user = await _userManager.FindByIdAsync(id);

            var rehabilitations = await _rehabilitation.FindAllByParentId(user.Id);

            var children = await _child.FindByParentId(user.Id);

            var rehabViewModel = await rehabilitations.GetRehabViewModel(children, _group);

            return View(rehabViewModel);
        }

        public async Task<IActionResult> Create(string id)
        {
            var @event = await _event.FindAll();
            var dates = new List<Event>();

            foreach (var item in @event)
            {
                if (DateTime.UtcNow.AddDays(7) < item.Start)
                {
                    dates.Add(item);
                }
            }

            ViewBag.dates = new SelectList(dates, "Id", "Start");

            ViewBag.children = id;

            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create(RehabDTO rehabDTO)
        {
            if (ModelState.IsValid)
            {
                var duration = rehabDTO.Form == "Амбулаторна" ? rehabDTO.DurationAmbylator : rehabDTO.DurationStatsionar;

                var dateOfRehab = await _event.FindById(rehabDTO.EventId);

                var rehab = new Rehabilitation
                {
                    Id = Guid.NewGuid().ToString(),
                    ChildId = rehabDTO.ChildId,
                    Form = rehabDTO.Form,
                    Duration = duration,
                    DateOfRehab = dateOfRehab.Start
                };

                var resultRehab = await _rehabilitation.CreateAsync(rehab);

                var child = await _child.FindById(rehabDTO.ChildId);

                var group = await _group.FindById(child.GroupId);

                await group.AddChildToQueue(_queue, rehab, _group, _child, _reserv);

                if (resultRehab != null)
                {
                    return RedirectToAction("Index", "Home");
                }

                await _child.DeleteAsync(rehabDTO.ChildId);
            }
    
            return View(rehabDTO);
        }
    }
}
