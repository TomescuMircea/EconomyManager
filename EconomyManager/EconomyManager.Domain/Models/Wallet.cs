using EconomyManager.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyManager.Domain.Models
{
    public class Wallet : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserHasWallet> UserHasWallets { get; set; }
        public List<Movement> Movements { get; set; }
        public List<Plan> Plans { get; set; }
        public List<AutomaticMovement> AutomaticMovements { get; set; }
        public string Ammount => $"{Balance():F2}€";
        
        public Wallet()
        {
            AutomaticMovements = new List<AutomaticMovement>();
            Movements = new List<Movement>();
            Plans = new List<Plan>();
            UserHasWallets = new List<UserHasWallet>();
        }

        public override string ToString()
        {
            string toString = $"Name: {Name} -- Description: {Description}\n" +
                $"user: ";
            foreach(var item in UserHasWallets)
                toString += item.ToString();
            return toString;
        }

        public float Balance()
        {
            float balance = 0f;
            foreach(Movement mov in Movements)
            {
                balance += (mov.Quantity *(mov.IsIncome? 1 : -1)); 
            }
            return balance;
        }



    }
}
