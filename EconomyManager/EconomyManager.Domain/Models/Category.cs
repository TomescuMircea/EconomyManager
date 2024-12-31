using EconomyManager.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyManager.Domain.Models
{
    public class Category : Entity
    {
        public string Name { get; set; }
        public bool IsIncome { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public List<Movement> Movements { get; set; }
        public List<AutomaticMovement> AutomaticMovements { get; set; }
        public string Type => (IsIncome) ? "Incomes" : "Outcomes";

        public override string ToString()
        {
            return $"Name: {Name} ({User.Id} - {User.Name})";
        }


        public Category() 
        {
            Movements = new List<Movement>();
            AutomaticMovements = new List<AutomaticMovement>();
        }
    }
}
