using EconomyManager.Domain.Models;
using EconomyManager.Domain.Repositories;
using EconomyManager.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomyManager.Infrastructure.Repositories
{
    public class WalletRepository : Repository<Wallet>, IWalletRepository
    {
        public WalletRepository(EconomyManagerDBContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Wallet> FindOrCreateAsync(Wallet wallet)
        {
            var exist = await _dbContext.Wallets.SingleOrDefaultAsync(x => x.Id == wallet.Id);

            if (exist != null && exist.Id==wallet.Id)
            {
                this.Create(wallet);
                exist = wallet;
            }
            return exist;
        }
        //TESTED - Is working

        public async Task<List<Wallet>> DeleteByUserByNameAsync(User user,string name)
        {
            var wallets = _dbContext.Wallets
                .Where(x => x.UserHasWallets.Any(y => y.UserId == user.Id));
            var removedWallets = new List<Wallet>();
            foreach (var wallet in wallets)
            {
                if (wallet.Name == name)
                {
                    removedWallets.Add(wallet);
                    _dbContext.Wallets.Remove(wallet);
                    List<UserHasWallet> userHasWallets = await _dbContext.UserHasWallets
                        .Where(x => x.WalletId == wallet.Id)
                        .ToListAsync();
                    foreach (var userHasWallet in userHasWallets)
                    {
                        _dbContext.UserHasWallets.Remove(userHasWallet);
                    }
                }
            }
            return removedWallets;
        }
        //NOT TESTED - It should work
        public async Task<List<Wallet>> FindAllByUserAsync(User user)
        {
            //List<UserHasWallet> userHasWalletList == await _dbContext.UserHasWallets
            //                                                 .Where(x => x.UserId == userId.Id)
            //                                                 .ToListAsync();

            //var wallets = await _dbContext.Wallets
            //    .Include(x => x.UserHasWallets)
            //    .ThenInclude(x=>x.User)
            //    //.Where(x => x)
            //    //.First(x => x.)
            //    .ToListAsync();

            var wallets = _dbContext.Wallets
                .Where(x => x.UserHasWallets.Any(y => y.UserId == user.Id))
                .Include(x => x.Movements)
                .Include(x => x.AutomaticMovements)
                .Include(x => x.Plans);

            return wallets.Where(x => x.UserHasWallets.Any(y=>y.UserId==user.Id)).ToList();
        }
        public async Task<List<Wallet>> FindAllByUserIdAsync(int userId)
        {
            //List<UserHasWallet> userHasWalletList == await _dbContext.UserHasWallets
            //                                                 .Where(x => x.UserId == userId.Id)
            //                                                 .ToListAsync();

            //var wallets = await _dbContext.Wallets
            //    .Include(x => x.UserHasWallets)
            //    .ThenInclude(x=>x.User)
            //    //.Where(x => x)
            //    //.First(x => x.)
            //    .ToListAsync();

            var wallets = _dbContext.Wallets
                .Where(x => x.UserHasWallets.Any(y => y.UserId == userId))
                .Include(x => x.Movements)
                .Include(x => x.AutomaticMovements)
                .Include(x => x.Plans);

            return wallets.Where(x => x.UserHasWallets.Any(y => y.UserId == userId))
                .Include(x => x.Movements)
                .Include(x => x.AutomaticMovements)
                .Include(x => x.Plans)
                .ToList();
        }
        //TESTED - Is working
        public async Task<List<Wallet>> DeleteAllByUserAsync(User user)
        {
            //List<Wallet> wallets = await FindAllAsync();
            var wallets = _dbContext.Wallets
                .Where(x => x.UserHasWallets.Any(y => y.UserId==user.Id));
            var removedWallets = new List<Wallet>();
            foreach (var wallet in wallets)
            {
                removedWallets.Add(wallet);
                _dbContext.Wallets.Remove(wallet);
                List<UserHasWallet> userHasWallets = await _dbContext.UserHasWallets
                    .Where(x => x.WalletId == wallet.Id)
                    .ToListAsync();
                foreach(var userHasWallet in userHasWallets)
                {
                    _dbContext.UserHasWallets.Remove(userHasWallet);
                }
            }
            return removedWallets;
        }
        //TESTED - Working
        public async Task<Wallet> FindByNameAsync(string name)
        {
            return await _dbContext.Wallets
                .Where(x=>x.Name== name)
                .SingleOrDefaultAsync();
        }

        public async Task CreateOrUpdate(Wallet wallet)
        {
            var existingWallet = await FindByNameAsync(wallet.Name);

            if (existingWallet == null)
            {

                _dbContext.Wallets.Add(wallet);
            }
            else
            {

                existingWallet.Description = wallet.Description;

            }

            await _dbContext.SaveChangesAsync();
        }
    }

   
}

