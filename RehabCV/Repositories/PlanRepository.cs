using RehabCV.Database;
using Microsoft.EntityFrameworkCore;
using RehabCV.Models;
using RehabCV.Interfaces;

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
            return await _context.Plans.ToListAsync();
        }

        public async Task<Plan> FindById(string id)
        {
            return await _context.Plans.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Plan> FindByChildId(string id)
        {
            return await _context.Plans
                                            .FirstOrDefaultAsync(x => x.ChildId == id);
        }

        public async Task<Plan> FindByTherapistId(string id)
        {
            return await _context.Plans
                                            .FirstOrDefaultAsync(x => x.TherapistId == id);
        }

        public async Task<string> CreateAsync(Plan plan)
        {
            await _context.Plans.AddAsync(plan);

            await _context.SaveChangesAsync();

            return plan.Id;
        }

        public async Task UpdateAsync(string id, Plan plan)
        {
            plan.Id = id;

            _context.Plans.Update(plan);

            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(string id)
        {
            var result = 0;

            var @plan = _context.Plans.FirstOrDefault(x => x.Id == id);

            if (@plan != null)
            {
                _context.Plans.Remove(@plan);

                result = await _context.SaveChangesAsync();
            }

            return result;
        }

        public async Task<IEnumerable<Plan>> FindByRehabDate(DateTime date)
        {
            return await _context.Plans.Where(x => x.Rehab.DateOfRehab == date).ToListAsync();
        }
    }
}
