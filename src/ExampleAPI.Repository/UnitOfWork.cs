using ExampleAPI.Data;
using ExampleAPI.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExampleAPI.Repository {
	public class UnitOfWork : IUnitOfWork {


		protected DbContext context;
        protected Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public UnitOfWork(InternetShopDbContext dbContext) {
            this.context = dbContext;
		}


		public async Task<IDbContextTransaction> BeginTransactionAsync() {
			return await context.Database.BeginTransactionAsync();
		}

		public async Task CommitAsync(IDbContextTransaction transaction) {
			await transaction.CommitAsync();
		}

		public async Task SaveAsync() {
			await context.SaveChangesAsync();
		}

		private IOrderItemRepository orderItemRepository; 
		public IOrderItemRepository OrderItemRepository {
			get {
				if (orderItemRepository == null)
					orderItemRepository = new OrderItemRepository(context);
				return orderItemRepository;	
			}
		}

		public IRepository<TEntity> BaseRepository<TEntity>() where TEntity : class, new() {
            var type = typeof(TEntity);
            if (repositories.ContainsKey(type))
                return (IRepository<TEntity>)repositories[type];
            repositories.Add(type, new Repository<TEntity>(context));
            return (IRepository<TEntity>)repositories[type];
        }
    }
}
