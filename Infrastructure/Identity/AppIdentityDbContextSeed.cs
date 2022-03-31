using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user=new AppUser
                {
                    DisplayName="Amjath",
                    Email="aamjathmohd@gmail.com",
                    UserName="aamjathmohd",
                    Address=new Address
                    {
                        FirstName="Amjath",
                        LastName="Muhammed",
                        Street="6thmile",
                        City="Kalpetta",
                        State="Kerala",
                        ZipCode="673575"
                    }
                };
                await userManager.CreateAsync(user,"Bystander@123");
            }
            
        }
    }
}