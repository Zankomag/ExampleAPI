﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExampleAPI.Repository.Abstractions {

    public interface IRepository<TEntity> where TEntity : class, new() {
		Task<IEnumerable<TEntity>> GetAsync(
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = null,
			bool asNoTracking = true);

		Task<TEntity> GetByIdAsync(int id);
        IQueryable<TEntity> GetWithRawSql(string query,
            params object[] parameters);
        Task AddAsync(TEntity entity);

		bool HasDataChanged { get;  }
    }
}
