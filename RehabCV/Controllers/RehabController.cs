using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RehabCV.DTO;
using RehabCV.Extension;
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

        public RehabController(IRehabilitation<Rehabilitation> rehabilitation, 
                               UserManager<User> userManager,
                               IRepository<Child> child)
        {
            _rehabilitation = rehabilitation;
            _userManager = userManager;
            _child = child;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var rehabilitations = await _rehabilitation.FindAllByParentId(user.Id);

            var children = await _child.FindByParentId(user.Id);

            var rehabViewModel = rehabilitations.GetRehabViewModel(children);

           

            return View(rehabViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var children = await _child.FindByParentId(user.Id);

            ViewBag.children = new SelectList(children, "Id", "FirstName");

            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create(RehabDTO rehabDTO)
        {
            if (ModelState.IsValid)
            {
                var rehab = new Rehabilitation
                {
                    Id = Guid.NewGuid().ToString(),
                    ChildId = rehabDTO.ChildId,
                    Form = rehabDTO.Form,
                    Duration = rehabDTO.Duration,
                    DateOfRehab = rehabDTO.DateOfRehab
                };

                var result = await _rehabilitation.CreateAsync(rehab);

                if (result != null)
                {
                    return RedirectToAction("Index", "Rehab");
                }
            }
    
            return View(rehabDTO);
        }
    }
}
