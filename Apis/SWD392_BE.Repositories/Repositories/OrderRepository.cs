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
    }
}

