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
    }
}
