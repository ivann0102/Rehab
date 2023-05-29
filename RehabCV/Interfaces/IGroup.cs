using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Interfaces
{
    public interface IGroup<TEntity> : IRepository<TEntity>
    {
        public Task<TEntity> FindByName(string name);
        public Task<IEnumerable<TEntity>> FindAll();
        public Task<TEntity> FindById(string id);
        public Task<int> DeleteAsync(string id);
    }
}
