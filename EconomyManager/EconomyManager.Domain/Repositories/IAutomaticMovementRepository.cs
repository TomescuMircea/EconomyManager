using EconomyManager.Domain.Models;
using EconomyManager.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EconomyManager.Domain.Repositories
{
    public interface IAutomaticMovementRepository : IRepository<AutomaticMovement>
    {
        Task<List<AutomaticMovement>> FindAllByWalletIdAsync(int walletId);
    }
}
