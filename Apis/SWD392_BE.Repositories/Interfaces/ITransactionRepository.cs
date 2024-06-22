using SWD392_BE.Repositories.ViewModels.TransactionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SWD392_BE.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<TransactionUserViewModel>?> GetTransaction(string? username = null, DateTime? createdDate = null);
    }
}
