using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasivianTechnicalTest.WebService.ViewModel
{
    public class Bet
    {
        public enum BetType { num, color };
        public Guid ClientId { get; set; }
        public decimal Amount { get; set; }
        public BetType Type { get; set; }
        public int Num { get; set; }
    }
}
