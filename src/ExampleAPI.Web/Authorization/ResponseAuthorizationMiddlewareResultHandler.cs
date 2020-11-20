using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ExampleAPI.Web.Communication;
using Newtonsoft.Json;

namespace ExampleAPI.Web.Authorization {
	public class ResponseAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler {

		private static readonly string unauthorizedResponseJson = JsonConvert.SerializeObject(new Response<object>(401, "Unauthorized"));

		public async Task HandleAsync(
			RequestDelegate requestDelegate, HttpContext httpContext,
			AuthorizationPolicy authorizationPolicy, PolicyAuthorizationResult policyAuthorizationResult) {

			if (!policyAuthorizationResult.Succeeded) {
				httpContext.Response.StatusCode = 401;
				httpContext.Response.ContentType = "application/json";
				await httpContext.Response.WriteAsync(unauthorizedResponseJson);
				return;
			}

			await new AuthorizationMiddlewareResultHandler().HandleAsync(requestDelegate, httpContext, authorizationPolicy, policyAuthorizationResult);
		}
	}
}
