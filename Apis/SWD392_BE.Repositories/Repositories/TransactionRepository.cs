using Microsoft.EntityFrameworkCore;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.ViewModels.TransactionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly CampusFoodSystemContext _context;
        private readonly IOrderRepository _orderRepository;

        public TransactionRepository(CampusFoodSystemContext context, IOrderRepository orderRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
        }

        public async Task<List<TransactionUserViewModel>?> GetTransaction(string? username = null, DateTime? createdDate = null)
        {
            var query = _context.Transactions.AsQueryable();

            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(x => x.User.UserName.ToLower().Contains(username.ToLower()));
            }

            if (createdDate.HasValue)
            {
                var date = createdDate.Value.Date;
                query = query.Where(x => x.CreatedDate == date);
            }

            return await query
                .Include(x => x.User)
                .Select(x => new TransactionUserViewModel
                {
                    TransationId = x.TransationId,
                    UserId = x.UserId,
                    Amonut = x.Amonut,
                    Type = x.Type,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    Status = x.Status,
                    User = new UserViewModel
                    {
                        UserId = x.User.UserId,
                        Name = x.User.Name,
                        UserName = x.User.UserName,
                    }
                })
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }
    }
}