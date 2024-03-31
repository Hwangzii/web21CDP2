using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace WebApplication1.Pages.clients
{
    public class CreateModel : PageModel
    {

        public clientInfo clientInfo = new clientInfo();
        public string errorMessage = ""; //hien thi thong bao loi khi nguoi dung ko nha du thong tin
        public void OnGet()
        {
        }

        public void OnPost() // xu ly http method: POST xay ra khi nguoi dung click Submit button 
        {
            // property request bien dien cac thong tin do user gui yeu cau (request) den Server
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["name"];
            clientInfo.address = Request.Form["address"];
            // check all files are filled
            if (string.IsNullOrEmpty(clientInfo.name)||
                string.IsNullOrEmpty(clientInfo.email)||
                string.IsNullOrEmpty(clientInfo.phone)||
                string.IsNullOrEmpty(clientInfo.address))
            {
                errorMessage = "All fileds are required!!!";
                return;
            }

            //if ok, save new client to database
            try
            {
                string connectionString = "Data Source=DIUHOE;Initial Catalog=WEB21CDP2;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Clients" +
                        "(name,email,phone,address) VALUES" +
                        "(@name,@email,@phone,@address);";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();

                    }
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            //clear info for next input
            clientInfo.name="";
            clientInfo.phone="";
            clientInfo.email="";
            clientInfo.address="";

            // sau khi them thanh cong 1 row thi redirect ve trang /clients/index de hien thi danh sach chi tiet clients
            Response.Redirect("/Clients"); 
        }
    }
}
