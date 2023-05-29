using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Interfaces
{
    public interface IChild<TEntity> : IRepository<TEntity>
    {
        public Task<IEnumerable<TEntity>> FindAll();
        public Task<TEntity> FindById(string id);
        public Task<int> DeleteAsync(string id);
        public Task<IEnumerable<TEntity>> FindByParentId(string id);
        public Task<IEnumerable<TEntity>> FindByRehabDate(DateTime date);
    }
}
