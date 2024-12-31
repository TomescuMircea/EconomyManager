using EconomyManager.Domain.Models;
using EconomyManager.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyManager.Infrastructure.Repositories
{
    public class AutomaticMovementRepository : Repository<AutomaticMovement>, IAutomaticMovementRepository
    {
        public AutomaticMovementRepository(EconomyManagerDBContext dbContext) : base(dbContext)
        {
        }

        public Task<List<AutomaticMovement>> FindAllByWalletIdAsync(int walletId)
        {
            return _dbContext.AutomaticMovements
                .Where(x => x.WalletId == walletId)
                .Include(x => x.Category)
                .Include(x => x.Wallet)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public override Task<AutomaticMovement> FindOrCreateAsync(AutomaticMovement e)
        {
            throw new NotImplementedException();
        }
    }
}
