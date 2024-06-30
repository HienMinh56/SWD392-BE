using Microsoft.EntityFrameworkCore;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.ViewModels.TransactionModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly CampusFoodSystemContext _context;
        private readonly DbSet<Transaction> _transactions;

        public TransactionRepository(CampusFoodSystemContext context) : base(context)
        {
            _context = context;
            _transactions = context.Transactions;
        }

        public Transaction GetByTransactionId(string transactionId)
        {
            return _context.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);
        }

        public IEnumerable<Transaction> GetRecentTransactions()
        {
            return _transactions.OrderByDescending(t => t.Id).Take(10).ToList();
        }

        public Transaction GetLatestTransaction()
        {
            return _transactions.OrderByDescending(t => t.Id).FirstOrDefault();
        }

        public Transaction GetById(int id)
        {
            return _transactions.Find(id);
        }

        public void Update(Transaction transaction)
        {
            _transactions.Attach(transaction);
            _context.Entry(transaction).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
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
                    TransationId = x.TransactionId,
                    UserId = x.UserId,
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
