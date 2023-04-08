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
    public class CardRepository : ICard<Card>
    {
        private readonly RehabCVContext _context;

        public  CardRepository(RehabCVContext context)
        {
            _context = context;

            if (_context == null)
            {
                throw new ArgumentNullException(nameof(_context));
            }
        }

        public async Task<string> CreateAsync(Card card)
        {
            await _context.Cards.AddAsync(card);

            await _context.SaveChangesAsync();

            return card.Id;
        }

        public async Task<IEnumerable<Card>> FindAll()
        {
            return await _context.Cards.ToListAsync();
        }

        public async Task<Card> FindById(string id)
        {
            return await _context.Cards.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(string id, Card card)
        {
            card.Id = id;

            _context.Cards.Update(card);

            await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(string id)
        {
            var result = 0;
            var card = _context.Cards.FirstOrDefault(x => x.Id == id);

            if (card!= null)
            {
                _context.Cards.Remove(card);

                result = await _context.SaveChangesAsync();
            }

            return result;
        }
    }
}
