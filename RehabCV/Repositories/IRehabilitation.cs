using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Repositories
{
    public interface IRehabilitation<TEntity>
    {
        public Task<IEnumerable<TEntity>> FindAllByParentId(string id);
        public Task<string> CreateAsync(TEntity entity);
    }
}
