using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServerProject.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            //2 api projem olduğu için iki apiresource ihtiyacım var

            return new List<ApiResource>()
            {
                new ApiResource("resource_api1"){ Scopes = { "api1.read", "api1.write", "api1.update" },ApiSecrets = new[]{ new Secret("secretapi1".Sha256()) } }, //api scope izinleri string olarak ekleniyor
                new ApiResource("resource_api2"){ Scopes = { "api2.read", "api2.write", "api2.update" },ApiSecrets = new[]{ new Secret("secretapi2".Sha256()) } }, //böylelikle api resourcem hangi api için hangi izinler var bilecek
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
            };
        }

        //api izinleri(scope) için apiscope kullanacagım
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>()
            {
                new ApiScope("api1.read","API 1 için okuma iznim"),
                new ApiScope("api1.write","API 1 için yazma iznim"),
                new ApiScope("api1.update","API 1 için güncelleme iznim"),

                new ApiScope("api2.read","API 2 için okuma iznim"),
                new ApiScope("api2.write","API 2 için yazma iznim"),
                new ApiScope("api2.update","API 2 için güncelleme iznim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId = "Client1",
                    ClientName = "Client 1 App",
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //kullanıcıyla ilgili işlemim olmadıgı için OAuth 2.0 izin tiplerinden olan en sonuncusu ClientCredentials seçtim. Apiye istek atıp sadece token alcam ben kardeş dicek
                    ClientSecrets = new[]{new Secret("secret".Sha256())}, //hashler geri dönmez yani şifrelenmiş metin tekrar çözülmez
                    AllowedScopes = { "api1.read" } //hangi izne sahip olacak
                },
                new Client()
                {
                    ClientId = "Client2",
                    ClientName = "Client 2 App",
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //kullanıcıyla ilgili işlemim olmadıgı için OAuth 2.0 izin tiplerinden olan en sonuncusu ClientCredentials seçtim. Apiye istek atıp sadece token alcam ben kardeş dicek
                    ClientSecrets = new[]{new Secret("secret".Sha256())}, //hashler geri dönmez yani şifrelenmiş metin tekrar çözülmez
                    AllowedScopes = { "api1.read", "api1.update", "api2.write", "api2.update" } //hangi izne sahip olacak
                },
                new Client()
                {
                    ClientId = "Client1-MVC",
                    RequirePkce = false,
                    ClientName = "Client 1 App UYGULAMASI",
                    ClientSecrets = new[]{new Secret("secret".Sha256())},
                    PostLogoutRedirectUris = new List<string>{"https://localhost:5003/signout-callback-oidc"}, //çıkış yapıldıgında bu uriye git
                    AllowedGrantTypes = GrantTypes.Hybrid, //neden hbrit ? client1 projemin startupunca grant type code id_token yaptıgım için buradaki grant type hibrit olarak belirtmem gerek
                    RedirectUris = new List<string>{"https://localhost:5003/signin-oidc"},
                    AllowedScopes = { IdentityServerConstants.StandardScopes.Email,IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,"api1.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},//sabit değişken bunlar fare ile üzerine gelip bakılabilir. postman ile istek atılıp kontrol edilebilir yani https://localhost:5001/connect/userinfo bu adrese token ile birlikte istek atılıp id ve profil bilgisi görülebilir.
                    AccessTokenLifetime = 7200, //access tokenin yaşam süresini 2 saat yaptım
                    AllowOfflineAccess = true, //reflesh token özelliğini aktif ettim
                    RefreshTokenUsage = TokenUsage.ReUse, //ömrü boyunca birden fazla kullanabileyim diye istersem bir kerede kullanılabilir yaparım
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds, //reflesh tokenin yaşam süresini 60 gün yaptım  
                    RequireConsent = true // facebook ile giriş yaparken şu site şu bilgilerinize erişmek istiyor gibisinde çıkan onay metni ile alakalıdır. True ya çektim artık onay metni gelecek
                },
                new Client()
                {
                    ClientId = "Client2-MVC",
                    RequirePkce = false,
                    ClientName = "Client 2 App UYGULAMASI",
                    ClientSecrets = new[]{new Secret("secret".Sha256())},
                    PostLogoutRedirectUris = new List<string>{"https://localhost:5005/signout-callback-oidc"}, //çıkış yapıldıgında bu uriye git
                    AllowedGrantTypes = GrantTypes.Hybrid, //neden hbrit ? client1 projemin startupunca grant type code id_token yaptıgım için buradaki grant type hibrit olarak belirtmem gerek
                    RedirectUris = new List<string>{"https://localhost:5005/signin-oidc"},
                    AllowedScopes = { IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,"api1.read", "api2.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},//sabit değişken bunlar fare ile üzerine gelip bakılabilir. postman ile istek atılıp kontrol edilebilir yani https://localhost:5001/connect/userinfo bu adrese token ile birlikte istek atılıp id ve profil bilgisi görülebilir.
                    AccessTokenLifetime = 7200, //access tokenin yaşam süresini 2 saat yaptım
                    AllowOfflineAccess = true, //reflesh token özelliğini aktif ettim
                    RefreshTokenUsage = TokenUsage.ReUse, //ömrü boyunca birden fazla kullanabileyim diye istersem bir kerede kullanılabilir yaparım
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds, //reflesh tokenin yaşam süresini 60 gün yaptım  
                    RequireConsent = true // facebook ile giriş yaparken şu site şu bilgilerinize erişmek istiyor gibisinde çıkan onay metni ile alakalıdır. True ya çektim artık onay metni gelecek
                },
                //angular clientini ekliyorum
                new Client()
                {
                    ClientId = "js-client",
                    RequireClientSecret = false,//clint secret isttemiyorum
                    ClientName = "Js Client (Angular)",
                    AllowedScopes = { IdentityServerConstants.StandardScopes.Email,IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,"api1.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles"},
                    RedirectUris = {"http://localhost:4200/callback"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    PostLogoutRedirectUris = {"http://localhost:4200" },//çıkış yaptıgında bu urle gidecek. Neden 4200 ? bir angular uygulaması default olarak 4200 portundan ayaga kalkar
                    AllowedGrantTypes = GrantTypes.Code
                },
                new Client()
                {
                    ClientId = "Client1ResourceOwner",
                    ClientName = "Client 1 App UYGULAMASI",
                    ClientSecrets = new[]{new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials, //iki akışıda desteklesin 
                    AllowedScopes = { IdentityServerConstants.StandardScopes.Email,IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile,"api1.read",IdentityServerConstants.StandardScopes.OfflineAccess,"CountryAndCity","Roles",IdentityServerConstants.LocalApi.ScopeName},//sabit değişken bunlar fare ile üzerine gelip bakılabilir. postman ile istek atılıp kontrol edilebilir yani https://localhost:5001/connect/userinfo bu adrese token ile birlikte istek atılıp id ve profil bilgisi görülebilir.
                    AccessTokenLifetime = 7200, //access tokenin yaşam süresini 2 saat yaptım
                    AllowOfflineAccess = true, //reflesh token özelliğini aktif ettim
                    RefreshTokenUsage = TokenUsage.ReUse, //ömrü boyunca birden fazla kullanabileyim diye istersem bir kerede kullanılabilir yaparım
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds, //reflesh tokenin yaşam süresini 60 gün yaptım  
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.Email(),
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(), //yani kullanıcının profil bilgisinie erişmek istiyorum
                new IdentityResource(){ Name = "CountryAndCity", DisplayName = "Country And City", Description = "Kullanıcı Ülke ve Şehir Bilgisi", UserClaims = new[] {"country","city" } },
                new IdentityResource(){ Name = "Roles", DisplayName="Roles",Description="Kullanıcı Rolleri",UserClaims=new[]{"role" } },
            };
        }

        public static IEnumerable<TestUser> GetTestUsers()
        {
            return new List<TestUser>()
            {
                new TestUser(){SubjectId = "1",Username="veysel_mutlu42@hotmail.com",Password="password",Claims=new List<Claim>()
                {
                    new Claim("given_name","Veysel"),
                    new Claim("family_name","MUTLU"),
                    new Claim("country","Türkiye"),
                    new Claim("city","Konya"),
                    new Claim("role","admin")
                }},
                new TestUser(){SubjectId = "2",Username="vmutlu",Password="password",Claims=new List<Claim>()
                {
                    new Claim("given_name","Veli"),
                    new Claim("family_name","MUTLU"),
                    new Claim("country","Türkiye"),
                    new Claim("city","Ankara"),
                    new Claim("role","customer")
                },}
            };
        }
    }
}
