using SWD392_BE.Repositories.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.Interfaces
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        IEnumerable<Transaction> GetRecentTransactions();
        Transaction GetLatestTransaction();
        Transaction GetById(int id);
        void Update(Transaction transaction);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
