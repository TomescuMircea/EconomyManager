using EconomyManager.Domain.Models;
using EconomyManager.Domain.Repositories;
using EconomyManager.Infrastructure;
using EconomyManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErasmusEconomy.Library.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository 
    {
        public UserRepository(EconomyManagerDBContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == email);
        }
        //TESTED - Is working

        public Task<User> FindByEmailAndPasswordAsync(string email, string password)
        {
            return _dbContext.Users
                .SingleOrDefaultAsync(x => x.Email == email && x.Password == password);
        }

        public async Task<User> FindByNameAsync(string username)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(x => x.Name == username);
        }

        public override async Task<User> FindOrCreateAsync(User entity)
        {
            var exist= await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == entity.Id);

            if (exist!= null && exist.Id != entity.Id)
            {
                this.Create(entity);
                exist = entity;
            }
            return exist;
        }
        //TESTED - Is working

        public async Task<User> Register(string name, string email, string password)
        {
            User usr = null;
            var existant = await FindByEmailAsync(email);
            if (existant == null)
            {
                usr = new User
                {
                    Name = name,
                    Email = email,
                    Password = password
                };
                base.Create(usr);
                //_dbContext.Users.Add(usr);
                _dbContext.SaveChanges();
            }
            return usr;
        }
    }
}
