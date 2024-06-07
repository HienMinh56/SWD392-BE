using SWD392_BE.Repositories.ViewModels.FoodModel;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Interfaces
{
    public interface IFoodService
    {
        public Task<ResultModel> getListFood();
        public Task<ResultModel> addFood(string storeId, List<FoodViewModel> foods, ClaimsPrincipal userCreate);
        public Task<ResultModel> updateFood(string foodId, FoodViewModel model, ClaimsPrincipal userUpdate);
    }
}
