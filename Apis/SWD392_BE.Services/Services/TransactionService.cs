﻿using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.Repositories;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepo;

        public TransactionService(ITransactionRepository transactionRepo)
        {
            _transactionRepo = transactionRepo;
        }
        public async Task<ResultModel> GetTransactionList(string? userId = null, DateTime? createdDate = null)
        {
            ResultModel result = new ResultModel();

            try
            {
                var transactions = await _transactionRepo.GetTransaction(userId, createdDate);
                if (transactions == null || transactions.Count == 0)
                {
                    result.IsSuccess = false;
                    result.Code = 404;
                    result.Data = null;
                    result.Message = "No transactions found.";
                }
                else
                {
                    result.IsSuccess = true;
                    result.Code = 200;
                    result.Data = transactions;
                    result.Message = "Transactions retrieved successfully.";
                };
            }
            catch (Exception ex)
            {
                {
                    result.IsSuccess = false;
                    result.Code = 500;
                    result.Data = null;
                    result.Message = $"An error occurred: {ex.Message}";
                };
            }
            return result;
        }

        public async Task<string> CheckLatestTransactionStatusByUserIdAndType(string userId, int type)
        {
            var transactions = await _transactionRepo.GetAllTransactionsAsync();
            var latestTransaction = transactions?
                .Where(t => t.UserId == userId && t.Type == type)
                .OrderByDescending(t => t.Id)
                .FirstOrDefault();

            if (latestTransaction != null)
            {
                if (latestTransaction.Status == 1)
                {
                    return "Thành công";
                }
                else if (latestTransaction.Status == 2)
                {
                    return "Thất bại";
                }
            }

            return "Không tìm thấy giao dịch";
        }
    }
}
