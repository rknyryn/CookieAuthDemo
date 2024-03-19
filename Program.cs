using CookieAuthDemo;
using CookieAuthDemo.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<IdentityDbContext>(c => c.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<IdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    //options.Cookie.Name = "CustomCookieName";
    options.ExpireTimeSpan = TimeSpan.FromDays(10);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(IdentityConstants.ApplicationScheme, policy =>
    {
        policy.RequireAuthenticatedUser()
        .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme);
    });
});

var app = await builder.BuildAndSetup();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapPost("/login", async (SignInManager<IdentityUser> signInManager) =>
{
    await signInManager.PasswordSignInAsync("test@mail.com", "password", false, false);
    return Results.Ok();
});

app.MapGet("/currentUserIdentifier", (HttpContext ctx) =>
{
    return ctx.User.FindFirstValue(ClaimTypes.NameIdentifier);
}).RequireAuthorization(IdentityConstants.ApplicationScheme);

app.MapGet("/userClaims", (ClaimsPrincipal claims) =>
{
    return claims.Claims.Select(c => KeyValuePair.Create(c.Type, c.Value));
}).RequireAuthorization(IdentityConstants.ApplicationScheme);

app.MapGet("/revokeUser", async (SignInManager<IdentityUser> signManager) =>
{
    await signManager.SignOutAsync();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
