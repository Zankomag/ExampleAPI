using System.ComponentModel.DataAnnotations;

namespace ExampleAPI.Data.Models {

	public class Customer {
		public int Id { get; set; }
		public int CustomerId { get; set; }
		[Required] public string FirstName { get; set; }
		[Required] public string LastName { get; set; }
		public string Pathronymic { get; set; }
		[Required] public string Email { get; set; }
		[Required] public string Login { get; set; }
		[Required] public string Password { get; set; }
		[Required] public string PhoneNumber { get; set; }

		public virtual OrderItem OrderItemFact { get; set; }
	}
}
