using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServerProject.AuthServer.Repository;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServerProject.AuthServer.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly ICustomUserRepository _customUserRepository;
        public CustomProfileService(ICustomUserRepository customUserRepository)
        {
            _customUserRepository = customUserRepository;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subId = context.Subject.GetSubjectId();
            var user = await _customUserRepository.FindById(int.Parse(subId));

            var claims = new List<Claim>()
            {
                new Claim("email",user.Email), //sabitler oldugu için kullandım olmasaydı kendim yazardım. Best practice için uygun olanı sabit kullanmak
                new Claim("name",user.UserName),
                new Claim("city",user.City)
            };

            if (user.Id == 1)
                claims.Add(new Claim("role", "admin"));
            else
                claims.Add(new Claim("role", "customer"));

            context.AddRequestedClaims(claims);
            //context.IssuedClaims = claims; // yayınlacak claimler. token içerisinde role ve city degerlerini görmek istersem kullanırım. Fakat tokenin boyutu büyür gerek yok bence. Kullanıcıyla alakalı bilgileri userendpointinden alırım.
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = await _customUserRepository.FindById(int.Parse(userId));

            context.IsActive = user != null ? true : false;
        }
    }
}
