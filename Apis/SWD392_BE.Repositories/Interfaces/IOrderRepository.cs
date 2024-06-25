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
        Task<List<Order>> GetOrders();


        Task<List<Order>> GetOrdersByTransactionId(string transactionId);
        IQueryable<Order> GetOrders();
    }
}
