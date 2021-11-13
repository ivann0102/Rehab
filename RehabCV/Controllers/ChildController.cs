﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RehabCV.Database;
using RehabCV.DTO;
using RehabCV.Interfaces;
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
        private readonly IGroup<Group> _group;

        public ChildController(UserManager<User> userManager, 
                               IRepository<Child> repository,
                               IGroup<Group> group)
        {
            _userManager = userManager;
            _repository = repository;
            _group = group;
        }
        public async Task<IActionResult> Index(string id)
        {
            var children = await _repository.FindByParentId(id);

            return View(children);
        }
        public async Task<IActionResult> Parent()
        {
            var parents = await _userManager.GetUsersInRoleAsync("Parent");

            return View(parents);
        }
        public async Task<IActionResult> Create()
        {
            var groups = await _group.FindAll();

            ViewBag.groups = new SelectList(groups, "Id", "NameOfDisease");

            return View();
        }

        [HttpPost, ActionName("Create")]
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
                    GroupId = childDTO.GroupId,
                    HomeAddress = childDTO.HomeAddress
                };

                var result = await _repository.CreateAsync(child);

                if (result != null)
                {
                    return RedirectToAction("Create", "Rehab", new {id = child.Id});
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

            var childDTO = new ChildDTO
            {
                FirstNameOfChild = child.FirstName,
                LastNameOfChild = child.LastName,
                MiddleNameOfChild = child.MiddleName,
                BirthdayOfChild = child.Birthday,
                GroupId = child.GroupId,
                HomeAddress = child.HomeAddress
            };

            var groups = await _group.FindAll();

            ViewBag.groups = new SelectList(groups, "Id", "Name");

            return View(childDTO);
        }

        [HttpPost, ActionName("Update")]
        public async Task<IActionResult> Update(string id, ChildDTO childDTO)
        {
            if (ModelState.IsValid)
            {
                var child = await _repository.FindById(id);

                child.FirstName = childDTO.FirstNameOfChild;
                child.LastName = childDTO.LastNameOfChild;
                child.MiddleName = childDTO.MiddleNameOfChild;
                child.Birthday = childDTO.BirthdayOfChild;
                child.GroupId = childDTO.GroupId;
                child.HomeAddress = childDTO.HomeAddress;

                await _repository.UpdateAsync(id, child);

                return RedirectToAction("Index", "Child");
            }

            return View(childDTO);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var child = await _repository.FindById(id);

            var childDTO = new ChildDTO
            {
                FirstNameOfChild = child.FirstName,
                LastNameOfChild = child.LastName,
                MiddleNameOfChild = child.MiddleName
            };

            return View(childDTO);
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
