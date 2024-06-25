using Microsoft.EntityFrameworkCore;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Interfaces;
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
    }
}
