﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Interfaces
{
    public interface IRehabilitation<TEntity> : IRepository<TEntity>
    {
        public Task<IEnumerable<TEntity>> FindAllByParentId(string id);
        public Task<TEntity> FindByChildId(string id);
    }
}
