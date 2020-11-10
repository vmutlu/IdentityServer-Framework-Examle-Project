using IdentityServerProject.AuthServer.Data;
using IdentityServerProject.AuthServer.Models;
using IdentityServerProject.AuthServer.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IdentityServerProject.AuthServer
{
    public class CustomUserService : ICustomUserRepository
    {
        private readonly CustomDbContext _customDbContext;
        public CustomUserService(CustomDbContext customDbContext)
        {
            _customDbContext = customDbContext;
        }
        public async Task<CustomUser> FindByEmail(string email)
        {
            return await _customDbContext.CustomUsers.FirstOrDefaultAsync(i => i.Email == email).ConfigureAwait(false);
        }

        public async Task<CustomUser> FindById(int id)
        {
            return await _customDbContext.CustomUsers.FindAsync(id).ConfigureAwait(false); ;
        }

        public async Task<bool> Validate(string email, string password)
        {
            return await _customDbContext.CustomUsers.AnyAsync(x=>x.Email == email && x.Password == password).ConfigureAwait(false);
        }
    }
}
