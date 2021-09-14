using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RehabCV.Database;
using RehabCV.DTO;
using RehabCV.Models;
using RehabCV.Services;
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
        private readonly IService<ChildDTO> _service;
        private readonly IMapper _mapper;

        public ChildController(UserManager<User> userManager, IService<ChildDTO> service, IMapper mapper)
        {
            _userManager = userManager;
            _service = service;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var children = _service.FindById(user.Id).Select(_mapper.Map<Child>).ToList();
            return View(children);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChildDTO childDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                _service.Create(childDTO, user.Id);

                return RedirectToAction("Index", "Child");
            }
            return View(childDTO);
        }
    }
}
