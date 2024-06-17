using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.Repositories;
using SWD392_BE.Repositories.ViewModels.OrderModel;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Services
{
    public class OrderServices : IOrderService
    {
        private readonly IOrderRepository _order;

        public OrderServices(IOrderRepository order)
        {
            _order = order;
        }
        public async Task<ResultModel> getAllOrder()
        {
            var result = new ResultModel();
            try
            {
                var order = _order.Get();
                if (order == null)
                {
                    result.IsSuccess = true;
                    result.Code = 201;
                    result.Message = "No Order Here";
                    return result;
                }

                result.IsSuccess = true;
                result.Code = 200;
                result.Data = order;
                result.Message = "Get Order Success";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.Message = ex.Message;
                return result;
            }
        }
        public async Task<ResultModel> GetOrderByUserIdAsync(string userId)
        {
            var result = await _order.GetOrderByUserIdAsync(userId);

            if (!result.IsSuccess)
            {
                return result;
            }

            var orders = (List<Order>)result.Data;

            var orderDetails = orders.Select(o => new GetOrderViewModel
            {
                OrderId = o.OrderId,
                SessionId = o.SessionId,
                TransationId = o.TransationId,
                UserName = o.User.UserName,
                StoreName = o.Store.Name,
                Price = o.Price,
                Quantity = o.Quantity,
                Status = o.Status,
                CreatedTime = o.CreatedTime,
                CreatedDate = o.CreatedDate,
                CreatedBy = o.CreatedBy,
                ModifiedDate = o.ModifiedDate,
                ModifiedBy = o.ModifiedBy,
                // Add other properties as needed
            }).ToList();

            return new ResultModel
            {
                IsSuccess = true,
                Code = 200,
                Data = orderDetails
            };
        }
    }
}

