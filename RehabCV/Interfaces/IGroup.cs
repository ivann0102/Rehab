using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Interfaces
{
    public interface IGroup<TEntity>
    {
        public Task<IEnumerable<TEntity>> FindAll();
        public Task<TEntity> FindById(string id);
        public Task<TEntity> FindByName(string name);
        public Task<string> CreateAsync(TEntity entity);
        public Task UpdateAsync(string id, TEntity entity);
        public Task<int> DeleteAsync(string id);
    }
}
