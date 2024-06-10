using Microsoft.EntityFrameworkCore;
using SWD392_BE.Repositories.Entities;
using SWD392_BE.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.Repositories
{
    public class StoreRepository : GenericRepository<Store>, IStoreRepository
    {
        private readonly CampusFoodSystemContext _context;

        public StoreRepository(CampusFoodSystemContext context) : base(context)
        {
            _context = context;
        }
        public async Task<string> GetLastStoreIdAsync()
        {
            // Example query to get the last StoreId
            var lastStore = await _context.Stores
                                            .OrderByDescending(s => s.StoreId)
                                            .FirstOrDefaultAsync();

            return lastStore?.StoreId;
        }

        public async Task<List<Store>> GetStores()
        {
            var stores = await _context.Stores
                .Include(x => x.Area)
                .Include(x => x.StoreSessions)
                .AsNoTracking()
                .ToListAsync();

            var result = stores.Select(x => new Store
            {
                StoreId = x.StoreId,
                Name = x.Name,
                Address = x.Address,
                Area = new Area { Name = x.Area.Name }
            }).ToList();

            // Populate StoreSessions for each store
            foreach (var store in result)
            {
                var storeEntity = stores.First(s => s.StoreId == store.StoreId);
                foreach (var session in storeEntity.StoreSessions)
                {
                    store.StoreSessions.Add(new StoreSession
                    {
                        SessionId = session.SessionId,
                        StoreSessionId = session.StoreSessionId,
                        StoreId = session.StoreId
                    });
                }
            }

            return result;
        }


        public async Task<IEnumerable<Store>> FilterStoresAsync(string? areaId, int? status)
        {
            IQueryable<Store> query = _context.Stores;

            if (!string.IsNullOrEmpty(areaId))
            {
                query = query.Where(s => s.AreaId == areaId);
            }

            if (status.HasValue)
            {
                query = query.Where(s => s.Status == status.Value);
            }

            return await query.ToListAsync();
        }
        public Store GetStoreWithFoods(string storeId)
        {
            return _context.Stores
                           .Include(s => s.Foods)
                           .FirstOrDefault(s => s.StoreId == storeId);
        }
    }
}
