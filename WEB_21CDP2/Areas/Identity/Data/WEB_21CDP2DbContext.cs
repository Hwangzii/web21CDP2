using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WEB_21CDP2.Areas.Identity.Data;

namespace WEB_21CDP2.Data;

public class WEB_21CDP2DbContext : IdentityDbContext<ApplicationUser>
{
    public WEB_21CDP2DbContext(DbContextOptions<WEB_21CDP2DbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Tùy chỉnh mô hình ASP.NET Identity và ghi đè các giá trị mặc định nếu cần.
        // Ví dụ, bạn có thể đổi tên bảng ASP.NET Identity và nhiều hơn nữa.
        // Thêm các tùy chỉnh của bạn sau khi gọi base.OnModelCreating(builder);
    }
}
