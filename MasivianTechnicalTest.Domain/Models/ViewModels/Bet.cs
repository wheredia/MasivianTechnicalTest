using System;
using System.Collections.Generic;
using System.Text;

namespace MasivianTechnicalTest.Domain.Models.ViewModels
{
    public class Bet
    {
        public enum betType { num, color };
        public Guid clientId { get; set; }
        public decimal amount { get; set; }
        public betType type { get; set; }
        public int num { get; set; }
    }
}
