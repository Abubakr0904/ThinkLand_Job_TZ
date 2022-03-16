using Microsoft.EntityFrameworkCore;
using webapp.Core.IConfiguration;
using webapp.Data;

var builder = WebApplication.CreateBuilder(args);

// Service Container
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("NpgSqlConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Middlewares
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();