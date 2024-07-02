using SWD392_BE.Repositories.Interfaces;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Services
{
    public class OrderDetailsServices : IOrderDetailsServices
    {
        private readonly IOrderDetailsRepository _orderDetails;

        public OrderDetailsServices(IOrderDetailsRepository orderDetails)
        {
            _orderDetails = orderDetails;
        }
        public Task<ResultModel> GetOrderDetails(string orderId)
        {
            var result = new ResultModel();
            var orderDetails = _orderDetails.GetList(o => o.OrderId == orderId);
            if (orderDetails == null)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.Message = "Order details not found";
                return Task.FromResult(result);
            }
            result.IsSuccess = true;
            result.Code = 200;
            result.Message = "Get order details successfully";
            result.Data = orderDetails;

            return Task.FromResult(result);
        }
    }
}
