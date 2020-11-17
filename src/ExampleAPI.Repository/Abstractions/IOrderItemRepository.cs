using ExampleAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExampleAPI.Repository.Abstractions {
	public interface IOrderItemRepository : IRepository<OrderItem> {
		Task<IEnumerable<OrderItem>> GetAsync(
			Expression<Func<OrderItem, bool>> filter = null,
			Func<IQueryable<OrderItem>, IOrderedQueryable<OrderItem>> orderBy = null,
			string includeProperties = null,
			bool asNoTracking = true,
			bool includeAllProperties = false);

		Task<IEnumerable<OrderItem>> GetWithoutDetailsAsync(
			Expression<Func<OrderItem, bool>> filter = null,
			Func<IQueryable<OrderItem>, IOrderedQueryable<OrderItem>> orderBy = null);

		Task<IEnumerable<object>> GetGroupedByDetails();

		Task<OrderItem> GetByIdAsync(int id, bool includeAllProperties = false);

		Task<bool> DeleteAsync(int id);

	}
}
