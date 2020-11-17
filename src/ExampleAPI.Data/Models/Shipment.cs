using System;
using System.ComponentModel.DataAnnotations;


namespace ExampleAPI.Data.Models {
	public class Shipment {
		public int Id { get; set; }
		public int ShipmentId { get; set; }
		[Required] public string TrackingNumber { get; set; }
		public DateTime Date { get; set; }
		public string Details { get; set; }
		[Required] public string Country { get; set; }
		[Required] public string Address { get; set; }

		public virtual OrderItem OrderItemFact { get; set; }
	}
}
