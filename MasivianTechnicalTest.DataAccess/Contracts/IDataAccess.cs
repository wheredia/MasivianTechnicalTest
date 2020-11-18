using System;
using System.Collections.Generic;
using System.Text;

namespace MasivianTechnicalTest.DataAccess.Contracts
{
    public interface IDataAccess
    {
        string Get(string key);
        void Set(string key, string value);
        void Delete(string key);
        Dictionary<string, string> GetAll();
    }
}
