using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.ViewModels.TransactionModel;
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
        Task<List<TransactionUserViewModel>?> GetTransaction(string? username = null, DateTime? createdDate = null);
    }
}
