using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Math.EC.Rfc7748;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<Order> CreateOrder(List<(string foodId, int quantity)> foodItems, string userId, string userName)
        {
            // Generate new OrderId
            var newOrderId = await GenerateNewOrderIdAsync();

            //New Transaction
            var newTransactionId = await GenerateNewTransactionIdAsync();

            var newTransaction = new Transaction
            {
                TransactionId = newTransactionId,
                UserId = userId,
                Type = 2,
                Status = 2,
                CreatedDate = DateTime.Now,
                CreatTime = DateTime.Now.TimeOfDay
            };
            _dbContext.Transactions.Add(newTransaction);
            await _dbContext.SaveChangesAsync();

            // Find the session when create order
            var session = await GetCurrentSessionAsync();

            var order = new Order
            {
                OrderId = newOrderId,
                UserId = userId,
                SessionId = session.SessionId,
                CreatedDate = DateTime.Now,
                CreatedBy = userName,
                CreatedTime = DateTime.Now.TimeOfDay,
                TransactionId = newTransactionId,
                Status = 3,
            };

            int totalPrice = 0;
            foreach (var item in foodItems)
            {
                var food = await _dbContext.Foods.FirstOrDefaultAsync(f => f.FoodId == item.foodId);
                if (food == null)
                {
                    throw new Exception($"Food with ID {item.foodId} not found.");
                }

                order.StoreId = food.StoreId;

                var orderDetail = new OrderDetail
                {
                    FoodId = item.foodId,
                    Quantity = item.quantity,
                    Price = food.Price * item.quantity,
                    OrderId = order.OrderId,
                    Status = 1,
                    Note = null
                };

                totalPrice += orderDetail.Price;
                order.OrderDetails.Add(orderDetail);
            }

            order.Price = totalPrice;
            order.Quantity = foodItems.Sum(item => item.quantity);

            try
            {
                await _dbContext.Orders.AddAsync(order);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                Console.WriteLine(innerException?.Message);
                throw new Exception($"An error occurred while creating the order: {innerException?.Message}", ex);
            }
            return order;
        }

        private async Task<string> GenerateNewOrderIdAsync()
        {
            var lastOrder = await _dbContext.Orders.OrderByDescending(o => o.OrderId).FirstOrDefaultAsync();
            var lastOrderIdNumber = lastOrder != null ? int.Parse(lastOrder.OrderId[5..]) : 0;
            return $"ORDER{lastOrderIdNumber + 1:D6}";
        }

        private async Task<Session> GetCurrentSessionAsync()
        {
            var currentTime = DateTime.Now.TimeOfDay;
            var session = await _dbContext.Sessions
                 .Where(s => currentTime >= s.StartTime && currentTime <= s.EndTime)
                 .FirstOrDefaultAsync();
            if (session == null)
            {
                throw new Exception("No session available for the current time.");
            }
            return session;
        }

        private async Task<string> GenerateNewTransactionIdAsync()
        {
            var latestTransaction = await _dbContext.Transactions.OrderByDescending(t => t.TransactionId).FirstOrDefaultAsync();
            var numericPart = latestTransaction != null ? int.Parse(latestTransaction.TransactionId.Substring(5)) + 1 : 1;
            return $"TRANS{numericPart:D3}";
        }

    }
}

