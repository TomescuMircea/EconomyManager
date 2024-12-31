using EconomyManager.Domain.Models;
using EconomyManager.Domain.SeedWork;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace EconomyManager.Domain.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> CategoriesFromWallet(Wallet wallet);
        Task<List<Category>> CategoriesFromWalletId(int walletId);
        Task<List<Category>> CategoriesFromUser(User user);
        Task<List<Category>> CategoriesFromUserId(int userId);
    }
}
