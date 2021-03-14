using ExampleAPI.Data.Models;
using ExampleAPI.Web.Communication;
using ExampleAPI.Web.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExampleAPI.Web.Services.Abstractions {
	public interface IOrderItemService {

		//TODO Converting to response should be on UI layer itself not BLL!!!
		Task<Response<OrderItem>> GetByIdAsync(int id);
		Task<Response<IEnumerable<OrderItem>>> GetAllAsync();
		Task<Response<IEnumerable<OrderItem>>> GetAllWithoutDetailsAsync();
		Task<Response<OrderItem>> AddAsync(OrderItemInputResource inputOrderItem);
		Task<Response<OrderItem>> UpdateAsync(int id, OrderItemInputResource inputOrderItem);
		Task<OkResponse> DeleteAsync(int id);
		Task<Response<IEnumerable<object>>> GetGroupedByDescriptionAsync();

	}
}
