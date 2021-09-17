using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RehabCV.Database;
using RehabCV.DTO;
using RehabCV.Models;
using RehabCV.Repositories;
using RehabCV.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Controllers
{
    public class ChildController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Child> _repository;

        public ChildController(UserManager<User> userManager, IRepository<Child> repository)
        {
            _userManager = userManager;
            _repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var children = await _repository.FindByParentId(user.Id);

            return View(children);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChildDTO childDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                var child = new Child
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    FirstName = childDTO.FirstNameOfChild,
                    MiddleName = childDTO.MiddleNameOfChild,
                    LastName = childDTO.LastNameOfChild,
                    Birthday = childDTO.BirthdayOfChild,
                    Diagnosis = childDTO.Diagnosis,
                    Priority = childDTO.Priority,
                    HomeAddress = childDTO.HomeAddress
                };

                var result = await _repository.CreateAsync(child);

                if (result != null)
                {
                    return RedirectToAction("Index", "Child");
                }
                
            }
            return View(childDTO);
        }

        public async Task<ActionResult> Update(string id)
        {
            var child = await _repository.FindById(id);

            if (child == null )
            {
                return BadRequest();
            }

            var childViewModel = new ChildViewModel
            {
                Id = id,
                FirstNameOfChild = child.FirstName,
                LastNameOfChild = child.LastName,
                MiddleNameOfChild = child.MiddleName,
                BirthdayOfChild = child.Birthday,
                Diagnosis = child.Diagnosis,
                Priority = child.Priority,
                HomeAddress = child.HomeAddress
            };

            return View(childViewModel);
        }

        [HttpPost, ActionName("Update")]
        public async Task<IActionResult> Update(string id, ChildViewModel childViewModel)
        {
            if (ModelState.IsValid)
            {
                var child = await _repository.FindById(id);

                child.FirstName = childViewModel.FirstNameOfChild;
                child.LastName = childViewModel.LastNameOfChild;
                child.MiddleName = childViewModel.MiddleNameOfChild;
                child.Birthday = childViewModel.BirthdayOfChild;
                child.Diagnosis = childViewModel.Diagnosis;
                child.Priority = childViewModel.Priority;
                child.HomeAddress = childViewModel.HomeAddress;

                await _repository.UpdateAsync(id, child);

                return RedirectToAction("Index", "Child");
            }

            return View(childViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var child = await _repository.FindById(id);

            var childViewModel = new ChildViewModel
            {
                Id = id,
                FirstNameOfChild = child.FirstName,
                LastNameOfChild = child.LastName,
                MiddleNameOfChild = child.MiddleName
            };

            return View(childViewModel);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _repository.DeleteAsync(id);

            if (result == 0)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Child");
        }
    }
}
