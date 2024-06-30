using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        IQueryable<Order> GetOrders();
        Task<List<Order>> GetOrdersByDateRange(DateTime startDate, DateTime endDate);
        Task<Order> CreateOrder(List<(string foodId, int quantity)> foodItems, string userId, string userName);
    }
}