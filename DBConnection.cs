using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using Microsoft.Data.SqlClient;

namespace GroceryTracker
{
    class DBConnection
    {

        static string dataSource = Environment.GetEnvironmentVariable("DATASOURCE");
        static string SQLUsername = Environment.GetEnvironmentVariable("AZURE_SQL_USERNAME");
        static string SQLPassword = Environment.GetEnvironmentVariable("AZURE_SQL_PASSWORD");
        static string DB = "grocery";
        static string table = "dbo.receipts";
        
        // TODO: export db as csv
        public static void SQLInsert(List<Product> receipt)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = dataSource;
                builder.UserID = SQLUsername;
                builder.Password = SQLPassword;
                builder.InitialCatalog = DB;
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    //sql statement here 
                    string insert = $"INSERT INTO {table}([Purchase Date], [Product Number], [Product Name], [Product Price], [Uncleaned Data], [Username])" +
                        $"VALUES(@PurchaseDate, @ProductNumber, @ProductName, @ProductPrice, @UncleanedData, @Username)";

                    using (SqlCommand command = new SqlCommand(insert, connection))
                    {

                        command.Parameters.Add("@PurchaseDate", SqlDbType.Date);
                        command.Parameters.Add("@ProductNumber", SqlDbType.Int);
                        command.Parameters.Add("@ProductName", SqlDbType.Text);
                        command.Parameters.Add("@ProductPrice", SqlDbType.Money);
                        command.Parameters.Add("@UncleanedData", SqlDbType.Text);
                        command.Parameters.Add("@Username", SqlDbType.Text);

                        connection.Open();
                        for(int i = 0; i < receipt.Count; i++) { 
                            command.Parameters["@PurchaseDate"].Value = DateTime.Parse(receipt[i].PurchaseDate);
                            command.Parameters["@ProductNumber"].Value = receipt[i].ProductNumber;
                            command.Parameters["@ProductName"].Value = receipt[i].ProductName;
                            command.Parameters["@ProductPrice"].Value = receipt[i].ProductPrice;
                            command.Parameters["@UncleanedData"].Value = receipt[i].PreCleanedText;
                            command.Parameters["@Username"].Value = receipt[i].ResponsibleParty.Name;

                            command.ExecuteNonQuery();
                        }
                        Console.WriteLine("Inserted Receipt Data to database\n\n");
                        connection.Close();
                    }
                }
            }
            catch
            {
                Console.WriteLine("There was an error logging receipt data to the database.");
                Driver.Main();
            }
        }
    }
}
