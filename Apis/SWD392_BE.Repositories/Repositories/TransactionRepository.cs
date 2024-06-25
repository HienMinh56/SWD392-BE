using Microsoft.EntityFrameworkCore;
using SWD392_BE.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.Repositories
{
    public class TransactionRepository
    {
        private readonly CampusFoodSystemContext _context;

        public TransactionRepository(CampusFoodSystemContext context)
        {
            _context = context;
        }

        public async Task<Transaction> GetTransaction()
        {
            return _context.Transactions
                .Include(x => x.User)
                .Include(x => x.Orders)
                .Select(x => new Transaction
                {
                    TransationId = x.TransationId,
                    UserId = x.UserId,
                    Amonut = x.Amonut,
                    Type = x.Type,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    User = new User
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