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

        public async Task<List<Order>> GetOrders()
        {
            return await _dbContext.Orders
                .Include(x => x.Store)
                .Include(x => x.User)
                .Select(x => new Order
                {
                    OrderId = x.OrderId,
                    UserId = x.UserId,
                    User = new User
                    {
                        Name = x.User.Name,
                    },
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Store = new Store
                    {
                        Name = x.Store.Name,
                    },
                    Status = x.Status,
                    CreatedTime = x.CreatedTime,
                    CreatedDate = x.CreatedDate,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedDate = x.ModifiedDate,
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }
}

