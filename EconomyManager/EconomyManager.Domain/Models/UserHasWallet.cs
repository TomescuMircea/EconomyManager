using EconomyManager.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyManager.Domain.Models
{
    public class UserHasWallet : Entity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }
        public UserHasWallet() { }

        public override string ToString()
        {
            string toString = $"UsedName: {User.Name} WalletName: {Wallet.Name} Categories: \n";
            foreach (var item in User.Categories)
                toString += item.Name ;
            return toString;
        }
    }
}
