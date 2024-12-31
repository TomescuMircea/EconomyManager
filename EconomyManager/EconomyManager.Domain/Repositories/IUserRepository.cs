using EconomyManager.Domain.Models;
using EconomyManager.Domain.SeedWork;
using System.Threading.Tasks;

namespace EconomyManager.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByNameAsync(string username);
        Task<User> Register(string name, string email, string password);
        Task<User> FindByEmailAndPasswordAsync(string email, string password);
    }
}
