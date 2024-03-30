using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;

namespace WebApplication1.Pages.clients
{
    public class LoginModel : PageModel
    {
        public Account Account = new Account();
        public string errorMessages = "";

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            Account.user_name = Request.Form["user_name"];
            Account.password = Request.Form["password"];
            if (string.IsNullOrEmpty(Account.user_name) || string.IsNullOrEmpty(Account.password))
            {
                errorMessages = "Yêu cầu nhập đầy đủ thông tin!!!";
                return Page();
            }

            try
            {
                string connectionString = "Data Source=HOANGPHI;Initial Catalog=incomeManagement;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO taikhoan_TB (user_name,password) VALUES (@taikhoan, @matkhau);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@taikhoan", Account.user_name);
                        command.Parameters.AddWithValue("@matkhau", Account.password);

                        command.ExecuteNonQuery();
                    }
                }

                // chuyen huong ve tran chu sau khi dang nhap thanh cong
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            finally
            {
                // Xóa các trường sau khi gửi thành công hoặc thất bại
                Account.user_name = "";
                Account.password = "";
            }
        }
    }

    public class Account
    {
        public int id { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
    }
}
