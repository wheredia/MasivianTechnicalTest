using System;
using System.Collections.Generic;
using System.Text;

namespace MasivianTechnicalTest.Domain.Contracts
{
    public interface IDomain 
    {
        void GetDataAccess(MasivianTechnicalTest.DataAccess.Contracts.IDataAccess client);
    }
}
