using ExampleAPI.Web.Communication;
using ExampleAPI.Web.Extensions;
using ExampleAPI.Web.Resources;
using System;
using System.Collections.Generic;
using Xunit;

namespace ExampleAPI.Web.Tests {
	public class SwaggerExtensionsTests {

		[Theory]
		[InlineData(typeof(object), "Object")]
		[InlineData(typeof(OrderItemResource), "OrderItem")]
		[InlineData(typeof(OrderItemInputResource), "OrderItemInput")]
		[InlineData(typeof(OkResponse), "OkResponse")]
		[InlineData(typeof(Response<object>), "ObjectResponse")]
		[InlineData(typeof(Response<OrderItemResource>), "OrderItemResponse")]
		[InlineData(typeof(Response<IEnumerable<OrderItemResource>>), "OrderItemListResponse")]
		[InlineData(typeof(IEnumerable<object>), "ObjectList")]
		[InlineData(typeof(IList<object>), "ObjectList")]
		[InlineData(typeof(IDictionary<int, string>), "IDictionary<Int32,String>")]
		[InlineData(typeof(List<object>), "ObjectList")]
		public void FullTypeName_ShouldSucceed(Type type, string expected) {
			string actual = type.FullTypeName();
			Assert.Equal(expected, actual);
		}

	}
}
