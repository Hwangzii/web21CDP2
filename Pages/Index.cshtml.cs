using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BCCM1.Pages
{
    public class UsersInfo
    {
        public string id { get; set; }
        public string email { get; set; }
        public string pass { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string gender { get; set; }
        public string birthDay { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
    }

    public class Accounts
    {
        public string accountId { get; set; }
        public string userId { get; set; }
        public string balance { get; set; }
    }

    public class Transactions
    {
        public string transId { get; set; }
        public string userId { get; set; }
        public string accountId { get; set; }
        public string amount { get; set; }
        public string amountIncome { get; set; }
        public string amountExp {  get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public string transDate { get; set; }
        public string description { get; set; }
    }
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public List<UsersInfo> listUsers = new List<UsersInfo>();
        public void OnGet()
        {
   
        }
    }
}
