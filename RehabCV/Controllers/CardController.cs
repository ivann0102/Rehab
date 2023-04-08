using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class CardController : Controller
    {
        private readonly ICard<Card> _repository;
        private readonly List<Therapist> therapists;
        private readonly List<Child> children;

        private const string policy = "RequireAdminRole";

        public CardController(ICard<Card> repository)
        {
            _repository = repository;
        }

        [HttpGet, Authorize(Policy = policy)]
        public async Task<IActionResult> Index()
        {
            var cards = await _repository.FindAll();

            return View(cards);
        }

        [HttpGet, Authorize(Policy = policy)]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost, Authorize(Policy = policy), ActionName("Create")]
        public async Task<IActionResult> Create(CardDTO cardDTO)
        {
            if (ModelState.IsValid)
            {
                var therapist = new Card
                {
                    Id = Guid.NewGuid().ToString(),
                    Child = cardDTO.Child,

         
                };

                var result = await _repository.CreateAsync(therapist);

                if (result != null)
                {
                    return RedirectToAction("Index", "Therapist");
                }

            }
            return View(cardDTO);
        }

    }
}
