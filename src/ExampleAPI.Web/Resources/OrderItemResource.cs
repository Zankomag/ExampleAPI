using System;
using System.ComponentModel.DataAnnotations;

namespace ExampleAPI.Web.Resources {
	public class OrderItemResource {
		public int Id { get; set; }
		public int Quantity { get; set; }
		public int ItemPrice { get; set; }
		public string Details { get; set; }

		public CustomerResource Customer { get; set; }
		public InvoiceResource Invoice { get; set; }
		public ProductResource Product { get; set; }
		public ShipmentResource Shipment { get; set; }
		public OrderResource Order { get; set; }
	}

	public class InvoiceResource {
		public int Id { get; set; }
		public int InvoiceId { get; set; }
		public DateTime Date { get; set; }
		public int Amount { get; set; }
		public string Details { get; set; }
		[Required] public string Status { get; set; }

		public PaymentMethodResource PaymentMethod { get; set; }
	}

	public class PaymentMethodResource {
		public int Id { get; set; }
		public int PaymenthMethodId { get; set; }
		[Required] public string Description { get; set; }
		public string CardNumber { get; set; }
		public string Details { get; set; }
	}

	public class CustomerResource {
		public int Id { get; set; }
		public int CustomerId { get; set; }
		[Required] public string FirstName { get; set; }
		[Required] public string LastName { get; set; }
		public string Pathronymic { get; set; }
		[Required] public string Email { get; set; }
		[Required] public string Login { get; set; }
		[Required] public string Password { get; set; }
		[Required] public string PhoneNumber { get; set; }
	}

	public class ProductResource {
		public int Id { get; set; }
		public int ProductId { get; set; }
		[Required] public string ProductName { get; set; }
		public int Price { get; set; }
		public string Color { get; set; }
		public int Length { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		[Required] public string Description { get; set; }
		[Required] public string ProductType { get; set; }
	}

	public class ShipmentResource {
		public int Id { get; set; }
		public int ShipmentId { get; set; }
		[Required] public string TrackingNumber { get; set; }
		public DateTime Date { get; set; }
		public string Details { get; set; }
		[Required] public string Country { get; set; }
		[Required] public string Address { get; set; }
	}

	public class OrderResource {
		public int Id { get; set; }
		public int OrderId { get; set; }
		[Required] public string Status { get; set; }
		public DateTime OrderDateTime { get; set; }
		public string Details { get; set; }
	}
}
