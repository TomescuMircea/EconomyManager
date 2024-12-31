using EconomyManager.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyManager.Domain.Models
{
    public class Plan : Entity
    {
        public string Name { get; set; }
        public float Quantity { get; set; }
        public float Objective { get; set; }
        public DateTime Deadline { get; set; }
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }
        public Plan(){
            Deadline = DateTime.Now;
        }
        public override string ToString()
        {
            return $"Quantity: {Quantity}; Objective: {Objective}; Deadline: {Deadline}; WalletId: {WalletId}";
        }
    }
}
