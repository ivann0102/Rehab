using Microsoft.AspNetCore.Mvc;
using RehabCV.DTO;
using RehabCV.Models;
using RehabCV.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Controllers
{
    public class DiseaseController : Controller
    {
        private readonly IDisease<Disease> _disease;

        public DiseaseController(IDisease<Disease> disease)
        {
            _disease = disease;
        }

        public async Task<IActionResult> Index()
        {
            var diseases = await _disease.FindAll();

            return View(diseases);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create(DiseaseDTO diseaseDTO)
        {
            if (ModelState.IsValid)
            {
                var disease = new Disease
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = diseaseDTO.Name
                };

                var result = await _disease.CreateAsync(disease);

                if (result != null)
                {
                    RedirectToAction("Index", "Disease");
                }
            }

            return View(diseaseDTO);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            var disease = await _disease.FindById(id);

            var diseaseDTO = new DiseaseDTO
            {
                Name = disease.Name
            };

            return View(diseaseDTO);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _disease.DeleteAsync(id);

            if (result == 0)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Disease");
        }
    }
}
