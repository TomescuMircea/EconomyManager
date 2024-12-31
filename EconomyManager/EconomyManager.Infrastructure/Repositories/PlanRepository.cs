using EconomyManager.Domain.Models;
using EconomyManager.Domain.Repositories;
using EconomyManager.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyManager.Infrastructure.Repositories
{
    public class PlanRepository : Repository<Plan>, IPlanRepository
    {
        public PlanRepository(EconomyManagerDBContext dbContext) : base(dbContext)
        {
        }
        public Task<Plan> FindPlanByPlanIdAsync(int planId)
        {
            return _dbContext.Plans.SingleOrDefaultAsync(x => x.Id == planId);
        }
        public Task<List<Plan>> FindAllByWalletIdAsync(int walletId)
        {
            return _dbContext.Plans
                .Where(x => x.WalletId == walletId)
                .Include(x => x.Wallet)
                .OrderBy(x => x.Quantity)
                .ToListAsync();
        }
        public Task<List<Plan>> FindAllByWalletAsync(Wallet wallet)
        {
            return _dbContext.Plans
                .Where(x => x.WalletId == wallet.Id)
                .Include(x => x.Wallet)
                .OrderBy(x => x.Quantity)
                .ToListAsync();
        }

        public override async Task<Plan> FindOrCreateAsync(Plan entity)
        {
            var exist = await _dbContext.Plans.SingleOrDefaultAsync(x => x.Id == entity.Id);

            if (exist != null && exist.Id != entity.Id)
            {
                this.Create(entity);
                exist = entity;
            }
            return exist;
        }
    }
}
