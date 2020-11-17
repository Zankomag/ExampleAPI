using System.ComponentModel.DataAnnotations;

namespace ExampleAPI.Data.Models {
	public class PaymentMethod {

		public int Id { get; set; }
		public int PaymenthMethodId { get; set; }
		[Required] public string Description { get; set; }
		public string CardNumber { get; set; }
		public string Details { get; set; }

		public virtual Invoice Invoice { get; set; }
	}
}
