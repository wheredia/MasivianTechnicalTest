using MasivianTechnicalTest.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasivianTechnicalTest.Domain.Models.ViewModels
{
    public class Roulette : IResponseContent
    {
        public enum Status { create, open, close }
        public Guid id { get; set; }
        public IEnumerable<Bet> bets { get; set; }
        public Status status { get; set; }
    }
}
