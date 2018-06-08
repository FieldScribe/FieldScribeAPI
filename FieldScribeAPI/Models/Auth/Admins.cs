using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public static class Admins
    {
        private static readonly RegisterForm[] admins = new[]
        {
            new RegisterForm
            {
                Email = "michael.mrkvicka@oit.edu",
                FirstName = "Michael",
                LastName = "Mrkvicka",
                Password = "jp2018MM!!",                
            },
            new RegisterForm
            {
                Email = "alexander.ott@oit.edu",
                FirstName = "Alex",
                LastName = "Ott",
                Password = "jp2018AO!!",
            },
            new RegisterForm
            {
                Email = "john.zimmerman@oit.edu",
                FirstName = "John",
                LastName = "Zimmerman",
                Password = "jp2018JZ!!",
            },
            new RegisterForm
            {
                Email = "ruby.felton@oit.edu",
                FirstName = "Ruby",
                LastName = "Felton",
                Password = "jp2018RF!!",
            },
            new RegisterForm
            {
                Email = "cody.garvin@oit.edu",
                FirstName = "Cody",
                LastName = "Garvin",
                Password = "jp2018CG!!",
            },
            new RegisterForm
            {
                Email = "christian.rhodes@oit.edu",
                FirstName = "Christian",
                LastName = "Rhodes",
                Password = "jp2018CR!!",
            }
        };


        public static async Task CreateAdmins(UserManager<UserEntity> userManager)
        {
            foreach(RegisterForm a in admins)
            {
                var user = new UserEntity
                {
                    Email = a.Email,
                    UserName = a.Email,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                if(await userManager.FindByNameAsync(a.Email) == null)
                {
                    var create = await userManager.CreateAsync(user, a.Password);

                    if(!create.Succeeded)
                    {
                        throw new Exception("Failed to create user");
                    }

                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
