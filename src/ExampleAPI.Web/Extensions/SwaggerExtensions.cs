using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

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

				if (type.GetGenericTypeDefinition() == typeof(IEnumerable<>)
					|| type.GetGenericTypeDefinition() == typeof(IList<>)
					|| type.GetGenericTypeDefinition() == typeof(List<>)) {
					return new StringBuilder(genericArgumentIds[0]).Append("List").ToString();
				}

				if(type.GetGenericTypeDefinition() == typeof(Communication.Response<>)) {
					return new StringBuilder(genericArgumentIds[0]).Append("Response").ToString();
				}

				return sb.Append(string.Format("<{0}>", string.Join(",", genericArgumentIds))).ToString();
			}

			
			return typeName.Replace("Resource", string.Empty);
		}
	}
}
