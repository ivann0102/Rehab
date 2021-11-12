using RehabCV.Database;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using RehabCV.Interfaces;

namespace RehabCV.Repositories
{
    public class ChildRepository : IRepository<Child>
    {
        private readonly RehabCVContext _context;

        public ChildRepository(RehabCVContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Child>> FindAll()
        {
            if (_context != null)
            {
                return await _context.Children.ToListAsync();
            }

            return null;
        }

        public async Task<Child> FindById(string id)
        {
            if (_context != null)
            {
                return await _context.Children.FirstOrDefaultAsync(x => x.Id == id);
            }

            return null;
        }

        public async Task<IEnumerable<Child>> FindByParentId(string parentId)
        {
            if (_context != null)
            {
                return await _context.Children
                                    .AsNoTracking()
                                    .AsQueryable()
                                    .Where(x => x.UserId == parentId).ToListAsync();
            }

            return null;
        }

        public async Task<string> CreateAsync(Child child)
        {
            if (_context != null)
            {
                await _context.Children.AddAsync(child);

                await _context.SaveChangesAsync();

                return child.Id;
            }

            return null;
        }

        public async Task UpdateAsync(string id, Child child)
        {
            if (_context != null)
            {
                child.Id = id;

                _context.Children.Update(child);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            var result = 0;

            if (_context != null)
            {
                var child = _context.Children.FirstOrDefault(x => x.Id == id);

                if (child != null)
                {
                    _context.Children.Remove(child);

                    result =  await _context.SaveChangesAsync();
                }

                return result;
            }

            return result;
        }
    }
}
