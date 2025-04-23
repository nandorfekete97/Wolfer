using Microsoft.AspNetCore.Identity;

namespace Wolfer.Services.Authentication;

public class AuthenticationSeeder
{
    private RoleManager<IdentityRole> _roleManager;

    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }
    
    public void AddRoles()
    {
        var tAdmin = CreateAdminRole(_roleManager);
        tAdmin.Wait();

        var tUser = CreateUserRole(_roleManager);
        tUser.Wait();
    }

    private async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole("Admin")); //The role string should better be stored as a constant or a value in appsettings
    }

    async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole("User")); //The role string should better be stored as a constant or a value in appsettings
    }
}