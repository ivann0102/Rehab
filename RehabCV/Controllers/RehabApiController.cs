using System.Text.Encodings.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class RehabApiController : Controller
    {
        private readonly IRehabilitation<Rehabilitation> _rehabilitation;
        private readonly UserManager<User> _userManager;
        private readonly IChild<Child> _child;
        private readonly IQueue<Queue> _queue;
        private readonly IGroup<Group> _group;
        private readonly IReserve<Reserve> _reserve;
        private readonly IEvent<Event> _event;
        private const string policy = "RequireAdminRole";

        public RehabApiController(IRehabilitation<Rehabilitation> rehabilitation,
                               UserManager<User> userManager,
                               IChild<Child> child,
                               IQueue<Queue> queue,
                               IGroup<Group> group,
                               IReserve<Reserve> reserve,
                               IEvent<Event> @event)
        {
            _rehabilitation = rehabilitation;
            _userManager = userManager;
            _child = child;
            _queue = queue;
            _group = group;
            _reserve = reserve;
            _event = @event;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var rehabs = await _rehabilitation.FindAll();
            return Ok(rehabs);
        }
    }
}