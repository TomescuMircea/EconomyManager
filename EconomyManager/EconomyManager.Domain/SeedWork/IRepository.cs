using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EconomyManager.Domain.SeedWork
{
    public interface IRepository<T> where T : Entity
    {
        void Create(T e);
        void Update(T e);
        void Delete(T e);
        Task<T> FindOrCreateAsync(T e);
        Task<T> FindByIdAsync(int id);
        Task<List<T>> FindAllAsync();
    }
}
