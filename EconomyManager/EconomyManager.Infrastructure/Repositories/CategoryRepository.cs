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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(EconomyManagerDBContext dbContext) : base(dbContext)
        {
        }


    public async Task<List<Category>> CategoriesFromWallet(Wallet wallet)
    {
            var usersFromWallet = _dbContext.UserHasWallets.Where(x=> x.WalletId == wallet.Id).ToList();
            ISet<int> usersIds = new HashSet<int>();
            foreach (var userWallet in usersFromWallet)
            {
                usersIds.Add(userWallet.UserId);
            }
            return await _dbContext.Categories
                .Where(x => usersIds.Contains(x.UserId))
                .Include(x => x.User)
                .Include(x => x.AutomaticMovements)
                .Include(x => x.Movements)
                .OrderBy(x => x.Id)
                .ToListAsync();

            /*var query = from category in _dbContext.Categories
                        join user in _dbContext.Users on category.UserId equals user.Id
                        join user_wallet in _dbContext.UserHasWallets on user.Id equals user_wallet.UserId
                        join wal in _dbContext.Wallets on user_wallet.WalletId equals wallet.Id
                        select new { category.Name };*/

        //return categories;
    }
        public async Task<List<Category>> CategoriesFromWalletId(int walletId)
        {
            var usersFromWallet = _dbContext.UserHasWallets.Where(x => x.WalletId == walletId).ToList();
            ISet<int> usersIds = new HashSet<int>();
            foreach (var userWallet in usersFromWallet)
            {
                usersIds.Add(userWallet.UserId);
            }
            return await _dbContext.Categories
                .Where(x => usersIds.Contains(x.UserId))
                .Include(x => x.User)
                .Include(x => x.AutomaticMovements)
                .Include(x => x.Movements)
                .OrderBy(x => x.Id)
                .ToListAsync();

            /*var query = from category in _dbContext.Categories
                        join user in _dbContext.Users on category.UserId equals user.Id
                        join user_wallet in _dbContext.UserHasWallets on user.Id equals user_wallet.UserId
                        join wal in _dbContext.Wallets on user_wallet.WalletId equals wallet.Id
                        select new { category.Name };*/

            //return categories;
        }

        public async Task<List<Category>> CategoriesFromUser(User user)
    {
            return await _dbContext.Categories
                    .Where(x => x.UserId == user.Id)
                    .Include(x => x.User)
                    .Include(x => x.AutomaticMovements)
                    .Include(x => x.Movements)
                    .OrderBy(x => x.Id)
                    .ToListAsync();
        }
        public async Task<List<Category>> CategoriesFromUserId(int userId)
        {
            return await _dbContext.Categories
                .Where(x => x.UserId == userId)
                .Include(x => x.User)
                .Include(x => x.AutomaticMovements)
                .Include(x => x.Movements)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public override async Task<Category> FindOrCreateAsync(Category e)
        {
            var f = await _dbContext.Categories
                .SingleOrDefaultAsync(x => x.User == e.User && x.Name == e.Name);

            if (f == null)
            {
                Create(e);
                f = e;
            }
            return f;
        }
    }


}

