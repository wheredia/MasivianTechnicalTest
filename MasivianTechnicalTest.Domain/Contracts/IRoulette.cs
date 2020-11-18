using System;
using System.Collections.Generic;
using System.Text;
using ExportModels = MasivianTechnicalTest.Domain.Models.ViewModels;

namespace MasivianTechnicalTest.Domain.Contracts
{
    public interface IRoulette : IDomain
    {
        IResponse Create();

        IResponse Open(Guid id);

        IResponse AddBet(Guid id, Object bet);

        IResponse Close(Guid id);

        IList<ExportModels.Roulette> GetAll();

    }
}
