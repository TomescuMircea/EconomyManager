using EconomyManager.Domain.Models;
using EconomyManager.Domain.Repositories;
using EconomyManager.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EconomyManager.Infrastructure.Repositories
{
    public class MovementRepository : Repository<Movement>, IMovementRepository
    {
        public MovementRepository(EconomyManagerDBContext dbContext) : base(dbContext)
        {
        }

        public Task<List<Movement>> FindAllByCategoryAsync(Category category)
        {
            return _dbContext.Movements
                .Where(x => x.CategoryId == category.Id)
                .Include(x => x.Wallet)
                .Include(x => x.Category)
                .Include(x => x.Attachments)
                .OrderBy(x => x.Quantity)
                .ToListAsync();
        }

        public Task<List<Movement>> FindAllByMinQuantity(float quantity)
        {
            return _dbContext.Movements
                .Where(x => x.Quantity == quantity)
                .Include(x => x.Wallet)
                .Include(x => x.Category)
                .Include(x => x.Attachments)
                .OrderBy(x => x.Quantity)
                .ToListAsync();
        }

        public Task<List<Movement>> FindAllByTypeAsync(bool isIncome)
        {
            return _dbContext.Movements
                .Where(x => x.IsIncome == isIncome)
                .Include(x => x.Wallet)
                .Include(x => x.Category)
                .Include(x => x.Attachments)
                .OrderBy(x => x.Quantity)
                .ToListAsync();
        }

        public void CreateAll(List<Movement> movements)
        {
            foreach (var movement in movements)
            {
                base.Create(movement);
            }
        }

        public Task<List<Movement>> FindAllByWalletAsync(Wallet wallet)
        {
            return _dbContext.Movements
                .Where(x => x.WalletId == wallet.Id)
                .Include(x => x.Wallet)
                .Include(x => x.Category)
                .Include(x => x.Attachments)
                .OrderBy(x => x.MovementDate)
                .ToListAsync();
        }
        public Task<List<Movement>> FindAllByWalletIdAsync(int walletId)
        {
            return _dbContext.Movements
                .Where(x => x.WalletId == walletId)
                .Include(x => x.Wallet)
                .Include(x => x.Category)
                .Include(x => x.Attachments)
                .OrderBy(x => x.MovementDate)
                .ToListAsync();
        }

        public Task<List<Movement>> FindAllAsync()
        {
            return _dbContext.Movements.ToListAsync();
        }

        public override async Task<Movement> FindOrCreateAsync(Movement entity)
        {
            var existingEntity = await _dbContext.Movements
                .SingleOrDefaultAsync(x => x.Id == entity.Id);

            if (existingEntity == null)
            {
                // If the entity doesn't exist, create a new one
                this.Create(entity);
                existingEntity = entity;
            }

            return existingEntity;
        }
    }
}
