// Được cấp phép cho .NET Foundation dưới một hoặc nhiều thỏa thuận.
// .NET Foundation cấp phép tệp này cho bạn dưới giấy phép MIT.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using WEB_21CDP2.Areas.Identity.Data;

namespace WEB_21CDP2.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     API này hỗ trợ cơ sở hạ tầng UI mặc định của ASP.NET Core Identity và không được dùng
        ///     trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các phiên bản sau.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     API này hỗ trợ cơ sở hạ tầng UI mặc định của ASP.NET Core Identity và không được dùng
        ///     trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các phiên bản sau.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     API này hỗ trợ cơ sở hạ tầng UI mặc định của ASP.NET Core Identity và không được dùng
        ///     trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các phiên bản sau.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     API này hỗ trợ cơ sở hạ tầng UI mặc định của ASP.NET Core Identity và không được dùng
        ///     trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các phiên bản sau.
        /// </summary>
        public class InputModel
        {
            [DataType(DataType.Text)]
            [Display(Name = "Tên")]
            public string FirstName { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Họ")]
            public string LastName { get; set; }

            /// <summary>
            ///     API này hỗ trợ cơ sở hạ tầng UI mặc định của ASP.NET Core Identity và không được dùng
            ///     trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các phiên bản sau.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     API này hỗ trợ cơ sở hạ tầng UI mặc định của ASP.NET Core Identity và không được dùng
            ///     trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các phiên bản sau.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} và tối đa {1} ký tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu")]
            public string Password { get; set; }

            /// <summary>
            ///     API này hỗ trợ cơ sở hạ tầng UI mặc định của ASP.NET Core Identity và không được dùng
            ///     trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các phiên bản sau.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Xác nhận mật khẩu")]
            [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/");
            }
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Người dùng đã tạo tài khoản mới với mật khẩu.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Xác nhận email của bạn",
                        $"Vui lòng xác nhận tài khoản của bạn bằng cách <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>nhấn vào đây</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Nếu đã đến đây, có lỗi xảy ra, hiển thị lại biểu mẫu
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Không thể tạo một phiên bản của '{nameof(ApplicationUser)}'. " +
                    $"Đảm bảo rằng '{nameof(ApplicationUser)}' không phải là một lớp trừu tượng và có một hàm tạo không tham số, hoặc tùy chọn khác " +
                    $"là ghi đè trang đăng ký trong /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("Giao diện người dùng mặc định yêu cầu một cửa hàng người dùng hỗ trợ email.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
