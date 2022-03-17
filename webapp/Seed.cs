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
        if(await _userM.FindByNameAsync("admin") == null)
        {
            var newUser = new AppUser()
            {
                UserName = "admin",
                FullName = "admin"
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