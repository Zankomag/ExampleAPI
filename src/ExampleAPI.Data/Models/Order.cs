using System;
using System.ComponentModel.DataAnnotations;

namespace ExampleAPI.Data.Models {
	public class Order {
		public int Id { get; set; }
		public int OrderId { get; set; }
		[Required] public string Status { get; set; }
		public DateTime OrderDateTime { get; set; }
		public string Details { get; set; }

		public virtual OrderItem OrderItemFact { get; set; } 
	}
}
