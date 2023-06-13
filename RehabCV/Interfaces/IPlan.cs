using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RehabCV.Interfaces
{
    public interface IPlan<TEntity> : IRepository<TEntity>
    {
        public Task<IEnumerable<TEntity>> FindAll();
        public Task<TEntity> FindById(string id);
        public Task<IEnumerable<TEntity>> FindByRehabDate(DateTime date);
        public Task<int> DeleteAsync(string id);
    }
}