using RehabCV.Database;
using RehabCV.Interfaces;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace RehabCV.Repositories
{
    public class ReserveRepository : IReserve<Reserve>
    {
        public readonly RehabCVContext _context;

        public ReserveRepository(RehabCVContext context)
        {
            _context = context;
        }

        public async Task<Reserve> GetReserve()
         {
            if (_context != null)
            {
                return await _context.Reserves
                                 .AsNoTracking()
                                 .AsQueryable()
                                 .Include(c => c.Children)
                                 .FirstOrDefaultAsync();
            }

            return null;
        }

        public async Task<string> CreateAsync(Reserve reserve)
        {
            if (_context != null)
            {
                await _context.Reserves.AddAsync(reserve);

                await _context.SaveChangesAsync();

                return reserve.Id;
            }

            return null;
        }

        public async Task UpdateAsync(string id, Reserve reserve)
        {
            if (_context != null)
            {
                reserve.Id = id;

                _context.Reserves.Update(reserve);

                await _context.SaveChangesAsync();
            }
        }
    }
}
