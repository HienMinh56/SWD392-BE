using Microsoft.EntityFrameworkCore;
﻿using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.Repositories;
using SWD392_BE.Repositories.ViewModels.OrderModel;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.UserModel;
using SWD392_BE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        public async Task<ResultModel> getOrders(string? userId, DateTime? createdDate, int? status, string? storeName, string? sessionId)
        {
            var result = new ResultModel();
            try
            {
                var orders = await _order.GetOrders();

                if (!string.IsNullOrEmpty(userId))
                {
                    orders = orders.Where(o => o.UserId.ToLower() == userId).ToList();
                }

                if (createdDate.HasValue)
                {
                    orders = orders.Where(o => o.CreatedDate == createdDate.Value).ToList();
                }

                if (status.HasValue)
                {
                    orders = orders.Where(o => o.Status == status.Value).ToList();
                }

                if (!string.IsNullOrEmpty(storeName))
                {
                    orders = orders.Where(o => o.Store.Name.ToLower() == storeName.ToLower()).ToList();
                }

                if (!string.IsNullOrEmpty(sessionId))
                {
                    orders = orders.Where(o => o.SessionId == sessionId).ToList();
                }

                if (!orders.Any())
                {
                    result.Message = "Data not found";
                    result.IsSuccess = false;
                    result.Code = 404;
                }
                else
                {
                    var orderViewModels = orders.Select(o => new OrderListViewModel
                    {
                        OrderId = o.OrderId,
                        Name = o.User.Name,
                        SessionId = o.SessionId,
                        Price = o.Price,
                        Quantity = o.Quantity,
                        StoreName = o.Store.Name,
                        TransationId = o.TransationId,
                        Status = o.Status,
                        CreatedTime = o.CreatedTime,
                        CreatedDate = o.CreatedDate,
                    }).ToList();

                    result.Data = orderViewModels;
                    result.Message = "Success";
                    result.IsSuccess = true;
                    result.Code = 200;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
            }
            return result;
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
