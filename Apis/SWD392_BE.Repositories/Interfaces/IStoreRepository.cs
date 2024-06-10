using SWD392_BE.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.Interfaces
{
    public interface IStoreRepository : IGenericRepository<Store>
    {
        Task<string> GetLastStoreIdAsync();
        public Store GetStoreWithFoods(string storeId);
        Task<List<Store>> GetStores();
        Task<IEnumerable<Store>> FilterStoresAsync(string? areaId, int? status);
    }
}
