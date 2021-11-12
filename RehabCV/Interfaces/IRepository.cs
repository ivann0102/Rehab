using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> FindAll();
        Task<TEntity> FindById(string id);
        Task<IEnumerable<TEntity>> FindByParentId(string id);
        Task<string> CreateAsync(TEntity entity);
        Task UpdateAsync(string id, TEntity entity);
        Task<int> DeleteAsync(string id);
    }
}
