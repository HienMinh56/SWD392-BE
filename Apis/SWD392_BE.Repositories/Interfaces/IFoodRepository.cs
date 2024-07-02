using Microsoft.EntityFrameworkCore;
using SWD392_BE.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_BE.Repositories.Interfaces
{
    public interface IFoodRepository : IGenericRepository<Food>
    {
        Task<string> GetLastFoodIdAsync();
        Task<Food> GetAsync(Expression<Func<Food, bool>> predicate);
    }
}
