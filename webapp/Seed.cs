using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using webapp.Entities;


namespace webapp;

public class Seed : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Seed> _logger;
    public UserManager<AppUser> _userM { get; private set; }
    public RoleManager<IdentityRole<Guid>> _roleM { get; private set; }

    public Seed(IServiceProvider serviceProvider, ILogger<Seed> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        _userM = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        _roleM = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        if(await _roleM.FindByNameAsync("Admin") == null)
        {
            var roleResult = await _roleM.CreateAsync(new IdentityRole<Guid>("Admin"));
            if(roleResult.Succeeded)
            {
                _logger.LogInformation("Role Admin is created successfully.");
            }
            else
            {
                _logger.LogError("Role Admin is created successfully.", roleResult.Errors);
            }
        }
        if(await _roleM.FindByNameAsync("SuperAdmin") == null)
        {
            var roleResult = await _roleM.CreateAsync(new IdentityRole<Guid>("SuperAdmin"));
            if(roleResult.Succeeded)
            {
                _logger.LogInformation("Role SuperAdmin is created successfully.");
            }
            else
            {
                _logger.LogError("Role SuperAdmin is created successfully.", roleResult.Errors);
            }
        }
        if(await _userM.FindByNameAsync("superadmin") == null)
        {
            var newUser = new AppUser()
            {
                UserName = "superadmin",
                FullName = "superadmin",
                Email = "superadmin@admin.uz",
                JoinedAt = DateTimeOffset.UtcNow,
                Roles = "Admin, SuperAdmin",
                Password = "12345"
            };
            var identityResult = await _userM.CreateAsync(newUser, "12345");
            if(identityResult.Succeeded)
            {
                var roleResult = await _userM.AddToRoleAsync(await _userM.FindByNameAsync("superadmin"), "SuperAdmin");
                if(roleResult.Succeeded)
                {
                    _logger.LogInformation("User with name superadmin is added to role SuperAdmin successfully.");
                }
                else
                {
                    _logger.LogError("Error in adding user with name superadmin to role SuperAdmin", roleResult.Errors);
                }
                roleResult = await _userM.AddToRoleAsync(await _userM.FindByNameAsync("superadmin"), "Admin");
                if(roleResult.Succeeded)
                {
                    _logger.LogInformation("User with name superadmin is added to role Admin successfully.");
                }
                else
                {
                    _logger.LogError("Error in adding user with name superadmin to role Admin", roleResult.Errors);
                }
            }
            else
            {
                _logger.LogError("User with name superadmin is failed to be created", identityResult.Errors);
            }
        }
        if(await _userM.FindByNameAsync("admin") == null)
        {
            var newUser = new AppUser()
            {
                UserName = "admin",
                FullName = "admin",
                Email = "admin@admin.uz",
                JoinedAt = DateTimeOffset.UtcNow,
                Roles = "Admin",
                Password = "12345"
            };
            var identityResult = await _userM.CreateAsync(newUser, "12345");
            if(identityResult.Succeeded)
            {
                var roleResult = await _userM.AddToRoleAsync(await _userM.FindByNameAsync("admin"), "Admin");
                if(identityResult.Succeeded)
                {
                    _logger.LogInformation("User with name admin is added to role Admin successfully.");
                }
                else
                {
                    _logger.LogError("Error in adding user with name admin to role Admin", roleResult.Errors);
                }
            }
            else
            {
                _logger.LogError("User with name admin is failed to be created", identityResult.Errors);
            }
        }
    }
}