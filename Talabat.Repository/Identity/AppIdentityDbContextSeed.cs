using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public async static Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            // Seed Default User
            if(userManager.Users.Count() == 0)
            {
                var user = new AppUser
                {
                    DisplayName = "Mohammed",
                    Email = "Mohammed.Hassan@gmail.com",
                    UserName = "Mohammed.Hassan" ,
                    PhoneNumber = "01000000000",
                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
