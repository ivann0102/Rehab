using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Repositories
{
    public interface ICountOfCh<TEntity>
    {
        public Task<TEntity> GetCount();
        public Task<string> CreateAsync(TEntity entity);
        public Task UpdateAsync(TEntity entity);
    }
}
