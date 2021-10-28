using Microsoft.EntityFrameworkCore;
using RehabCV.Database;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Repositories
{
    public class GroupRepository : IGroup<Group>
    {
        private readonly RehabCVContext _context;

        public GroupRepository(RehabCVContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Group>> FindAll()
        {
            if (_context != null)
            {
                return await _context.Groups.ToListAsync();
            }

            return null;
        }

        public async Task<Group> FindById(string id)
        {
            if (_context != null)
            {
                return await _context.Groups
                                 .AsNoTracking()
                                 .AsQueryable()
                                 .Include(c => c.Children)
                                 .FirstOrDefaultAsync(x => x.Id == id);
            }

            return null;
        }

        public async Task<string> CreateAsync(Group group)
        {
            if (_context != null)
            {
                await _context.Groups.AddAsync(group);

                await _context.SaveChangesAsync();

                return group.Id;
            }

            return null;
        }

        public async Task UpdateAsync(string id, Group group)
        {
            if (_context != null)
            {
                group.Id = id;

                _context.Groups.Update(group);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            var result = 0;
            if (_context != null)
            {
                var group = _context.Groups.FirstOrDefault(x => x.Id == id);

                if (group != null)
                {
                    _context.Groups.Remove(group);

                    result = await _context.SaveChangesAsync();
                }

                return result;
            }

            return result;
        }
    }
}
