using System.ComponentModel.DataAnnotations;

namespace ExampleAPI.Data.Models {
	public class Product {
		public int Id { get; set; }
		public int ProductId { get; set; }
		[Required] public string ProductName { get; set; }
		public int Price { get; set; }
		public string Color { get; set; }
		public int Length { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		[Required] public string Description { get; set; }
		[Required] public string  ProductType { get; set; }

		public virtual OrderItem OrderItemFact { get; set; }
	}
}
