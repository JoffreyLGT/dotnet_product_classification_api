using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API;

/// <summary>
/// Bridge between Entity Framework and Database.
/// Provides access to the database tables.
/// </summary>
public class AppDbContext:IdentityDbContext<User, Role, string>
{
    
    private readonly IConfigurationSection _seededAdminSection;
    private readonly IConfigurationSection _seededUserSection;
    
    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="options">db context options</param>
    /// <param name="configuration">app configuration</param>
    public AppDbContext(DbContextOptions options, IConfiguration configuration):base(options)
    {
        _seededAdminSection = configuration.GetSection("SeededAdmin");
        _seededUserSection = configuration.GetSection("SeededUser");
    }
    
    /// <summary>
    /// Seed database if it is empty.
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Generate roles and insert them in database
        var adminRole = GetAdminRole();
        var userRole = GetUserRole();
        builder.Entity<Role>().HasData(adminRole, userRole);
        // Generate the first users and insert them in database
        var adminUser = GenerateAdminUser();
        var standardUser = GenerateStandardUser();
        builder.Entity<User>().HasData(adminUser, standardUser);
        // Insert user roles in database
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            },
            new IdentityUserRole<string>
            {
                UserId = standardUser.Id,
                RoleId = adminRole.Id
            });
    }

    
    /// <summary>
    /// Generate the first admin user based on secret data.
    /// </summary>
    /// <returns></returns>
    private User GenerateAdminUser()
    {
        var passwordHasher = new PasswordHasher<User>();
        return new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = _seededAdminSection["UserName"],
            NormalizedUserName = _seededAdminSection["UserName"]!.ToUpperInvariant(),
            Email = _seededAdminSection["Email"],
            NormalizedEmail = _seededAdminSection["Email"]!.ToUpperInvariant(),
            EmailConfirmed = true,
            PasswordHash = passwordHasher.HashPassword(null!, _seededAdminSection["Password"]!),
        };
    }
    
    /// <summary>
    /// Generate the first standard user based on secret data.
    /// </summary>
    /// <returns></returns>
    private User GenerateStandardUser()
    {
        var passwordHasher = new PasswordHasher<User>();
        return new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = _seededUserSection["UserName"],
            NormalizedUserName = _seededUserSection["UserName"]!.ToUpperInvariant(),
            Email = _seededUserSection["Email"],
            NormalizedEmail = _seededUserSection["Email"]!.ToUpperInvariant(),
            EmailConfirmed = true,
            PasswordHash = passwordHasher.HashPassword(null!, _seededUserSection["Password"]!),
        };
    }
    
    /// <summary>
    /// Create a Role object for the admin role.
    /// </summary>
    /// <returns>admin role object</returns>
    private Role GetAdminRole()
    {
        return new Role
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Administrator role for the user managing the application."
            };
    }
    
    /// <summary>
    /// Create a Role object for the user role.
    /// </summary>
    /// <returns>user role object</returns>
    private Role GetUserRole()
    {
        return new Role
            {
                Id = Guid.NewGuid().ToString(),
                Name = "User",
                NormalizedName = "USER",
                Description = "Basic user role."
            };
    }
    
    /// <summary>
    /// Access to the Products table.
    /// </summary>
    public DbSet<Product> Products { get; init; }

}