using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using webapp;
using webapp.Core.IConfiguration;
using webapp.Data;
using webapp.Entities;

var builder = WebApplication.CreateBuilder(args);

// Service Container
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("NpgSqlConnection")));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Admin"));
});

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options => 
    {
        options.Conventions.AuthorizeFolder("/Categories", "RequireAdministratorRole");
        options.Conventions.AuthorizePage("/Expenses/Edit", "RequireAdministratorRole");
        options.Conventions.AuthorizePage("/Expenses/Delete", "RequireAdministratorRole");
        options.Conventions.AuthorizePage("/Expenses/Create");
        options.Conventions.AuthorizePage("/Expenses/Index");
    });

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddHostedService<Seed>();

// Middlewares
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // app.UseExceptionHandler("/Error");
    app.UseDeveloperExceptionPage();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();