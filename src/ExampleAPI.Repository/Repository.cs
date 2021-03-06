﻿using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using ExampleAPI.Repository.Abstractions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ExampleAPI.Repository {
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new() {

		protected DbContext context;
		protected DbSet<TEntity> dbSet;

		public Repository(DbContext context) {
			this.context = context;
			this.dbSet = context.Set<TEntity>();
		}

		public bool HasDataChanged { get => context.ChangeTracker.HasChanges(); }

		public virtual IQueryable<TEntity> GetWithRawSql(string query,
			params object[] parameters) {
			return dbSet.FromSqlRaw(query, parameters);
		}

		protected virtual IQueryable<TEntity> Get(
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = null,
			bool asNoTracking = true) {

			IQueryable<TEntity> query = dbSet;

			if (asNoTracking) 
				query = query.AsNoTracking();

			if(includeProperties != null) {
				query = includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
					.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
			}

			if (filter != null) {
				query = query.Where(filter);
			}

			return orderBy == null ? query : orderBy(query);
		}

		public virtual async Task<IEnumerable<TEntity>> GetAsync(
			Expression<Func<TEntity, bool>> filter = null,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
			string includeProperties = null,
			bool asNoTracking = true) {

			return await Get(filter, orderBy, includeProperties, asNoTracking).ToListAsync();
		}

		public virtual async Task<TEntity> GetByIdAsync(int id) {
			return await dbSet.FindAsync(id);
		}

		public virtual async Task AddAsync(TEntity entity) {
			await dbSet.AddAsync(entity);
		}

	}
}
