using Microsoft.EntityFrameworkCore;
using RehabCV.Database;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Repositories
{
    public class NumberOfChRepository : INumberOfCh<NumberOfChildren>
    {
        private readonly RehabCVContext _context;

        public NumberOfChRepository(RehabCVContext context)
        {
            _context = context;
        }

        public async Task<NumberOfChildren> GetNumber()
        {
            if (_context != null)
            {
                return await _context.NumberOfChildren.FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<string> CreateAsync(NumberOfChildren numberOfChildren)
        {
            if (_context != null)
            {
                await _context.NumberOfChildren.AddAsync(numberOfChildren);

                await _context.SaveChangesAsync();

                return numberOfChildren.Id;
            }

            return null;
        }

        public async Task UpdateAsync(NumberOfChildren numberOfChildren)
        {
            if (_context != null)
            {
                _context.NumberOfChildren.Update(numberOfChildren);

                await _context.SaveChangesAsync();
            }
        }
    }
}
