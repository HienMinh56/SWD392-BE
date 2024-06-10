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
    public class OrderServices : IOrderService
    {
        private readonly IOrderRepository _order;

        public OrderServices(IOrderRepository order)
        {
            _order = order;
        }

        public async Task<ResultModel> getOrder()
        {
            var result = new ResultModel();
            try
            {
                var order = _order.GetAll();
                if (order == null)
                {
                    result.IsSuccess = true;
                    result.Code = 201;
                    result.Message = "No Order Here";
                    return result;
                }

                result.IsSuccess = true;
                result.Code = 200;
                result.Data = order;
                result.Message = "Get Order Success";
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Code = 400;
                result.Message = ex.Message;
                return result;
            }
        }
    }
}

