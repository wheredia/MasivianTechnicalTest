using System;
using System.Collections.Generic;
using System.Text;

namespace MasivianTechnicalTest.Domain.Models.ViewModels
{
    public class Bet
    {
        public enum BetType { num, color };
        public enum BetColor { red, black };
        public Guid ClientId { get; set; }
        public double Amount { get; set; }
        public BetType Type { get; set; }
        public BetColor Color { get; set; }
        public int Num { get; set; }
    }
}
