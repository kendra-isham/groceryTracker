using System;
using Microsoft.Data.SqlClient;

// TODO 
// https://docs.microsoft.com/en-us/azure/azure-sql/database/connect-query-dotnet-visual-studio
// should rename method 
// have this be an insert method 
// create a query method as well to display data to the user 
// add column to table for user

namespace GroceryTracker
{
    class DBConnection
    {

        static string dataSource = Environment.GetEnvironmentVariable("DATASOURCE");
        static string SQLUsername = Environment.GetEnvironmentVariable("AZURE_SQL_USERNAME");
        static string SQLPassword = Environment.GetEnvironmentVariable("AZURE_SQL_PASSWORD");
        static string groceryDB = "grocery";
        public static void SQLTestConnection() {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.DataSource = dataSource;
                    builder.UserID = SQLUsername;
                    builder.Password = SQLPassword;
                    builder.InitialCatalog = groceryDB;
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    Console.WriteLine("\nQuery data example:");
                    Console.WriteLine("=========================================\n");

                    String sql = "SELECT [Product Name] FROM [dbo].[receipts]";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0}", reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
