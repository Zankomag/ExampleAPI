
namespace ExampleAPI.Web.Communication.Extensions {
	public static class ResponseExtensions {

		//Due to impossibility of using interface (e.g. IEnumerable<object>) as implicit operator parameter 
		//extension method that duplicates implicit operator has to be created
		public static Response<TResult> AsResponse<TResult>(this TResult result) => new Response<TResult>(result);

	}
}
