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
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly CampusFoodSystemContext _dbContext;

        public GenericRepository(CampusFoodSystemContext dbContext) 
        {
            _dbSet = dbContext.Set<TEntity>();
            _dbContext = dbContext;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {           
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return _dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<bool> Update(TEntity entity)
        {           
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
