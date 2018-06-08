using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public static class RolesData
    {
        private static readonly string[] roles = new[]
        {
            "Admin",
            "Timer",
            "Scribe"
        };

        public static async Task CreateRoles(RoleManager<UserRoleEntity> roleManager)
        {
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var create = await roleManager.CreateAsync(new UserRoleEntity(role));

                    if (!create.Succeeded)
                    {
                        throw new Exception("Failed to create role");
                    }
                }
            }
        }
    }
}
