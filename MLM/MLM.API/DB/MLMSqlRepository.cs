using MLM.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MLM.API.DB
{
    public class MLMSqlRepository<T> : RepositoryBase<T> where T : MLMBaseEntity
    {
        private readonly MLMDbContext _database = null;
        private readonly IDbSet<T> _dbSet;

        public MLMSqlRepository(DatabaseFactory databaseFactory, ConnectionRetriever connRetriever)
            : base(databaseFactory, connRetriever)
        {
            _database = (MLMDbContext)databaseFactory.Get(connRetriever);
            _dbSet = _database.Set<T>();
        }

        public virtual void Update(Type type, string column, string value, string predicate)
        {
            var table = _database.GetSchemaName() + "." + TableColumnNamesHelper.GetTableName(type);
            _database.Database.ExecuteSqlCommand(String.Format("UPDATE {0} SET {1} = {2} WHERE {3}", table, column, value, predicate));

        }

        public virtual void Insert(Type type, string columnsOrder, string predicate)
        {
            var table = _database.GetSchemaName() + "." + TableColumnNamesHelper.GetTableName(type);
            _database.Database.ExecuteSqlCommand(String.Format("insert into {0}{1} values {2}", table, columnsOrder, predicate));


        }

        public virtual System.Data.Common.DbDataReader ExecuteProcedure(string procedureName, SqlParameter[] parameters)
        {
            var con = new SqlConnection(_database.Database.Connection.ConnectionString);
            con.Open();
            var cmd = con.CreateCommand();

            cmd.CommandText = _database.GetSchemaName() + "." + procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
   
    }
}