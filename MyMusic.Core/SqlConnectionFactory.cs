using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Core
{
    public class SqlConnectionFactory : IDatabaseConnectionFactory
    {
        public string ConnectionString { get; }
        public SqlConnectionFactory(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public IDbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
