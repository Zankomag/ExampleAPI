using ExampleAPI.Web.Communication;
using ExampleAPI.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace ExampleAPI.Web.Tests {
	public class ResponseTests {

		public static IEnumerable<object[]> TResultData() {
			yield return new object[] { 45 };
			yield return new object[] { "string" };
			yield return new object[] { true };
			yield return new object[] { false };
			yield return new object[] { new List<OrderItemInputResource>() };
			yield return new object[] { new List<OrderItemResource>() { new OrderItemResource(), new OrderItemResource() } };
			yield return new object[] { new List<List<OrderItemResource>>() };
			yield return new object[] { new OrderItemResource() };
		}

		public static IEnumerable<object[]> ResponseData() {
			yield return new object[] { new Response<OrderItemResource>() };
			yield return new object[] { new Response<OrderItemResource>(new OrderItemResource()) };
			yield return new object[] { new Response<string>(result: "hhet") };
			yield return new object[] { new Response<bool>(false) };
			yield return new object[] { new Response<OrderItemResource>("Not Found") };
			yield return new object[] { new Response<OrderItemResource>(404, "Noy Found") };
			yield return new object[] { new Response<string>(null, "Error") };
			yield return new object[] { new Response<object>(new object()) };
			yield return new object[] { new Response<object>() };
		}

		public static IEnumerable<object[]> OrderItemResponseData() {
			yield return new object[] { new Response<OrderItemResource>() };
			yield return new object[] { new Response<OrderItemResource>(new OrderItemResource()) };
			yield return new object[] { new Response<OrderItemResource>("Not Found") };
			yield return new object[] { new Response<OrderItemResource>(404, "Not Found") };
		}

		[Theory]
		[MemberData(nameof(TResultData))]
		public void Constructor_ShouldSucceed_When_TResult_Not_Null(object result) {
			var response = new Response<object>(result);
			Assert.True(response.Success);
			Assert.Null(response.Description);
			Assert.Null(response.ErrorCode);
			Assert.Equal(result, response.Result);
		}

		[Fact]
		public void Constructor_ShouldThrow_When_TResult_Is_Null() {
			Assert.Throws<ArgumentNullException>("result",
				() => new Response<object>(result: null));
		}

		[Theory]
		[MemberData(nameof(ResponseData))]
		public void Constructor_ShouldThrow_When_TResult_Is_Another_Response(object otherResponse) {
			Assert.Throws<ArgumentException>("result", () => new Response<object>(otherResponse));
		}

		[Fact]
		public void Constructor_Should_Call_Description_Constructor_And_Succeed_When_Passing_Null() {
			var response = new Response<object>(null);
			Assert.False(response.Success);
			Assert.Null(response.Result);
			Assert.Equal(400, response.ErrorCode);
			Assert.Equal("Bad Request", response.Description);
		}

		[Fact]
		public void Constructor_ShouldSucceed_When_Description_Is_Good() {
			string description = "Not found";
			var response = new Response<object>(description);
			Assert.False(response.Success);
			Assert.Null(response.Result);
			Assert.Equal(400, response.ErrorCode);
			Assert.Equal($"Bad Request: {description}", response.Description);
		}

		[Theory]
		[InlineData("")]
		[InlineData("     ")]
		[InlineData(null)]
		public void Constructor_ShouldSucceed_When_Description_Is_Null_Or_WhiteSpace_Or_Empty(string description) {
			var response = new Response<object>(description);
			Assert.False(response.Success);
			Assert.Null(response.Result);
			Assert.Equal(400, response.ErrorCode);
			Assert.Equal("Bad Request", response.Description);
		}

		[Fact]
		public void Constructor_ShouldSucceed_WhenEmpty() {
			var response = new Response<object>();
			Assert.False(response.Success);
			Assert.Null(response.Result);
			Assert.Equal(500, response.ErrorCode);
			Assert.Equal("Internal Server Error", response.Description);
		}

		[Theory]
		[InlineData(100)]
		[InlineData(200)]
		[InlineData(500)]
		[InlineData(int.MaxValue)]
		[InlineData(0)]
		public void Constructor_ShouldSucceed_When_Description_Is_Good_And_ErrorCode_Not_Null(int? errorCode) {
			var response = new Response<object>(errorCode, "Error");
			Assert.False(response.Success);
			Assert.Null(response.Result);
			Assert.Equal(errorCode, response.ErrorCode);
			Assert.Equal("Error", response.Description);
		}

		[Theory]
		[InlineData(100, "")]
		[InlineData(200, "    ")]
		[InlineData(int.MaxValue, null)]
		public void Constructor_ShouldSucceed_When_Description_Is_Null_Or_WhiteSpace_Or_Empty_And_ErrorCode_Not_Null(int? errorCode, string description) {
			var response = new Response<object>(errorCode, description);
			Assert.False(response.Success);
			Assert.Null(response.Result);
			Assert.Equal(errorCode, response.ErrorCode);
			Assert.Null(response.Description);
		}

		[Fact]
		public void Constructor_ShouldSucceed_When_Description_Is_Good_And_ErrorCode_Is_Null() {
			var response = new Response<object>(null, "Error");
			Assert.False(response.Success);
			Assert.Null(response.Result);
			Assert.Equal(500, response.ErrorCode);
			Assert.Equal("Error", response.Description);
		}

		[Fact]
		public void Constructor_ShouldSucceed_When_Description_Not_Specified_And_ErrorCode_Not_Null() {
			var response = new Response<object>(505);
			Assert.False(response.Success);
			Assert.Null(response.Result);
			Assert.Equal(505, response.ErrorCode);
			Assert.Null(response.Description);
		}

		[Fact]
		public void Constructor_ShouldSucceed_When_Description_Is_Null_And_ErrorCode_Is_Null() {
			var response = new Response<object>(null, null);
			Assert.False(response.Success);
			Assert.Null(response.Result);
			Assert.Equal(500, response.ErrorCode);
			Assert.Equal("Internal Server Error", response.Description);
		}

		[Theory]
		[InlineData("")]
		[InlineData("    ")]
		[InlineData(null)]
		public void Implicit_String_ShouldSucceed_When_Is_Null_Or_WhiteSpace_Or_Empty(string description) {
			Response<object> response = description;
			Assert.False(response.Success);
			Assert.Null(response.Result);
			Assert.Equal(400, response.ErrorCode);
			Assert.Equal("Bad Request", response.Description);
		}

		[Fact]
		public void Implicit_String_ShouldSucceed_When_Is_Good() {
			Response<object> response = "Not Found";
			Assert.False(response.Success);
			Assert.Null(response.Result);
			Assert.Equal(400, response.ErrorCode);
			Assert.Equal($"Bad Request: Not Found", response.Description);
		}

		[Theory]
		[InlineData(HttpStatusCode.OK)]
		[InlineData(HttpStatusCode.BadGateway)]
		[InlineData(HttpStatusCode.NotFound)]
		public void Implicit_HttpStatusCode_ShouldSucceed_When_Not_500(HttpStatusCode httpStatusCode) {
			Response<object> response = httpStatusCode;
			Assert.False(response.Success);
			Assert.Null(response.Result);
			Assert.Equal((int)httpStatusCode, response.ErrorCode);
			Assert.Null(response.Description);
		}

		[Fact]
		public void Implicit_HttpStatusCode_ShouldSucceed_When_Is_500() {
			Response<object> response = HttpStatusCode.InternalServerError;
			Assert.False(response.Success);
			Assert.Null(response.Result);
			Assert.Equal(500, response.ErrorCode);
			Assert.Equal("Internal Server Error", response.Description);
		}

		[Theory]
		[MemberData(nameof(TResultData))]
		public void Implicit_TResult_ShouldSucceed(object result) {
			Response<object> response = result;
			Assert.True(response.Success);
			Assert.Null(response.Description);
			Assert.Null(response.ErrorCode);
			Assert.Equal(result, response.Result);
		}

		[Theory]
		[MemberData(nameof(ResponseData))]
		public void Implicit_TResult_ShouldThrow_When_Is_Another_Response(object otherResponse) {
			Assert.Throws<ArgumentException>("result", () => { Response<object> response = otherResponse; });
		}

		[Theory]
		[MemberData(nameof(OrderItemResponseData))]
		public void Implicit_Response_To_ObjectResult_ShouldSucceed(object responseObj) {
			var response = (responseObj as Response<OrderItemResource>);
			ObjectResult objectResult = response;
			Assert.Equal(response, (Response<OrderItemResource>)objectResult.Value);
			Assert.Equal(response.ErrorCode, objectResult.StatusCode);
		}
	}
}
