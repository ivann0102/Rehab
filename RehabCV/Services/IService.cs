using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Services
{
    public interface IService<TEntity>
    {
        List<TEntity> FindById(string id);
        void Create(TEntity entity, string id);
    }
}
