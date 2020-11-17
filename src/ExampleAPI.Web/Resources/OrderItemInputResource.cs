using System;
using System.ComponentModel.DataAnnotations;

namespace ExampleAPI.Web.Resources {
	public class OrderItemInputResource {
		public int Quantity { get; set; }
		public int ItemPrice { get; set; }
		public string Details { get; set; }

		public CustomerInputResource Customer { get; set; }
		public InvoiceInputResource Invoice { get; set; }
		public ProductInputResource Product { get; set; }
		public ShipmentInputResource Shipment { get; set; }
		public OrderInputResource Order { get; set; }
	}

	public class InvoiceInputResource {
		public int InvoiceId { get; set; }
		public DateTime Date { get; set; }
		public int Amount { get; set; }
		public string Details { get; set; }
		[Required] public string Status { get; set; }

		public PaymentMethodInputResource PaymentMethod { get; set; }
	}

	public class PaymentMethodInputResource {
		public int PaymenthMethodId { get; set; }
		[Required] public string Description { get; set; }
		public string CardNumber { get; set; }
		public string Details { get; set; }
	}

	public class CustomerInputResource {
		public int CustomerId { get; set; }
		[Required] public string FirstName { get; set; }
		[Required] public string LastName { get; set; }
		public string Pathronymic { get; set; }
		[Required] public string Email { get; set; }
		[Required] public string Login { get; set; }
		[Required] public string Password { get; set; }
		[Required] public string PhoneNumber { get; set; }
	}

	public class ProductInputResource {
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

	public class ShipmentInputResource {
		public int ShipmentId { get; set; }
		[Required] public string TrackingNumber { get; set; }
		public DateTime Date { get; set; }
		public string Details { get; set; }
		[Required] public string Country { get; set; }
		[Required] public string Address { get; set; }
	}

	public class OrderInputResource {
		public int OrderId { get; set; }
		[Required] public string Status { get; set; }
		public DateTime OrderDateTime { get; set; }
		public string Details { get; set; }
	}

}
