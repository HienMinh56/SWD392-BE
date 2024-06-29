﻿using SWD392_BE.Repositories.ViewModels.OrderModel;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ResultModel> getOrders(string? userId, string? userName, DateTime? createdDate,
                                    int? status, string? storeName, string? sessionId);
        Task<ResultModel> getTotalOrderAmount(DateTime startDate, DateTime endDate);
        Task<ResultModel> getOrderAmountPerDayInMonth(int year, int month);
        Task<ResultModel> getOrderAmountPerWeekInMonth(int year, int month);
        Task<ResultModel> getOrderAmountPerMonthInYear(int year);

        Task<ResultModel> CreateOrderAsync(List<string> orderDetailIds);

    }
}
