using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServerProject.AuthServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(LocalApi.PolicyName)] //config dosyasında ApiScope(IdentityServerConstants.LocalApi.ScopeName) olarak eklediğim satırsa ScopeName ile buradaki ScopeName sabitleri aynı
    public class UsersController : ControllerBase
    {
        [HttpPost]
        public IActionResult SignUp()
        {
            return Ok("Çalıştı");
        }
    }
}
