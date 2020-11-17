using System.ComponentModel.DataAnnotations;

namespace ExampleAPI.Data.Models {
	public class OrderItem {

		public int Id { get; set; }
		public int Quantity { get; set; }
		public int ItemPrice { get; set; }
		public string Details { get; set; }


		public virtual Invoice Invoice { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual Product Product {get; set;}
		public virtual Shipment Shipment { get; set; }
		public virtual Order Order { get; set; }


	}
}
