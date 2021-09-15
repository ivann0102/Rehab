using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Repositories
{
    public interface IEvent<TEntity>
    {
        Task<IEnumerable<TEntity>> FindAll();
        Task<TEntity> FindById(string id);
        Task<string> CreateAsync(TEntity entity);
        Task UpdateAsync(string id, TEntity entity);
        Task<int> DeleteAsync(string id);
    }
}
