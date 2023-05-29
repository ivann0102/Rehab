using Microsoft.AspNetCore.Mvc;
using RehabCV.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using RehabCV.Models;
using RehabCV.Repositories;
using RehabCV.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using RehabCV.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RehabCV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventApiController : Controller
    {
        private readonly IEvent<Event> _event;
        private readonly IChild<Child> _childRepository;
        private readonly ITherapist<Therapist> _therapistRepository;
        private const string policy = "RequireAdminRole";
        public EventApiController(IEvent<Event> eventRepository, IChild<Child> childRepository,
            ITherapist<Therapist> therapistRepository)
        {
            _event = eventRepository;
            _childRepository = childRepository;
            _therapistRepository = therapistRepository;
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteGenerated()
        {
            var plan = await _event.DeleteBySubject("Generated");
            if (plan != 0)
            {
                return Ok("Deleted!");
            }
            return BadRequest();
        }
    }
}