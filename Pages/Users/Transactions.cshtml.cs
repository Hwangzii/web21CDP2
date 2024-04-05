using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BCCM1.Pages.Users
{
    public class TransactionsModel : PageModel
    {
        public UsersInfo userInfo = new UsersInfo();
        public Accounts account = new Accounts();
        public Transactions transInfo = new Transactions();
        public string errorMessage = "";

        public List<Transactions> listTrans = new List<Transactions>();

        // thêm trường userId để lưu trữ ID của người dùng
        public string userId = "";

        public void OnGet()
        {
            String id = Request.Query["id"];
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
                            }
                        }
                    }

                    string sqlTrans = "Select transactionId,amount,Type,category,transDate,description from Transactions where userId=@id";
                    using (SqlCommand command = new SqlCommand(sqlTrans, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                 Transactions transaction = new Transactions();
                                transaction.transId = "" + reader.GetInt32(0);
                                transaction.amount = reader.GetDecimal(1).ToString();
                                transaction.type = reader.GetString(2);
                                transaction.category = reader.GetString(3);
                                transaction.transDate = reader.GetDateTime(4).ToString();
                                transaction.description = reader.GetString(5);
                                listTrans.Add(transaction);
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

        public void OnPost()
        {
            // lấy userId từ form
            string id = Request.Form["userId"];

            // kiểm tra xem userId có được gửi từ form hay không
            if (string.IsNullOrEmpty(id))
            {
                errorMessage = "User ID is required";
                return;
            }

            // lấy thông tin người dùng từ cơ sở dữ liệu
            try
            {
                string connectionString = "Data Source = HOANGPHI;Initial Catalog=HFINANCE01;" + "Integrated Security=True;Pooling=False;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Users WHERE userId=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id",id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // gán thông tin người dùng vào dối tượng userInfo
                                userInfo.id = id;

                                // cần xử lý thông tin này tùy thuộc vào yêu cầu của ứng dụng
                                transInfo.amount = "";
                                transInfo.type = "";
                                transInfo.category = "";
                                transInfo.transDate = "";
                                transInfo.description = "";
                            }
                            else
                            {
                                errorMessage = "User not found";
                                return;
                            }
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine (errorMessage); 
                throw;
            }




            

            if (!string.IsNullOrEmpty(Request.Form["amount"]))
            {
                // kiểm tra các trường thông tin giao dịch có được điền đầy đủ hay không
                if (string.IsNullOrEmpty(Request.Form["amount"]) ||
                    string.IsNullOrEmpty(Request.Form["type"]) ||
                    string.IsNullOrEmpty(Request.Form["category"]) ||
                    string.IsNullOrEmpty(Request.Form["tranDate"]) ||
                    string.IsNullOrEmpty(Request.Form["description"]))
                {
                    errorMessage = "All fieds are required";
                    return;
                }

                // lưu thông tin giao dịch vào cơ sở dữ liệu
                try
                {
                    string connectionString = "Data Source = HOANGPHI;Initial Catalog=HFINANCE01;" + "Integrated Security=True;Pooling=False;TrustServerCertificate=True";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "INSERT INTO Transactions" +
                            "(userId,amount,type,category,transDate,description) VALUES" +
                            "(@userId,@amount,@Type,@category,@transDate,@description);";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@userId", transInfo.userId);
                            command.Parameters.AddWithValue("@amount", transInfo.amount);
                            command.Parameters.AddWithValue("@type", transInfo.type);
                            command.Parameters.AddWithValue("@category", transInfo.category);
                            command.Parameters.AddWithValue("@transDate", transInfo.transDate);
                            command.Parameters.AddWithValue("@description", transInfo.description);

                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }

                // clear info for next input
                transInfo.amount = "";
                transInfo.type = "";
                transInfo.category = "";
                transInfo.transDate = "";
                transInfo.description = "";

                // ssau khi them thanh cong 1 row thi redirect ve trang /transactions/index de hien thi danh sach chi tiet transactions
                Response.Redirect("");
            }

        }
    }
}
