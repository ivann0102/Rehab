using Microsoft.EntityFrameworkCore;
using RehabCV.Database;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Repositories
{
    public class DiseaseRepository : IDisease<Disease>
    {
        private readonly RehabCVContext _context;

        public DiseaseRepository(RehabCVContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Disease>> FindAll()
        {
            if (_context != null)
            {
                return await _context.Diseases.ToListAsync();
            }

            return null;
        }

        public async Task<Disease> FindById(string id)
        {
            if (_context != null)
            {
                return await _context.Diseases.FirstOrDefaultAsync(x => x.Id == id);
            }

            return null;
        }

        public async Task<string> CreateAsync(Disease disease)
        {
            if (_context != null)
            {
                await _context.Diseases.AddAsync(disease);

                await _context.SaveChangesAsync();

                return disease.Id;
            }

            return null;
        }

        public async Task<int> DeleteAsync(string id)
        {
            var result = 0;
            if (_context != null)
            {
                var disease = _context.Diseases.FirstOrDefault(x => x.Id == id);

                if (disease != null)
                {
                    _context.Diseases.Remove(disease);

                    result = await _context.SaveChangesAsync();
                }

                return result;
            }

            return result;
        }
    }
}
