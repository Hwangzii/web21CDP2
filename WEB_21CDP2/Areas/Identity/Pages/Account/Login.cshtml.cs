﻿// Được cấp phép cho .NET Foundation dưới một hoặc nhiều thỏa thuận.
// .NET Foundation cấp phép tệp này cho bạn dưới giấy phép MIT.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WEB_21CDP2.Areas.Identity.Data;

namespace WEB_21CDP2.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
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
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     API này hỗ trợ cơ sở hạ tầng UI mặc định của ASP.NET Core Identity và không được dùng
        ///     trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các phiên bản sau.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     API này hỗ trợ cơ sở hạ tầng UI mặc định của ASP.NET Core Identity và không được dùng
        ///     trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các phiên bản sau.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     API này hỗ trợ cơ sở hạ tầng UI mặc định của ASP.NET Core Identity và không được dùng
        ///     trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các phiên bản sau.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     API này hỗ trợ cơ sở hạ tầng UI mặc định của ASP.NET Core Identity và không được dùng
            ///     trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các phiên bản sau.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     API này hỗ trợ cơ sở hạ tầng UI mặc định của ASP.NET Core Identity và không được dùng
            ///     trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các phiên bản sau.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     API này hỗ trợ cơ sở hạ tầng UI mặc định của ASP.NET Core Identity và không được dùng
            ///     trực tiếp từ mã của bạn. API này có thể thay đổi hoặc bị loại bỏ trong các phiên bản sau.
            /// </summary>
            [Display(Name = "Ghi nhớ?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/");
            }
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Xóa cookie bên ngoài hiện tại để đảm bảo quá trình đăng nhập sạch sẽ
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // Điều này không tính các lần đăng nhập không thành công đến việc khoá tài khoản
                // Để bật tính năng khoá tài khoản khi đăng nhập sai, đặt lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Người dùng đã đăng nhập.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Tài khoản người dùng bị khoá.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Cố gắng đăng nhập không hợp lệ.");
                    return Page();
                }
            }

            // Nếu đã đến đây, có lỗi xảy ra, hiển thị lại biểu mẫu
            return Page();
        }
    }
}
