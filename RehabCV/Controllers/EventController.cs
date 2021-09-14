using Microsoft.AspNetCore.Mvc;
using RehabCV.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using RehabCV.Models;

namespace RehabCV.Controllers
{
    public class EventController : Controller
    {
        private readonly RehabCVContext _context;
        public EventController(RehabCVContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetEvents()
        {
            var events = _context.Events.ToList();

            return Json(events, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
        }

        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            if (_context != null)
            {
                if (!String.IsNullOrEmpty(e.Id))
                {
                    //Update the event
                    var v = _context.Events.Where(a => a.Id == e.Id).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    e.Id = Guid.NewGuid().ToString();
                    _context.Events.Add(e);
                }
                _context.SaveChanges();

                status = true;
            }

            return Json(status, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
        }

        [HttpPost]
        public JsonResult DeleteEvent(string id)
        {
            var status = false;
            if (_context != null)
            {
                var v = _context.Events.Where(a => a.Id == id).FirstOrDefault();
                if (v != null)
                {
                    _context.Events.Remove(v);
                    _context.SaveChanges();
                    status = true;
                }
            }

            return Json(status, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
        }
    }
}
