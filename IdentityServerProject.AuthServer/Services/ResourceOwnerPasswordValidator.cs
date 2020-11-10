using IdentityModel;
using IdentityServer4.Validation;
using IdentityServerProject.AuthServer.Repository;
using System.Threading.Tasks;

namespace IdentityServerProject.AuthServer.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ICustomUserRepository _customUserRepository;
        public ResourceOwnerPasswordValidator(ICustomUserRepository customUserRepository)
        {
            _customUserRepository = customUserRepository;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var isUser = await _customUserRepository.Validate(context.UserName, context.Password); //buradaki username benim email alanıma denk geliyor
            if(isUser) //true ise
            {
                var user = _customUserRepository.FindByEmail(context.UserName); //kullanıcının email adresini ver bana

                context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password); //sabitlerden password kullandım. Resource ownerin diger adı password pwd dedir.
            }
        }
    }
}
