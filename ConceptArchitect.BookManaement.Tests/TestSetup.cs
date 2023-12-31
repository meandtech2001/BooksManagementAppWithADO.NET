﻿using ConceptArchitect.BookManagement;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptArchitect.BookManaement.Tests
{
    internal class TestSetup
    {
        //private IConnectionsFactory connectionFactory;
        private Func<IDbConnection> connectionFactory;

        public string ConnectionString { get; set; }

        //public TestSetup(IConnectionsFactory factory)
        //{
        //    connectionFactory = factory;
        //}

        public void SetUpAuthors(params Author[] authors)
        {
            //ConnectionsFactoryDelegate connectionCreator = null;
            IDbConnection connection = null;

            try
            {
                //connection.ConnectionString = ConnectionString;

                connection = new SqlConnection(ConnectionString);
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = "drop table if exists authors;";
                command.ExecuteNonQuery();

                command.CommandText = "create table AUTHORS(" +
                    "id varchar(50) primary key, " +
                    "name varchar(50) not null, " +
                    "biography varchar(2000), " +
                    "photo varchar(256), " +
                    "email varchar(100) " +
                    ");";

                command.ExecuteNonQuery();

                foreach (var author in authors)
                {
                    var qry = $"insert into authors(id,name,biography,photo,email) " +
                              $"values('{author.Id}','{author.Name}','{author.Biography}','{author.Photo}','{author.Email}')";

                    command.CommandText = qry;
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }
    }
}