using EconomyManager.Domain.Models;
using EconomyManager.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EconomyManager.Domain.Repositories
{
    public interface IMovementRepository : IRepository<Movement>
    {
        void CreateAll(List<Movement> movements);
        Task<List<Movement>> FindAllByWalletAsync(Wallet wallet);
        Task<List<Movement>> FindAllByWalletIdAsync(int walletId);
        Task<List<Movement>> FindAllByCategoryAsync(Category category);
        Task<List<Movement>> FindAllByTypeAsync(bool isIncome);
        Task<List<Movement>> FindAllByMinQuantity(float quantity);
    }
}
