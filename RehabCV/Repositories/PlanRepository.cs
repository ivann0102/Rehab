using RehabCV.Database;
using Microsoft.EntityFrameworkCore;
using RehabCV.Models;
using RehabCV.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace RehabCV.Repositories
{
    public class PlanRepository : IPlan<Plan>
    {
        private readonly RehabCVContext _context;

        public PlanRepository(RehabCVContext context)
        {
            _context = context;

            if (_context == null)
            {
                throw new ArgumentNullException(nameof(_context));
            }
        }

        public async Task<IEnumerable<Plan>> FindAll()
        {
            if (_context != null)
                return await _context.Plans.ToListAsync();
            return null;
        }

        public async Task<Plan> FindById(string id)
        {
            if (_context != null)
                return await _context.Plans.FirstOrDefaultAsync(x => x.Id == id);
            return null;
        }

        public async Task<Plan> FindByChildId(string id)
        {
            if (_context != null)
                return await _context.Plans
                                .Include(x => x.Rehab)
                                .Include(x => x.Rehab.Child)
                                .Where(x => x.Rehab.ChildId == id)
                                .FirstOrDefaultAsync();
            return null;
        }

        public async Task<Plan> FindByTherapistId(string id)
        {
            if (_context != null)
                return await _context.Plans
                        .FirstOrDefaultAsync(x => x.TherapistId == id);
            return null;
        }

        public async Task<string> CreateAsync(Plan plan)
        {
            int result = 0;
            if (_context != null)
            {
                await _context.Plans.AddAsync(plan);

                result = await _context.SaveChangesAsync();

                if (result != 0)
                    return plan.Id;
            }
            return null;
        }

        public async Task UpdateAsync(string id, Plan plan)
        {
            if (_context != null)
            {
                plan.Id = id;

                _context.Plans.Update(plan);

                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> DeleteAsync(string id)
        {
            var result = 0;

            if (_context != null)
            {

                var plan = _context.Plans.FirstOrDefault(x => x.Id == id);

                if (plan != null)
                {
                    _context.Plans.Remove(plan);

                    result = await _context.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<IEnumerable<Plan>> FindByRehabDate(DateTime date)
        {
            if (_context != null)
                return await _context.Plans
                            .Include(x => x.Rehab)
                            .Include(x => x.Rehab.Child)
                            .Where(x => x.Rehab.DateOfRehab == date)
                            .ToListAsync();
            return null;
        }
    }
}
