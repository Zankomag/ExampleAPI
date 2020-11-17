using ExampleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ExampleAPI.Repository.Extensions {
	public static class InternetShopRepositoryExtensions {
		public static IQueryable<OrderItem> IncludeAll(this IQueryable<OrderItem> dbset) {
			return dbset.Include(o => o.Customer)
				.Include(o => o.Invoice.PaymentMethod)
				.Include(o => o.Order)
				.Include(o => o.Product)
				.Include(o => o.Shipment);
		}

	}
}
