using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Repositories
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> FindAll();
        Task<string> CreateAsync(TEntity entity);
        Task UpdateAsync(string id, TEntity entity);
        Task<int> DeleteAsync(string id);
    }
}
