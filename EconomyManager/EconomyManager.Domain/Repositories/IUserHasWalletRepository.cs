using EconomyManager.Domain.Models;
using EconomyManager.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EconomyManager.Domain.Repositories
{
    public interface IUserHasWalletRepository : IRepository<UserHasWallet>
    {
        Task<List<UserHasWallet>> FindByUserIdAsync(int id);
        Task<List<UserHasWallet>> FindByWalletIdAsync(int id);
    }
}
