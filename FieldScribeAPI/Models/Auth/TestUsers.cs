using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace FieldScribeAPI.Models
{
    public class TestUsers
    {
        private static readonly RegisterForm[] testusers = new[]
{
            new RegisterForm
            {
                Email = "idelle.woodham@gmail.com",
                FirstName = "Idelle",
                LastName = "Woodham",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "ileen.dudley@gmail.com",
                FirstName = "Ileen",
                LastName = "Dudley",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "benson.jinks@gmail.com",
                FirstName = "Benson",
                LastName = "Jinks",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "lamar.maynard@gmail.com",
                FirstName = "Lamar",
                LastName = "Maynard",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "zhi.liu@gmail.com",
                FirstName = "Zhi",
                LastName = "Liu",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "shyla.townsend@gmail.com",
                FirstName = "Shyla",
                LastName = "Townsend",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "guanyu.chu@gmail.com",
                FirstName = "Guanyu",
                LastName = "Chu",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "ajith.chaudhari@gmail.com",
                FirstName = "Ajith",
                LastName = "Chaudhari",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "lara.hardy@gmail.com",
                FirstName = "Lara",
                LastName = "Hardy",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "pascal.sneijers@gmail.com",
                FirstName = "Pascal",
                LastName = "Sneijers",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "simon.wirner@gmail.com",
                FirstName = "Simon",
                LastName = "Wirner",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "carola.piovene@gmail.com",
                FirstName = "Carola",
                LastName = "Piovene",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "shavonne.mckay@gmail.com",
                FirstName = "Shavonne",
                LastName = "McKay",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "shamus.hyland@gmail.com",
                FirstName = "Shamus",
                LastName = "Hyland",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "jose.ventura@gmail.com",
                FirstName = "Jose",
                LastName = "Ventura",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "angelina.campos@gmail.com",
                FirstName = "Angelina",
                LastName = "Campos",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "nneka.okafor@gmail.com",
                FirstName = "Nneka",
                LastName = "Okafor",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "olwin.merrick@gmail.com",
                FirstName = "Olwin",
                LastName = "Merrick",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "premek.kostelecky@gmail.com",
                FirstName = "Premek",
                LastName = "Kostelecky",
                Password = "Password123!!!",
            },

            new RegisterForm
            {
                Email = "omiros.metaxas@gmail.com",
                FirstName = "Omiros",
                LastName = "Metaxas",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "ibrahim.ajam@gmail.com",
                FirstName = "Ibrahim",
                LastName = "Ajam",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "duff.macdougall@gmail.com",
                FirstName = "Duff",
                LastName = "MacDougall",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "sunan.bunnag@gmail.com",
                FirstName = "Sunan",
                LastName = "Bunnag",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "jessica.anthonsen@gmail.com",
                FirstName = "Jessica",
                LastName = "Anthonsen",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "yuliy.aleksandrov@gmail.com",
                FirstName = "Yuliy",
                LastName = "Aleksandrov",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "leila.khoroushi@gmail.com",
                FirstName = "Leila",
                LastName = "Khoroushi",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "howie.vance@gmail.com",
                FirstName = "Howie",
                LastName = "Vance",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "edmond.perrin@gmail.com",
                FirstName = "Edmond",
                LastName = "Perrin",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "sandra.omdahl@gmail.com",
                FirstName = "Sandra",
                LastName = "Omdahl",
                Password = "Password123!!!",
            },
            new RegisterForm
            {
                Email = "jeong.song@gmail.com",
                FirstName = "Jeong-Hun",
                LastName = "Song",
                Password = "Password123!!!",
            }
        };


        public static async Task CreateTestUsers(UserManager<UserEntity> userManager)
        {
            foreach (RegisterForm rf in testusers)
            {
                var user = new UserEntity
                {
                    Email = rf.Email,
                    UserName = rf.Email,
                    FirstName = rf.FirstName,
                    LastName = rf.LastName,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                if (await userManager.FindByNameAsync(rf.Email) == null)
                {
                    var create = await userManager.CreateAsync(user, rf.Password);

                    if (!create.Succeeded)
                    {
                        throw new Exception("Failed to create user");
                    }

                    await userManager.AddToRoleAsync(user, "Scribe");
                }
            }
        }
    }
}
