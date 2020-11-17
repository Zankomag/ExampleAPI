using ExampleAPI.Data.Models;
using ExampleAPI.Web.Resources;
using AutoMapper;

namespace ExampleAPI.Web.Mapping {
	public class InputResourceToModelProfile : Profile {
		public InputResourceToModelProfile() {

			CreateMap<OrderItemInputResource, OrderItem>();
			CreateMap<InvoiceInputResource, Invoice>();
			CreateMap<PaymentMethodInputResource, PaymentMethod>();
			CreateMap<CustomerInputResource, Customer>();
			CreateMap<OrderInputResource, Order>();
			CreateMap<ProductInputResource, Product>();
			CreateMap<ShipmentInputResource, Shipment>();
		}
	}
}
