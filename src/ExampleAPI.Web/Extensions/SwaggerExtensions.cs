using System;
using System.Linq;
using System.Text;

namespace ExampleAPI.Web.Extensions {
	public static class SwaggerExtensions {
		public static string FullTypeName(this Type type) {

			string typeName = type.Name;

			if (type.IsGenericType) {
				var genericArgumentIds = type.GetGenericArguments()
					.Select(t => t.FullTypeName())
					.ToArray();

				var sb = new StringBuilder(typeName)
					.Replace($"`{genericArgumentIds.Count().ToString()}", string.Empty);

				if(type.GetGenericTypeDefinition() == typeof(System.Collections.Generic.IEnumerable<>)) 
					return new StringBuilder(genericArgumentIds[0]).Append("[]").ToString();

				if(type.GetGenericTypeDefinition() == typeof(Communication.Response<>)) {
					return new StringBuilder(genericArgumentIds[0]).Append(" Response").ToString();
				}

				return sb.Append(string.Format("<{0}>", string.Join(",", genericArgumentIds))).ToString();
			}

			
			return typeName;
		}
	}
}
