using AutoMapper;
using ExampleAPI.Data;
using ExampleAPI.Data.Models;
using ExampleAPI.Repository;
using ExampleAPI.Repository.Abstractions;
using ExampleAPI.Web.Communication;
using ExampleAPI.Web.Communication.Extensions;
using ExampleAPI.Web.Resources;
using ExampleAPI.Web.Resources.Extensions;
using ExampleAPI.Web.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.HttpStatusCode;

namespace ExampleAPI.Web.Services {
	public class OrderItemService : IOrderItemService {

		private readonly IUnitOfWork workUnit;
		private IMapper mapper;

		public OrderItemService(IUnitOfWork workUnit, IMapper mapper) {
			this.workUnit = workUnit;
			this.mapper = mapper;
		}

		public async Task<Response<IEnumerable<OrderItem>>> GetAllAsync() {
			try {
				var orderItems = await workUnit.OrderItemRepository.GetAsync(includeAllProperties: true);
				return orderItems.AsResponse();
			} catch(Exception ex) {
				//Log exception
				return InternalServerError;
			}
		}

		public async Task<Response<IEnumerable<OrderItem>>> GetAllWithoutDetailsAsync() {
			try {
				var orderItems = await workUnit.OrderItemRepository.GetWithoutDetailsAsync();
				return orderItems.AsResponse();
			} catch(Exception ex) {
				//Log exception
				return InternalServerError;
			}
		}

		public async Task<Response<OrderItem>> GetByIdAsync(int id) {
			try {
				var orderItem = await workUnit.OrderItemRepository.GetByIdAsync(id, true);
				if (orderItem != null)
					return orderItem.AsResponse();
				else
					return "Record not found";

			} catch(Exception ex) {
				//Log exception
				return InternalServerError;
			}
		}

		public async Task<Response<OrderItem>> AddAsync(OrderItemInputResource inputOrderItem) {
			var orderItem = inputOrderItem.AsModel(mapper);
			try {
				await workUnit.OrderItemRepository.AddAsync(orderItem);
				await workUnit.SaveAsync();
				return orderItem;
			} catch(Exception ex) {
				//Log exception
				return InternalServerError;
			}
		}

		public async Task<Response<OrderItem>> UpdateAsync(int id, OrderItemInputResource inputOrderItem) {
			if (id <= 0) return "Record not found";
			var orderItem = await workUnit.OrderItemRepository.GetByIdAsync(id, true);
			if (orderItem == null)
				return "Record not found";
			mapper.Map(inputOrderItem, orderItem);
			if (!workUnit.OrderItemRepository.HasDataChanged)
				return "Data is not modified";

			try {
				await workUnit.SaveAsync();
				return orderItem;
			} catch (DbUpdateConcurrencyException) {
				return "Record not found";
			} catch (Exception ex) {
				//Log exception
				return InternalServerError;
			}
		}

		public async Task<OkResponse> DeleteAsync(int id) {
			if(id <= 0) return "Record not found";

			if(!await workUnit.OrderItemRepository.DeleteAsync(id))
				return "Record not found";

			try {
				await workUnit.SaveAsync();
				return new OkResponse();
			} catch(DbUpdateConcurrencyException) {
				return "Record not found";
			} catch(Exception ex) {
				//Log exception
				return InternalServerError;
			}
		}

		public async Task<Response<IEnumerable<object>>> GetGroupedByDescriptionAsync() {
			try {
				var result = await workUnit.OrderItemRepository.GetGroupedByDetails();
				return result != null ?
					new Response<IEnumerable<object>>(result) :
					"Not found";
			} catch(Exception ex) {
				//Log exception
				return InternalServerError;
			}
		}
	}
}
