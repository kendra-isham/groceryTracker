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
        public static void SQLTestConnection(string dataSource, string sqlUsername, string sqlPassword, string groceryDB) {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.DataSource = dataSource;
                    builder.UserID = sqlUsername;
                    builder.Password = sqlPassword;
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
