using Microsoft.AspNetCore.Identity;
using queue.Data;
using queue.Entities;

namespace queue;

public class Seed : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Seed> _logger;

    public Seed(IServiceProvider serviceProvider, ILogger<Seed> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>(); 
        var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<User>>();

        var user = new User()
        {
            Email = "superadmin@alisher.uz"
        };
        if(!context.Users.Any())
        {
            await signInManager.PasswordSignInAsync(user, "123456", false, false);
        }
    }
}