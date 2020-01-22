using MongoDB.Driver;
using ElasticSearch7Template.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch7Template.Utility
{
    public class MongoDBFactory
    {
        private static readonly string connStr = MongoDBConfig.MongoDbUrl;

        private static readonly string dbName = MongoDBConfig.MongoDbName;

        private static IMongoDatabase db = null;

        private static readonly object lockHelper = new object();

        private MongoDBFactory() { }

        public static IMongoDatabase GetDb()
        {
            if (db == null)
            {
                lock (lockHelper)
                {
                    if (db == null)
                    {
                        var client = new MongoClient(connStr);
                        db = client.GetDatabase(dbName);
                    }
                }
            }
            return db;
        }
    }
}
