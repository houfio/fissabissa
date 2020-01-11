using FissaBissa.Entities;
using Microsoft.AspNetCore.Identity;

namespace FissaBissa.Utilities
{
    public static class IdentitySeeder
    {
        public static void Seed(RoleManager<RoleEntity> roleManager)
        {
            var roles = new[]
            {
                "Admin"
            };

            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    roleManager.CreateAsync(new RoleEntity
                    {
                        Name = role
                    }).Wait();
                }
            }
        }
    }
}
