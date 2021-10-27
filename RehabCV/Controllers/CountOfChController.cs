using Microsoft.AspNetCore.Mvc;
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
    public class CountOfChController : Controller
    {
        private readonly ICountOfCh<CountOfChildren> _countOfCh;
        private readonly IGroup<Group> _group;

        public CountOfChController(ICountOfCh<CountOfChildren> countOfCh,
                                   IGroup<Group> group)
        {
            _countOfCh = countOfCh;
            _group = group;
        }

        public async Task<IActionResult> Index()
        {
            var countOfCh = await _countOfCh.GetCount();

            return View(countOfCh);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create(CountOfChildrenDTO countOfChildrenDTO)
        {
            var countOfCh = await _countOfCh.GetCount();

            if (countOfCh != null)
            { 
                return RedirectToAction("Index", "CountOfCh");
            }

            if (ModelState.IsValid)
            {
                var countOfChildren = new CountOfChildren
                {
                    Id = Guid.NewGuid().ToString(),
                    CountOfChildrenInGroup = countOfChildrenDTO.CountOfChildren
                };

                var result = await _countOfCh.CreateAsync(countOfChildren);

                if (result != null)
                {
                    return RedirectToAction("Index", "CountOfCh");
                }
            }

            return View(countOfChildrenDTO);
        }

        public async Task<ActionResult> Update()
        {
            var countOfChildren = await _countOfCh.GetCount();

            if (countOfChildren == null)
            {
                return BadRequest();
            }

            var countOfChildrenDTO = new CountOfChildrenDTO
            {
                CountOfChildren = countOfChildren.CountOfChildrenInGroup
            };

            return View(countOfChildrenDTO);
        }

        [HttpPost, ActionName("Update")]
        public async Task<IActionResult> Update(CountOfChildrenDTO countOfChildrenDTO)
        {
            if (ModelState.IsValid)
            {
                var countOfChildren = await _countOfCh.GetCount();

                countOfChildren.CountOfChildrenInGroup = countOfChildrenDTO.CountOfChildren;

                await _countOfCh.UpdateAsync(countOfChildren);

                var groups = await _group.FindAll();

                foreach (var item in groups)
                {
                    var group = item.DivisionChildrenInGroups(countOfChildrenDTO.CountOfChildren);

                    await _group.UpdateAsync(group.Id, group);
                }

                return RedirectToAction("Index", "CountOfCh");
            }

            return View(countOfChildrenDTO);
        }
    }
}
