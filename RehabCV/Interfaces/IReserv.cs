using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Interfaces
{
    public interface IReserv<TEntity>
    {
        public Task<TEntity> GetReserv();
        public Task AddToReserv(TEntity entity);
        public Task UpdateReserv(string id, TEntity entity);
    }
}
