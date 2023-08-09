using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Npgsql;

namespace BooksManagmentConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var password = "22GbE2lDoF2v3HYG7L7o0Qoa4BBwUSBh";
            var userName = "dexynogt";
            var db = userName;
            var server = "john.db.elephantsql.com";
            var connectionString = $"Server={server};Userid={userName};Password={password};Database={db}";

            IDbConnection connection = null;



            try
            {
                //step #1 create a connection
                //connection = new SqlConnection();

                connection = new NpgsqlConnection();

                //connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\MyWorks\Corporate\202307-ecolab-cs\booksdb.mdf;Integrated Security=True;Connect Timeout=30";
                connection.ConnectionString = connectionString;
                connection.Open();

                //step #2 create a command
                //IDbCommand command = new SqlCommand();

                IDbCommand command = connection.CreateCommand();
                command.Connection = connection;
                command.CommandText = "select * from authors";

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var authorName = (string)reader["name"];
                    Console.WriteLine(authorName);

                }

            }catch(SqlException ex)
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
