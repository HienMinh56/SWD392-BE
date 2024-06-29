using Microsoft.EntityFrameworkCore;
﻿using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.Repositories;
using SWD392_BE.Repositories.ViewModels.OrderModel;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.UserModel;
using SWD392_BE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Services
{
    public class OrderServices : IOrderService
    {
        private readonly IOrderRepository _order;

        public OrderServices(IOrderRepository order)
        {
            _order = order;
        }

        public async Task<ResultModel> getOrders(string? userId, string? userName, DateTime? createdDate, int? status, string? storeName, string? sessionId)
        {
            var result = new ResultModel();
            try
            {
                var orders = _order.GetOrders();

                if (!string.IsNullOrEmpty(userId))
                {
                    orders = orders.Where(o => o.UserId.ToLower() == userId.ToLower());
                }

                if (!string.IsNullOrEmpty(userName))
                {
                    orders = orders.Where(o => o.User.Name.ToLower().Contains(userName.ToLower()));
                }

                if (createdDate.HasValue)
                {
                    orders = orders.Where(o => o.CreatedDate == createdDate.Value);
                }

                if (status.HasValue)
                {
                    orders = orders.Where(o => o.Status == status.Value);
                }

                if (!string.IsNullOrEmpty(storeName))
                {
                    orders = orders.Where(o => o.Store.Name.ToLower() == storeName.ToLower());
                }

                if (!string.IsNullOrEmpty(sessionId))
                {
                    orders = orders.Where(o => o.SessionId == sessionId);
                }

                if (!orders.Any())
                {
                    result.Message = "Data not found";
                    result.IsSuccess = false;
                    result.Code = 404;
                }
                else
                {
                    var orderViewModels = orders.Select(o => new OrderListViewModel
                    {
                        OrderId = o.OrderId,
                        Name = o.User.Name,
                        SessionId = o.SessionId,
                        Price = o.Price,
                        Quantity = o.Quantity,
                        StoreName = o.Store.Name,
                        TransationId = o.TransactionId,
                        Status = o.Status,
                        CreatedTime = o.CreatedTime,
                        CreatedDate = o.CreatedDate,
                    }).ToList();

                    result.Data = orderViewModels;
                    result.Message = "Success";
                    result.IsSuccess = true;
                    result.Code = 200;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
            }
            return result;
        }

        public async Task<ResultModel> getTotalOrderAmount(DateTime startDate, DateTime endDate)
        {
            var result = new ResultModel();
            try
            {
                var totalAmount = await _order.GetOrders()
                                                        .Where(o => o.CreatedDate.HasValue && o.CreatedDate.Value.Date >= startDate.Date && o.CreatedDate.Value.Date <= endDate.Date)
                                                        .SumAsync(o => o.Price);

                decimal percentageDifference = 0;
                decimal previousTotalAmount = 0;

                if (startDate.Date == endDate.Date)
                {
                    DateTime previousDate = startDate.AddDays(-1);

                    previousTotalAmount = await _order.GetOrders()
                                                      .Where(o => o.CreatedDate.HasValue
                                                              && o.CreatedDate.Value.Date == previousDate.Date)
                                                      .SumAsync(o => o.Price);

                    decimal combinedAmount = totalAmount + previousTotalAmount;

                    if (combinedAmount != 0)
                    {
                        decimal percentageToday = (totalAmount / combinedAmount) * 100;
                        decimal percentageYesterday = (previousTotalAmount / combinedAmount) * 100;
                        percentageDifference = Math.Round(percentageToday - percentageYesterday, 2);
                    }
                }

                result.Data = new
                {
                    totalAmount,
                    previousTotalAmount,
                    percentageDifference = $"{percentageDifference}%"
                };
                result.Message = "Success";
                result.IsSuccess = true;
                result.Code = 200;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
                result.Code = 500;
            }
            return result;
        }
        public async Task<ResultModel> getOrderAmountPerDayInMonth(int year, int month)
        {
            var result = new ResultModel();
            try
            {
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var orders = await _order.GetOrdersByDateRange(startDate, endDate);

                var data = orders.Where(o => o.CreatedDate.HasValue)
                                 .GroupBy(o => o.CreatedDate.Value.Date)
                                 .Select(g => new OrderAmountPerDayViewModel
                                 {
                                     Date = g.Key,
                                     TotalAmount = g.Sum(o => o.Price)
                                 })
                                 .ToList();

                result.Data = data;
                result.Message = "Success";
                result.IsSuccess = true;
                result.Code = 200;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
                result.Code = 500;
            }
            return result;
        }

        public async Task<ResultModel> getOrderAmountPerWeekInMonth(int year, int month)
        {
            var result = new ResultModel();
            try
            {
                var weeks = getWeeksInMonth(year, month);
                var orderAmountPerWeek = new List<OrderAmountPerWeekViewModel>();

                foreach (var week in weeks)
                {
                    var orders = await _order.GetOrdersByDateRange(week.StartDate, week.EndDate);
                    var totalAmount = orders.Sum(o => o.Price);

                    orderAmountPerWeek.Add(new OrderAmountPerWeekViewModel
                    {
                        StartDate = week.StartDate,
                        EndDate = week.EndDate,
                        TotalAmount = totalAmount
                    });
                }

                result.Data = orderAmountPerWeek;
                result.Message = "Success";
                result.IsSuccess = true;
                result.Code = 200;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
                result.Code = 500;
            }
            return result;
        }

        public async Task<ResultModel> getOrderAmountPerMonthInYear(int year)
        {
            var result = new ResultModel();
            try
            {
                var data = new List<OrderAmountPerMonthViewModel>();

                for (int month = 1; month <= 12; month++)
                {
                    var startDate = new DateTime(year, month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    var orders = await _order.GetOrdersByDateRange(startDate, endDate);
                    var totalAmount = orders.Sum(o => o.Price);

                    data.Add(new OrderAmountPerMonthViewModel
                    {
                        Month = month,
                        TotalAmount = totalAmount
                    });
                }

                result.Data = data;
                result.Message = "Success";
                result.IsSuccess = true;
                result.Code = 200;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
                result.Code = 500;
            }
            return result;
        }

        private List<(DateTime StartDate, DateTime EndDate)> getWeeksInMonth(int year, int month, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            var weeks = new List<(DateTime StartDate, DateTime EndDate)>();
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var currentStartOfWeek = firstDayOfMonth;
            while (currentStartOfWeek.DayOfWeek != startOfWeek)
            {
                currentStartOfWeek = currentStartOfWeek.AddDays(-1);
            }

            var currentEndOfWeek = currentStartOfWeek.AddDays(6);

            while (currentStartOfWeek <= lastDayOfMonth)
            {
                if (currentEndOfWeek > lastDayOfMonth)
                {
                    currentEndOfWeek = lastDayOfMonth;
                }

                weeks.Add((currentStartOfWeek, currentEndOfWeek));

                currentStartOfWeek = currentStartOfWeek.AddDays(7);
                currentEndOfWeek = currentStartOfWeek.AddDays(6);
            }

            return weeks;
        }
    }
}
