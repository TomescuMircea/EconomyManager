using EconomyManager.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyManager.Domain.Models
{
    public class AutomaticMovement : Entity
    {
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public float Quantity { get; set; }
        public bool IsIncome { get; set; }
        public int Frequency { get; set; }
        public bool Active { get; set; }
        public Wallet Wallet { get; set; }
        public int WalletId { get; set; }
        private string Status => Active ? "Active" : "Deactivated";
        private string Type => IsIncome ? "Income" : "Outcome";
        public override string ToString()
        {
            return $"({Id} from {WalletId}) Category:{Category}, Quantity: {Quantity} ({Type}), Frequency: {Frequency} days, {Status}";
        }
    }
}
