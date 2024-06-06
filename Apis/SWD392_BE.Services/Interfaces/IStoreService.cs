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
        public Task<ResultModel> getListStore();
        public Task<ResultModel> addStore(StoreViewModel storeReqModel, ClaimsPrincipal userCreate);
        public Task<ResultModel> updateStore(string storeId, StoreViewModel storeReqModel, ClaimsPrincipal userUpdate);
    }
}
