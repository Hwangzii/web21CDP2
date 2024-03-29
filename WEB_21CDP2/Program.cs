using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEB_21CDP2.Areas.Identity.Data;
using WEB_21CDP2.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WEB_21CDP2DbContextConnection") ?? throw new InvalidOperationException("Không tìm thấy chuỗi kết nối 'WEB_21CDP2DbContextConnection'.");

builder.Services.AddDbContext<WEB_21CDP2DbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<WEB_21CDP2DbContext>();

// Thêm dịch vụ vào container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
});

var app = builder.Build();

// Cấu hình pipeline yêu cầu HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
