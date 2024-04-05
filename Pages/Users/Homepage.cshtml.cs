using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BCCM1.Pages.Users
{
    public class HomepageModel : PageModel
    {
        public UsersInfo userInfo = new UsersInfo();
        public Accounts account = new Accounts();
        public Transactions transaction = new Transactions();
        public void OnGet()
            {
            string id = Request.Query["id"];
            try
                {
                    string connectionString = "Data Source = HOANGPHI;Initial Catalog=HFINANCE01;" + "Integrated Security=True;Pooling=False;TrustServerCertificate=True";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "Select * from Users where userId=@id";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                        command.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    userInfo.id = "" + reader.GetInt32(0);
                                    userInfo.email = reader.GetString(1);
                                    userInfo.pass = reader.GetString(2);
                                    userInfo.fName = reader.GetString(3);
                                    userInfo.lName = reader.GetString(4);
                                }
                            }
                        }

                        string sqlBalance = "Select Balance from Accounts where userId=@id";
                        using (SqlCommand command = new SqlCommand(sqlBalance, connection))
                        {
                            command.Parameters.AddWithValue("id", id);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    account.balance = reader.GetDecimal(0).ToString();
                                }
                            }
                        }

                        string sqlIncome = "SELECT SUM(amount) AS TotalIncome FROM Transactions WHERE Type = 'Income' AND userId=@id";
                        using (SqlCommand command = new SqlCommand(sqlIncome, connection))
                        {
                            command.Parameters.AddWithValue("id", id);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    transaction.amountIncome = reader.GetDecimal(0).ToString();
                                }
                            }
                        }

                        string sqlExp = "SELECT SUM(amount) AS TotalExpense FROM Transactions WHERE Type = 'Expense' AND userId=@id";
                        using (SqlCommand command = new SqlCommand(sqlExp, connection))
                        {
                            command.Parameters.AddWithValue("id", id);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    transaction.amountExp = reader.GetDecimal(0).ToString();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
    }
}
