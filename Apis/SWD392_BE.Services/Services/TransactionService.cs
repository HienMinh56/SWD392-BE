using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Services
{
    public class TransactionService
    {
        private readonly GenericRepository<Transaction> _transactionRepository;
        public async Task<Transaction> GetTransactionList()
        {
            return await _transactionRepository.Get()
                .Include(x => x.User)
                .Include(x => x.Order)
                .Select(x => new Transaction
                {
                    TransactionId = x.TransactionId,
                    UserId = x.UserId,
                    OrderId = x.OrderId,
                    Amount = x.Amount,
                    TransactionType = x.TransactionType,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    User = new User
                    {
                        UserId = x.User.UserId,
                        Name = x.User.Name,
                        UserName = x.User.UserName,
                        Password = x.User.Password,
                        Email = x.User.Email,
                        CampusId = x.User.CampusId,
                        Phone = x.User.Phone,
                        Role = x.User.Role,
                        Status = x.User.Status,
                        Balance = x.User.Balance,
                        CreatedDate = x.User.CreatedDate,
                        CreatedBy = x.User.CreatedBy,
                    },
                    Order = new Order
                    {
                        OrderId = x.Order.OrderId,
                        UserId = x.Order.UserId,
                        CampusId = x.Order.CampusId,
                        Total = x.Order.Total,
                        Status = x.Order.Status,
                        CreatedDate = x.Order.CreatedDate,
                        CreatedBy = x.Order.CreatedBy,
                    }
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
