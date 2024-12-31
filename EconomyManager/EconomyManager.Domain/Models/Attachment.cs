using EconomyManager.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace EconomyManager.Domain.Models
{
    public class Attachment : Entity
    {
        public string Url { get; set; }
        public string Type { get; set; }
        public int MovementId { get; set; }
        public Movement Movement { get; set; }
        public Attachment()
        {

        }

        public override string ToString()
        {
            return $"Url: {Url}, Type: {Type}, MovementId: {MovementId}";
        }
    }
}
