using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ExampleAPI.Web.Communication {

	public class Response<TResult> {
		public bool Success { get; private set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public int? ErrorCode { get; private set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string Description { get; private set; } = null;

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public TResult Result { get; private set; } = default;

		public Response(TResult result) {
			Success = true;
			Result = result;
		}

		/// <summary>
		/// Response for 400 Bad Request error.
		/// </summary>
		public Response(string description){
			Success = false;
			Description = $"Bad Request: {description}";
			ErrorCode = 400;
		}

		public Response(int? errorCode = 500, string description = null) {
			Success = false;
			Description = errorCode == 500 ? description ?? "Internal Server Error" : description;
			ErrorCode = errorCode;
		}

		public Response<TDestinationResult> Convert<TDestinationResult>(IMapper mapper) {
			if (Success) {
				return new Response<TDestinationResult>(mapper.Map<TDestinationResult>(Result));
			}
			return new Response<TDestinationResult>(ErrorCode, Description);
		}


		public static implicit operator Response<TResult>(string description) => new Response<TResult>(description);

		public static implicit operator Response<TResult>(HttpStatusCode errorCode) => new Response<TResult>((int)errorCode);

		public static implicit operator Response<TResult>(TResult result) => new Response<TResult>(result);

		public static implicit operator ObjectResult(Response<TResult> response) 
			=> new ObjectResult(response) { StatusCode = response.ErrorCode };

	}

}
