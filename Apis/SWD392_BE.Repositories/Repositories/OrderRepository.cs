using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
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
        private readonly CampusFoodSystemContext _dbContext;
        public OrderRepository(CampusFoodSystemContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Order> GetOrders()
        {
            return _dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.Store)
                .AsQueryable();
        }

        public async Task<List<Order>> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _dbContext.Orders
                                 .Where(o => o.CreatedDate >= startDate && o.CreatedDate <= endDate)
                                 .ToListAsync();
        }
        public async Task<Order> AddOrder(Order order)
        {
            var result = await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Order> CreateOrder(List<string> orderDetailIds)
        {
            var currentTime = DateTime.Now.TimeOfDay;

            // Find the session that matches the current time
            var session = await _dbContext.Sessions
                .Where(s => currentTime >= s.StartTime && currentTime <= s.EndTime)
                .FirstOrDefaultAsync();

            var orderDetails = await _dbContext.OrderDetails
                   .Where(od => orderDetailIds.Contains(od.OrderDetailId))
                   .Include(od => od.Food)
                   .ToListAsync();

            var sessionId = session.SessionId;
            var storeId = orderDetails.FirstOrDefault().Food.StoreId;
            var order = new Order
            {
                SessionId = sessionId,
                StoreId = storeId,
                CreatedDate = DateTime.Now,
                Status = 1,
                Id = 4,
                OrderId = "ORDER002",
                TransationId = null
            };

            int totalPrice = 0;
            int totalQuantity = 0;

            foreach (var orderDetail in orderDetails)
            {
                totalPrice += orderDetail.Price;
                totalQuantity += orderDetail.Quantity;

                // Add the order detail to the order
                order.OrderDetails.Add(orderDetail);
            }

            order.Price = totalPrice;
            order.Quantity = totalQuantity;

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return order;
        }


        private async Task<int> GetFoodPrice(string foodId)
        {
            var food = await _dbContext.Foods.FindAsync(foodId);
            return food?.Price ?? 0;
        }

    }
}

