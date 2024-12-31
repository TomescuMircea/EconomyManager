using EconomyManager.Domain;
using EconomyManager.Domain.Repositories;
using EconomyManager.Infrastructure.Repositories;
using ErasmusEconomy.Library.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EconomyManager.Infrastructure
{
    public class UnitOfWork: IUnitOfWork
    {
        private EconomyManagerDBContext _dbContext;
        public UnitOfWork() 
        {
            _dbContext = new EconomyManagerDBContext();
            _dbContext.Database.EnsureCreated();
        }

        public IAttachmentRepository AttachmentRepository => new AttachmentRepository(_dbContext);

        public IAutomaticMovementRepository AutomaticMovementRepository => new AutomaticMovementRepository(_dbContext);

        public ICategoryRepository CategoryRepository => new CategoryRepository(_dbContext);

        public IMovementRepository MovementRepository => new MovementRepository(_dbContext);

        public IPlanRepository PlanRepository => new PlanRepository(_dbContext);

        public IUserHasWalletRepository UserHasWalletRepository => new UserHasWalletRepository(_dbContext);

        public IUserRepository UserRepository => new UserRepository(_dbContext);

        public IWalletRepository WalletRepository => new WalletRepository(_dbContext);

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public string ReturnPath()
        {
            return _dbContext.DbPath;
        }
    }
}
