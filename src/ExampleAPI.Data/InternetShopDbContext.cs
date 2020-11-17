using ExampleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ExampleAPI.Data {
	public class InternetShopDbContext : DbContext {

		public InternetShopDbContext(DbContextOptions<InternetShopDbContext> options) : base(options) { }

		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Invoice> Invoices { get; set; }
		public DbSet<PaymentMethod> PaymentMethods { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Shipment> Shipments { get; set; }
		public DbSet<Order> Orders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			//Configure Shadow properties
			modelBuilder.Entity<Invoice>().Property<int>("PaymentMethodId");
			modelBuilder.Entity<OrderItem>().Property<int>("CustomerId");
			modelBuilder.Entity<OrderItem>().Property<int>("InvoiceId");
			modelBuilder.Entity<OrderItem>().Property<int>("OrderId");
			modelBuilder.Entity<OrderItem>().Property<int>("ProductId");
			modelBuilder.Entity<OrderItem>().Property<int>("ShipmentId");
		}

	}
}
