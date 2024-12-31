using EconomyManager.Domain.Models;
using EconomyManager.Infrastructure.Repositories;
using EconomyManager.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EconomyManager.Infrastructure;

namespace ErasmusEconomy.Library.Repositories
{
    public class UserHasWalletRepository : Repository<UserHasWallet>, IUserHasWalletRepository
    {
        public UserHasWalletRepository(EconomyManagerDBContext dbContext) : base(dbContext)
        {
        }

        public Task<List<UserHasWallet>> FindByUserIdAsync(int id)
        {
            return _dbContext.UserHasWallets
                .Where(x => x.UserId == id)
                .Include(x => x.User)
                .Include(x => x.Wallet)
                .OrderBy(x => x.WalletId)
                .ToListAsync();
        }
        public Task<List<UserHasWallet>> FindByWalletIdAsync(int id)
        {
            return _dbContext.UserHasWallets
                .Where(x => x.WalletId == id)
                .Include(x => x.User)
                .Include(x => x.Wallet)
                .OrderBy(x => x.WalletId)
                .ToListAsync();
        }
        public override async Task<UserHasWallet> FindOrCreateAsync(UserHasWallet entity)
        {
            var exist = await _dbContext.UserHasWallets
                .Where(x =>x.UserId == entity.UserId)
                .SingleOrDefaultAsync(x =>x.WalletId == entity.WalletId);

            if (exist != null && exist.UserId!=entity.UserId && exist.WalletId!=entity.WalletId)
            {
                this.Create(entity);
                exist = entity;
            }
            return exist;
        }

    }
}
