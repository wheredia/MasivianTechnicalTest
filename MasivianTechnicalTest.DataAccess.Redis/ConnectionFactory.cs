using StackExchange.Redis;
using System;

namespace MasivianTechnicalTest.DataAccess.Redis
{
    public class ConnectionFactory
    {
        private static Lazy<ConnectionMultiplexer> Connection;
        private readonly string REDIS_CONNECTIONSTRING = "REDIS_CONNECTIONSTRING";
        static ConnectionFactory()
        {
            Connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("192.168.176.1,6379"));
        }

        public static ConnectionMultiplexer GetConnection() => Connection.Value;
    }
}
