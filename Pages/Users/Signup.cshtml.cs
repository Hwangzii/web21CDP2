using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BCCM1.Pages.Users
{
    public class SignupModel : PageModel
    {
        public UsersInfo userInfo = new UsersInfo();
        public string errorMessage = ""; //hiển thị thông báo lỗi khi người dùng ko nhập đủ thông tin 

        public void OnGet()
        {
        }
    }
}
