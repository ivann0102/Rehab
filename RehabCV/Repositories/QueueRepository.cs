using RehabCV.Database;
using RehabCV.Interfaces;
using RehabCV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Repositories
{
    public class QueueRepository : IQueue<Queue>
    {
        private readonly RehabCVContext _context;

        public QueueRepository(RehabCVContext context)
        {
            _context = context;
        }

        public async Task<string> AddToQueue(Queue queue)
        {
            if (_context != null)
            {
                await _context.Queues.AddAsync(queue);

                await _context.SaveChangesAsync();

                return queue.Id;
            }

            return null;
        }

        public int GetLastNumberOfQueue()
        {
            if (_context != null)
            {
                return _context.Queues.OrderByDescending(x => x.NumberInQueue).FirstOrDefault()?.NumberInQueue ?? 0;
            }

            return 0;
        }
    }
}
