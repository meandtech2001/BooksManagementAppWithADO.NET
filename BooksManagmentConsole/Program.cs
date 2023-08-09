using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using BooksManagmentConsole;

namespace BooksManagmentConsole
{
    internal class Program
    {
        public delegate IDbConnection ConnectionsFactoryDelegate(string connectionString);

        private static void Main(string[] args)
        {
            //private IConnectionsFactory factory;
            //ConnectionsFactoryDelegate connectionCreator = new ConnectionsFactoryDelegate(ConnectionsFactory.CreateSqlConnection);

            IDbConnection connection = null;
            try
            {
                var connectionString = @"Data Source=.;Initial Catalog=BookTestData;Integrated Security=True";
                //step #1 create a connection
                connection = new SqlConnection(connectionString);

                connection.Open();

                //step #2 create a command
                IDbCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "select * from authors";

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var authorName = (string)reader["name"];
                    Console.WriteLine(authorName);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}