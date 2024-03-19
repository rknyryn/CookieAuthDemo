using Microsoft.AspNetCore.Identity;

namespace CookieAuthDemo;

public static class ApplicationBuilderExtension
{
    public static async Task<WebApplication> BuildAndSetup(this WebApplicationBuilder builder)
    {
        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole
            {
                Id = "8bb35973-beeb-4f8e-b8ca-7d5e112e8dbd",
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "8bb35973-beeb-4f8e-b8ca-7d5e112e8dbd"
            });

            IdentityUser user = new() { UserName = "test@mail.com", Email = "test@mail.com" };
            await userManager.CreateAsync(user, "password");
            await userManager.AddToRoleAsync(user, "Admin");
        }

        return app;
    }
}
