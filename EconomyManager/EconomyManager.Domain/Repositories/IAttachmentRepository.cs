using System.Collections.Generic;
using System.Threading.Tasks;
using EconomyManager.Domain.Models;

namespace EconomyManager.Domain.Repositories
{
    public interface IAttachmentRepository
    {
        Task<List<Attachment>> FindAllAsync();
        Task<Attachment> GetByIdAsync(int attachmentId);

    }
}
