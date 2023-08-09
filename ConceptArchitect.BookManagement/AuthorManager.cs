using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConceptArchitect.BookManagement
{
    //public delegate IDbConnection ConnectionsFactoryDelegate(string connectionString);

    public class AuthorManager
    {
        //const string connectionString= @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\MyWorks\Corporate\202307-ecolab-cs\booksdb.mdf;Integrated Security = True; Connect Timeout = 30";
        //public IConnectionsFactory connectionFactory;

        private Func<IDbConnection> connectionFactory;

        public string ConnectionString { get; set; }

        public AuthorManager(Func<IDbConnection> factory)
        {
            connectionFactory = factory;
        }

        //public IDbConnection MakeConnection()
        //{
        //    IDbConnection connection = null;

        //    connection = connectionFactory.CreateConnection();
        //    //connection = connectionCreator(ConnectionString);
        //    //connection = ConnectionsFactory.CreateSqlConnection(ConnectionString);
        //    //connection.ConnectionString = ConnectionString;
        //    //connection.Open();
        //    //return connection;
        //}

        public T ExecuteCommand<T>(Func<IDbCommand, T> commandExecutor)
        {
            IDbConnection connection = null;
            try
            {
                connection = connectionFactory();
                var command = connection.CreateCommand();

                return commandExecutor(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.GetType()} : {ex.Message}");
                throw;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public List<Author> GetAllAuthors()
        {
            return ExecuteCommand(command =>
            {
                var authors = new List<Author>();
                command.CommandText = "select * from authors";
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var author = new Author()
                    {
                        Id = reader["id"].ToString(),
                        Name = reader["name"].ToString(),
                        Biography = reader["biography"].ToString(),
                        Photo = reader["photo"].ToString(),
                        Email = reader["email"].ToString()
                    };

                    authors.Add(author);
                }

                return authors;
            });
        }

        public Author GetAuthorById(string id)
        {
            return ExecuteCommand(command =>
            {
                command.CommandText = $"select * from authors where id='{id}'";
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var author = new Author()
                    {
                        Id = reader["id"].ToString(),
                        Name = reader["name"].ToString(),
                        Biography = reader["biography"].ToString(),
                        Photo = reader["photo"].ToString(),
                        Email = reader["email"].ToString()
                    };

                    return author;
                }

                throw new InvalidIdException<string>() { Id = id };
            });
        }

        public int GetAuthorCount()
        {
            return ExecuteCommand(command =>
            {
                command.CommandText = "select count(*) from authors";
                var count = (int)command.ExecuteScalar();
                return count;
            });
        }

        public List<Author> Search(string text)
        {
            return ExecuteCommand(command =>
           {
               command.CommandText = $"select * from authors where name like '%{text}%' or biography like '%{text}%'";
               var result = new List<Author>();
               var reader = command.ExecuteReader();
               while (reader.Read())
               {
                   var author = new Author()
                   {
                       Id = (string)reader["id"],
                       Name = (string)reader["name"],
                       Biography = (string)reader["biography"],
                       Email = reader["email"].ToString(),
                       Photo = reader["photo"].ToString()
                   };

                   result.Add(author);
               }

               return result;
           });
        }

        public int AddAuthor(Author author)
        {
            return ExecuteCommand(command =>
            {
                command.CommandText = $"insert into authors(id,name,biography,photo,email) " +
                              $"values('{author.Id}','{author.Name}','{author.Biography}','{author.Photo}','{author.Email}')";
                if (author.Name == null || author.Biography == null)
                    throw new InvalidDataException("Name cannot be empty");
                var addCount = command.ExecuteNonQuery();
                return addCount;
            });

            //catch (SqlException ex)
            //{
            //    var expectedMessage = "Violation of PRIMARY KEY constraint";
            //    var expectedMessage2 = "The duplicate key value";

            //    if (ex.Message.Contains(expectedMessage) && ex.Message.Contains(expectedMessage2))
            //        throw new DuplicateIdException<string>($"Duplicate Author Id {author.Id}") { Id = author.Id };
            //    else
            //        throw;
            //}
        }

        public int RemoveAuthor(string id)
        {
            return ExecuteCommand(command =>
            {
                command.CommandText = $"delete from authors where id='{id}'";
                var deleteCount = command.ExecuteNonQuery();
                if (deleteCount == 0)
                    throw new InvalidIdException<string>() { Id = id };
                return deleteCount;
            });
        }
    }
}