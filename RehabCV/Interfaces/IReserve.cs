using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Interfaces
{
    public interface IReserve<TEntity>
    {
        public Task<TEntity> GetReserve();
        public Task AddToReserve(TEntity entity);
        public Task UpdateReserve(string id, TEntity entity);
    }
}
