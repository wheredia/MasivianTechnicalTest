using MasivianTechnicalTest.DataAccess.Contracts;
using StackExchange.Redis;
using System.Collections.Generic;

namespace MasivianTechnicalTest.DataAccess.Redis.Implementations
{
    public class Client : IDataAccess
    {
        private static IDatabase _database;
        private readonly RedisKey _key = new RedisKey("RouletteRepository");
        public Client()
        {
            _database = ConnectionFactory.GetConnection().GetDatabase();
        }
        public void Delete(string key)
        {
            throw new System.NotImplementedException();
        }
        public string Get(string key)
        {
            return _database.HashGet(key: _key, hashField: new RedisValue(key)).ToString();
        }
        public Dictionary<string, string> GetAll()
        {
            var response = new Dictionary<string, string>();
            foreach (var o in  _database.HashKeys(key: _key))
            {
                response.Add(o.ToString(), _database.HashGet(key: _key, o).ToString());
            }

            return response;
        }
        public void Set(string key, string value)
        {
            _database.HashSet(key: _key, hashField: new RedisValue(key), value: new RedisValue(value));
        }
    }
}
