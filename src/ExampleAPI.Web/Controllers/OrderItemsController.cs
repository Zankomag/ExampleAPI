using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ExampleAPI.Web.Resources;
using ExampleAPI.Web.Services.Abstractions;
using ExampleAPI.Web.Communication;
using System.Collections.Generic;
using ExampleAPI.Web.Resources.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace ExampleAPI.Web.Controllers {

	[ApiController]
	[Route("api/[controller]")]
	[Authorize(policy: "IsSub")]
	[Produces("application/json")]
	[Consumes("application/json")]
	[ProducesResponseType(typeof(Response<object>), 400)]
	[ProducesResponseType(typeof(Response<object>), 401)]
	[ProducesResponseType(typeof(Response<object>), 500)]
	public class OrderItemsController : ControllerBase {

		private readonly IOrderItemService service;
		private readonly IMapper mapper;

		public OrderItemsController(IOrderItemService orderItemService, IMapper mapper) {
			this.service = orderItemService;
			this.mapper = mapper;
		}


		//GET: api/OrderItems
		[HttpGet]
		[Produces(typeof(Response<IEnumerable<OrderItemResource>>))]
		public async Task<ObjectResult> GetAll() {
			var response = await service.GetAllAsync();
			return response.AsResource(mapper);
		}


		//GET: api/OrderItems/NoDetails
		[HttpGet("NoDetails")]
		[ProducesResponseType(typeof(Response<IEnumerable<OrderItemResource>>), 200)]
		public async Task<ObjectResult> GetAllWithoutDetails() {
			var response = await service.GetAllWithoutDetailsAsync();
			return response.AsResource(mapper);
		}

		//GET: api/OrderItems/47
		[HttpGet("{id}")]
		[Produces(typeof(Response<OrderItemResource>))]
		public async Task<ObjectResult> GetById(int id) {
			var response = await service.GetByIdAsync(id);
			return response.AsResource(mapper);
			
		}

		//POST: api/OrderItems
		[HttpPost]
		[Produces(typeof(Response<OrderItemResource>))]
		public async Task<ObjectResult> Post([FromBody] OrderItemInputResource inputOrderItem) {
			var response = await service.AddAsync(inputOrderItem);
			return response.AsResource(mapper);
		}

		//PUT: api/OrderItems/47
		[HttpPut("{id}")]
		[Produces(typeof(Response<OrderItemResource>))]
		public async Task<ObjectResult> Update(int id, [FromBody] OrderItemInputResource inputOrderItem) {
			var result = await service.UpdateAsync(id, inputOrderItem);
			return result.AsResource(mapper);
		}

		//DELETE: api/OrderItems/47
		[HttpDelete("{id}")]
		[Produces(typeof(OkResponse))]
		public async Task<ObjectResult> Delete(int id) {
			var response = await service.DeleteAsync(id);
			return response;
		}

		[HttpGet("GroupedByDetails")]
		public async Task<Response<IEnumerable<object>>> GetGroupedByDescription() {
			return await service.GetGroupedByDescriptionAsync();
			
		}

	}
}
