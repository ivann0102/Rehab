using RehabCV.Database;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RehabCV.Interfaces;

namespace RehabCV.Repositories
{
    public class EventRepository : IEvent<Event>
    {
        private readonly RehabCVContext _context;

        public EventRepository(RehabCVContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> FindAll()
        {
            if (_context != null)
            {
                return await _context.Events.ToListAsync();
            }

            return null;
        }

        public async Task<Event> FindById(string id)
        {
            if (_context != null)
            {
                return await _context.Events.FirstOrDefaultAsync(x => x.Id == id);
            }

            return null;
        }

        public async Task<string> CreateAsync(Event @event)
        {
            if (_context != null)
            {
                await _context.Events.AddAsync(@event);

                await _context.SaveChangesAsync();

                return @event.Id;
            }

            return null;
        }

        public async Task UpdateAsync(string id, Event e)
        {
            if (_context != null)
            {
                e.Id = id;

                _context.Events.Update(e);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            var result = 0;

            if (_context != null)
            {
                var @event = _context.Events.FirstOrDefault(x => x.Id == id);

                if (@event != null)
                {
                    _context.Events.Remove(@event);

                    result = await _context.SaveChangesAsync();
                }

                return result;
            }

            return result;
        }
    }
}
