using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RehabCV.Database;
using RehabCV.DTO;
using RehabCV.Interfaces;
using RehabCV.Models;
namespace RehabCV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : Controller
    {
        private const string policy = "RequireAdminRole";

        private readonly IPlan<Plan> _plan;

        public PlanController(IPlan<Plan> plan)
        {
            _plan = plan;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var all = await _plan.FindAll();
            return Ok(all);
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return HtmlEncoder.Default.Encode($"Hello {id}");
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlanDTO planDTO)
        {
            var plan = new Plan
            {
                Id = Guid.NewGuid().ToString(),
                TherapistId = planDTO.TherapistId,
                Description = planDTO.Description,
                NumberOfAppointments = planDTO.NumberOfAppointments,
                RehabId = planDTO.RehabId,
            };

            var resultPlan = await _plan.CreateAsync(plan);
            if (resultPlan != null)
            {
                return StatusCode(StatusCodes.Status201Created, plan);
            }
            return BadRequest();
        }

        [HttpPut("{id}"), ActionName("Update")]
        public async Task<IActionResult> Update(string id, PlanDTO planDTO)
        {
            var plan = await _plan.FindById(id);
            plan.Description = planDTO.Description;
            plan.NumberOfAppointments = planDTO.NumberOfAppointments;
            plan.TherapistId = planDTO.TherapistId;
            await _plan.UpdateAsync(id, plan);
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpDelete("{id}"), ActionName("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var plan = await _plan.DeleteAsync(id);
            if (plan != 0)
            {
                return StatusCode(StatusCodes.Status204NoContent, "Deleted!");
            }
            return BadRequest();
        }

    }
}