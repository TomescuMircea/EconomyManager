using EconomyManager.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EconomyManager.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IAttachmentRepository AttachmentRepository { get; }
        IAutomaticMovementRepository AutomaticMovementRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IMovementRepository MovementRepository { get; }
        IPlanRepository PlanRepository { get; }
        IUserHasWalletRepository UserHasWalletRepository { get; }
        IUserRepository UserRepository { get; }
        IWalletRepository WalletRepository { get; }
        Task SaveAsync();

        string ReturnPath();
    }
    // Some comment
    //Some comment 2
    // Some comment 3
}
