using System.Collections.Generic;
using System.Threading.Tasks;
using EconomyManager.Domain.Models;
using EconomyManager.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EconomyManager.Infrastructure.Repositories
{
    internal class AttachmentRepository : IAttachmentRepository
    {
        private readonly EconomyManagerDBContext _dbContext;

        public AttachmentRepository(EconomyManagerDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Attachment>> FindAllAsync()
        {
            return await _dbContext.Attachments.Include(x => x.Movement).ToListAsync();
        }

        public async Task<Attachment> GetByIdAsync(int attachmentId)
        {
            return await _dbContext.Attachments.FindAsync(attachmentId);
        }

        // Add other methods as needed based on your requirements

        public void Create(Attachment attachment)
        {
            _dbContext.Attachments.Add(attachment);
        }

        public void Update(Attachment attachment)
        {
            _dbContext.Attachments.Update(attachment);
        }

        public void Delete(Attachment attachment)
        {
            _dbContext.Attachments.Remove(attachment);
        }
    }
}
