using EconomyManager.Domain.Models;
using EconomyManager.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EconomyManager.Domain.Repositories
{
    public interface IWalletRepository : IRepository<Wallet>
    {
        Task<Wallet> FindByNameAsync(string name);
        Task<List<Wallet>> FindAllByUserAsync(User user);
        Task<List<Wallet>> FindAllByUserIdAsync(int userId);
        Task<List<Wallet>> DeleteAllByUserAsync(User user);
        Task<List<Wallet>> DeleteByUserByNameAsync(User user, string name);

        Task CreateOrUpdate(Wallet wallet);
    }
}
