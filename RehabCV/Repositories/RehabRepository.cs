using RehabCV.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RehabCV.Models;
using RehabCV.Interfaces;

namespace RehabCV.Repositories
{
    public class RehabRepository : IRehabilitation<Rehabilitation>
    {
        private readonly RehabCVContext _context;

        public RehabRepository(RehabCVContext context)
        {
            _context = context;
        }

        public async Task<Rehabilitation> FindByChildId(string id)
        {
            if (_context != null)
            {
                return await _context.Rehabilitations
                                            .FirstOrDefaultAsync(x => x.ChildId == id);
            }

            return null;
        }

        public async Task<string> CreateAsync(Rehabilitation rehab)
        {
            if (_context != null)
            {
                await _context.Rehabilitations.AddAsync(rehab);

                await _context.SaveChangesAsync();

                return rehab.ChildId;
            }

            return null;
        }

        public async Task UpdateAsync(string id, Rehabilitation rehabilitation)
        {
            if (_context != null)
            {
                rehabilitation.Id = id;

                _context.Rehabilitations.Update(rehabilitation);

                await _context.SaveChangesAsync();
            }
        }
    }
}
