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
    public class FoodRepository : GenericRepository<Food>, IFoodRepository
    {
        private readonly CampusFoodSystemContext _context;

        public FoodRepository(CampusFoodSystemContext context) : base(context)
        {
            _context = context;
        }
        public async Task<string> GetLastFoodIdAsync()
        {
            // Example query to get the last StoreId
            var lastFood = await _context.Foods
                                            .OrderByDescending(s => s.FoodId)
                                            .FirstOrDefaultAsync();

            return lastFood?.FoodId;
        }
        public async Task<IEnumerable<Food>> FilterFoodsAsync(int? cate)
        {
            IQueryable<Food> query = _context.Foods;

            if (cate.HasValue)
            {
                query = query.Where(s => s.Cate == cate.Value);
            }

            return await query.ToListAsync();
        }
    }
}
