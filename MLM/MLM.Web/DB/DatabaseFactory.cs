using System;
using System.Data.Entity;

namespace MLM.DB
{
    public class DatabaseFactory : IDatabaseFactory, IDisposable
    {
        private DbContext _database;
        public DbContext Get(IConnectionRetriever connRetreiver)
        {
            return _database ?? (_database = new MLMDbContext(connRetreiver));
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}