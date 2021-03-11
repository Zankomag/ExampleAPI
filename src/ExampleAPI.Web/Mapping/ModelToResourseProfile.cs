using AutoMapper;
using ExampleAPI.Data.Models;
using ExampleAPI.Web.Resources;

namespace ExampleAPI.Web.Mapping {
	public class ModelToResourceProfile : Profile {
		public ModelToResourceProfile() {
			CreateMap<OrderItem, OrderItemResource>().ReverseMap();
			CreateMap<Invoice, InvoiceResource>().ReverseMap();
			CreateMap<PaymentMethod, PaymentMethodResource>().ReverseMap();
			CreateMap<Customer, CustomerResource>().ReverseMap();
			CreateMap<Order, OrderResource>().ReverseMap();
			CreateMap<Product, ProductResource>().ReverseMap();
			CreateMap<Shipment, ShipmentResource>().ReverseMap();
		}
	}
}
