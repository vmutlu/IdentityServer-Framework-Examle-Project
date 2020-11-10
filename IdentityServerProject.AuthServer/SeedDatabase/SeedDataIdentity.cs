using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace IdentityServerProject.AuthServer.SeedDatabase
{
    public static class SeedDataIdentity
    {
        public static void SeedData(ConfigurationDbContext configurationDbContext)
        {
            if(!configurationDbContext.Clients.Any())
            {
                foreach (var client in Config.GetClients()) //config sınıfımda yer alan dataları database ekleme işlemini gerçekleştirecek
                {
                    configurationDbContext.Clients.Add(client.ToEntity());
                }
            }

            if (!configurationDbContext.ApiResources.Any())
            {
                foreach (var apiResource in Config.GetApiResources()) 
                {
                    configurationDbContext.ApiResources.Add(apiResource.ToEntity());
                }
            }

            if(!configurationDbContext.ApiScopes.Any())
            {
                foreach (var apiScopes in Config.GetApiScopes())
                {
                    configurationDbContext.ApiScopes.Add(apiScopes.ToEntity());
                }
            }

            //foreach kullanımı -2

            if(!configurationDbContext.IdentityResources.Any())
            {
                Config.GetIdentityResources().ToList().ForEach(identityResource =>
                {
                    configurationDbContext.IdentityResources.Add(identityResource.ToEntity());
                });
            }

            configurationDbContext.SaveChanges();
        }
    }
}
