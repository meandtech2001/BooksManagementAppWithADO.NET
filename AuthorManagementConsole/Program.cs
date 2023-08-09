using ConceptArchitect.BookManagement;
using Microsoft.Data.SqlClient;

internal class Program
{
    private static void Main()
    {
        var connectionString = @"Data Source=.;Initial Catalog=BookTestData;Integrated Security=True";
        //var manager = new AuthorManager(new SqlConnectionFactory(connectionString))
        //{
        //    ConnectionString = connectionString
        //};
        var manager = new AuthorManager(() =>
        {
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        });

        foreach (Author author in manager.GetAllAuthors())
            Console.WriteLine(author);
    }
}