using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.ViewModels.ResultModel;
using SWD392_BE.Repositories.ViewModels.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Services.Interfaces
{
    public interface IStoreService
    {
        Task<IEnumerable<GetStoreViewModel>> GetStoresAsync(int? status, string? areaName, string? sessionId);
        Task<ResultModel> FilterStoresAsync(string? areaId, int? status);
        public Task<ResultModel> addStore(StoreViewModel storeReqModel, ClaimsPrincipal userCreate);
        public Task<ResultModel> UpdateStoreAsync(string storeId, UpdateStoreViewModel model, ClaimsPrincipal userUpdate);
        public Task<ResultModel> DeleteStore(DeleteStoreReqModel request, ClaimsPrincipal userDelete);
    }
}
