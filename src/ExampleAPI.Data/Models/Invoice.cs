using System;
using System.ComponentModel.DataAnnotations;

namespace ExampleAPI.Data.Models {
	public class Invoice {
		public int Id { get; set; }
		public int InvoiceId { get; set; }
		public DateTime Date { get; set; }
		public int Amount { get; set; }
		public string Details { get; set; }
		[Required] public string Status { get; set; }

		public virtual PaymentMethod PaymentMethod { get; set; }
		public virtual OrderItem OrderItemFact { get; set; }
	}
}
