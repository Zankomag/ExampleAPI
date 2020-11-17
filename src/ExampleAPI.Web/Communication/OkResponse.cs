
namespace ExampleAPI.Web.Communication {
	public class OkResponse : Response<bool>{
		public OkResponse(string description) : base(description) { }
		public OkResponse() : base(true) { }
		public OkResponse(int? errorCode = 500, string description = null) : base(errorCode, description) { }

		public static implicit operator OkResponse(string description) => new OkResponse(description);

		public static implicit operator OkResponse(System.Net.HttpStatusCode errorCode) => new  OkResponse((int)errorCode);

	}
}
