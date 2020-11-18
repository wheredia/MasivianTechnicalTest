using StackExchange.Redis;
using System;

namespace MasivianTechnicalTest.DataAccess.Redis
{
    public class ConnectionFactory
    {
        private static Lazy<ConnectionMultiplexer> Connection;        
        static ConnectionFactory()
        {
            Connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("192.168.176.1,6379"));
        }

        public static ConnectionMultiplexer GetConnection() => Connection.Value;
    }
}
