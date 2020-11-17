using ExampleAPI.Data.Models;
using ExampleAPI.Web.Communication;
using System.Collections.Generic;
using AutoMapper;

namespace ExampleAPI.Web.Resources.Extensions {
	public static class ResourcesExtensions {
		public static Response<IEnumerable<OrderItemResource>> AsResource(this Response<IEnumerable<OrderItem>> source, IMapper mapper)
			=> source.Convert<IEnumerable<OrderItemResource>>(mapper);

		public static Response<OrderItemResource> AsResource(this Response<OrderItem> source, IMapper mapper)
			=> source.Convert<OrderItemResource>(mapper);

		public static OrderItem AsModel(this OrderItemInputResource source, IMapper mapper)
			=> mapper.Map<OrderItem>(source);

	}
}
