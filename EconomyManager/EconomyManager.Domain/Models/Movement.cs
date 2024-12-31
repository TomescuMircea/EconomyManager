using EconomyManager.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyManager.Domain.Models
{
    public class Movement : Entity
    {
        public static bool OUTCOME = false;
        public static bool INCOME = true;
        public float Quantity { get; set; }
        public bool IsIncome { get; set; }
        public bool IsOutcome => !IsIncome;
        public DateTime MovementDate { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public Wallet Wallet { get; set; }
        public int WalletId { get; set; }
        public string Amount { get { return (!IsIncome ? "-" : "") + $"{Quantity:F2}€"; } }

        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
        public override string ToString()
        {
            return $"Quantity: {Quantity}, Type: {IsIncome}, MovementDate: {MovementDate}, Category: {Category}";
        }

        public string GetDetails()
        {
            return ToString() ;
        }
    }
}
