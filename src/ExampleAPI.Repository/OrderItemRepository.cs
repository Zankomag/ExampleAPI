using ExampleAPI.Data.Models;
using ExampleAPI.Repository.Abstractions;
using ExampleAPI.Repository.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExampleAPI.Repository {
	public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository {
		public OrderItemRepository(DbContext context) : base(context) { }

		public async Task<IEnumerable<OrderItem>> GetAsync(
			Expression<Func<OrderItem, bool>> filter = null,
			Func<IQueryable<OrderItem>, IOrderedQueryable<OrderItem>> orderBy = null,
			string includeProperties = null,
			bool asNoTracking = true,
			bool includeAllProperties = false)
			=> includeAllProperties ?
				await base.Get(filter, orderBy, includeProperties, asNoTracking).IncludeAll().ToListAsync() :
				await base.GetAsync(filter, orderBy, includeProperties, asNoTracking);
		

		public async Task<IEnumerable<OrderItem>> GetWithoutDetailsAsync(
			Expression<Func<OrderItem, bool>> filter = null,
			Func<IQueryable<OrderItem>, IOrderedQueryable<OrderItem>> orderBy = null) 
			=> await base.Get(filter, orderBy)
				.Select(o => new OrderItem {
					Id = o.Id, 
					Quantity = o.Quantity, 
					ItemPrice = o.ItemPrice 
				}).ToListAsync();


		public async Task<OrderItem> GetByIdAsync(int id, bool includeAllProperties = false)
			=> includeAllProperties ?
				await dbSet.IncludeAll().FirstOrDefaultAsync(o => o.Id == id) :
				await base.GetByIdAsync(id);
		
		public async Task<bool> DeleteAsync(int id) {
			var orderItemFact = await dbSet.Where(x => x.Id == id)
				.Select(x => new {
					CustomerId = x.Customer.Id,
					PaymentMethodId = x.Invoice.PaymentMethod.Id,
					OrderId = x.Order.Id,
					ProductId = x.Product.Id,
					ShipmentId = x.Shipment.Id})
				.FirstOrDefaultAsync();

			if (orderItemFact == null)
				return false;

			context.Remove(new Customer { Id = orderItemFact.CustomerId });
			context.Remove(new PaymentMethod { Id = orderItemFact.PaymentMethodId });
			context.Remove(new Order { Id = orderItemFact.OrderId });
			context.Remove(new Product { Id = orderItemFact.ProductId });
			context.Remove(new Shipment { Id = orderItemFact.ShipmentId });

			return true;
		}

		public async Task<IEnumerable<object>> GetGroupedByDetails() {

			return await base.Get().GroupBy(x => x.Details)
				.Select(x => new {
					Details = x.Key,
					Count = x.Count()
				}).ToListAsync();

			//return await base.Get()
			//	.GroupBy(x => x.Details,
			//	(x, y) => new { 
			//		Details = x, 
			//		Count = y.Count() 
			//}).ToListAsync();
		}

	}
}
