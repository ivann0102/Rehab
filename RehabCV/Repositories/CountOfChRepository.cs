using Microsoft.EntityFrameworkCore;
using RehabCV.Database;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Repositories
{
    public class CountOfChRepository : ICountOfCh<CountOfChildren>
    {
        private readonly RehabCVContext _context;

        public CountOfChRepository(RehabCVContext context)
        {
            _context = context;
        }

        public async Task<CountOfChildren> GetCount()
        {
            if (_context != null)
            {
                return await _context.CountOfChildren.FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<string> CreateAsync(CountOfChildren countOfChildren)
        {
            if (_context != null)
            {
                await _context.CountOfChildren.AddAsync(countOfChildren);

                await _context.SaveChangesAsync();

                return countOfChildren.Id;
            }

            return null;
        }

        public async Task UpdateAsync(CountOfChildren countOfChildren)
        {
            if (_context != null)
            {
                _context.CountOfChildren.Update(countOfChildren);

                await _context.SaveChangesAsync();
            }
        }
    }
}
