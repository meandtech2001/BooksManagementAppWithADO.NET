using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.BookManagement
{
    public interface IConnectionsFactory
    {
        public IDbConnection CreateConnection();

        //public static IDbConnection CreateSqlConnection(string connectionString)
        //{
        //    return new SqlConnection(connectionString);
        //}
    }

    public class SqlConnectionFactory : IConnectionsFactory
    {
        private string connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDbConnection CreateConnection()

        {
            var c = new SqlConnection() { ConnectionString = connectionString };
            c.Open();
            return c;
        }
    }
}