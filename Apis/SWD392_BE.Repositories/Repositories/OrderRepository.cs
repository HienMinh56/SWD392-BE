using Microsoft.EntityFrameworkCore;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly CampusFoodSystemContext _context;

        public OrderRepository(CampusFoodSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ResultModel> GetOrderByUserIdAsync(string userId)
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.UserId == userId)
                    .Include(o => o.User)
                    .Include(o => o.Store)
                    .ToListAsync();

                if (orders == null || !orders.Any())
                {
                    return new ResultModel
                    {
                        IsSuccess = false,
                        Code = 404,
                        Message = "No orders found for the given user ID."
                    };
                }

                return new ResultModel
                {
                    IsSuccess = true,
                    Code = 200,
                    Data = orders
                };
            }
            catch (Exception ex)
            {
                return new ResultModel
                {
                    IsSuccess = false,
                    Code = 500,
                    Message = $"An error occurred while retrieving orders: {ex.Message}"
                };
            }
        }
    }
}

