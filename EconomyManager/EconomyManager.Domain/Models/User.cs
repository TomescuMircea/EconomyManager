    using EconomyManager.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EconomyManager.Domain.Models
{
    public class User : Entity
    {
        public static string EncodeString(string value)
        {
            var data = Encoding.UTF8.GetBytes(value);
            var hashData = new SHA1Managed().ComputeHash(data);
            return BitConverter.ToString(hashData).Replace("-", "").ToUpper();
        }

        private string _password;

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = User.EncodeString(value);
            }
        }
        public List<Category> Categories { get; set; }
        public List<UserHasWallet> UserHasWallets { get; set; }

        public bool Active { get; set; }

        public User()
        {
            Categories = new List<Category>();
            UserHasWallets = new List<UserHasWallet>();
        }
        public override string ToString()
        {
            return $"{Id}: {Name}";
        }
    }
}
