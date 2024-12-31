using EconomyManager.Domain.Models;
using EconomyManager.Domain.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EconomyManager.Domain.Repositories
{
    public interface IPlanRepository : IRepository<Plan>
    {
        Task<Plan> FindPlanByPlanIdAsync(int planId);
        Task<List<Plan>> FindAllByWalletIdAsync(int walletId);
        Task<List<Plan>> FindAllByWalletAsync(Wallet wallet);
    }
}
