using EconomyManager.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EconomyManager.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly EconomyManagerDBContext _dbContext;
        public Repository(EconomyManagerDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Create(T e) 
        {
            _dbContext.Add(e);
        }

        public void Delete(T e)
        {
            _dbContext.Remove(e);
        }

        public async Task<List<T>> FindAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> FindByIdAsync(int id)
        {
            return await _dbContext.Set<T>().SingleAsync(x => x.Id == id);
        }
        public void Update(T e)
        {
            _dbContext.Update(e);
        }
        public abstract Task<T> FindOrCreateAsync(T e);
    }
}
