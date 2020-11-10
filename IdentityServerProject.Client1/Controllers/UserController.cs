using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServerProject.Client1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            //index sayfasında cookie içindeki kullanıcı bilgileri okuyup ekranda gösteriyorum
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("Cookies");

            return RedirectToAction("Index", "Home");
            //await HttpContext.SignOutAsync("oidc"); //yönlendirmeyi bu yapıyor yani çıkış yaptıktan sonra index sayfasına gitmemesi istenirse bura yoruma alınır

        }

        public async Task<IActionResult> GetRefreshToken()
        {
            HttpClient httpClient = new HttpClient();
            var discovery = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");
            //tekrardan signin işlemi mantıgı

            var refleshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken); // ben hali hazırda yazılı olan sabiti kullandım mouse ile sabit üzerine gelerek gördügünüz string değeride yazabilirsiniz.

            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest()
            {
                ClientId = _configuration["ClientResourceOwner:ClientId"],
                ClientSecret = _configuration["ClientResourceOwner:ClientSecret"],
                RefreshToken = refleshToken,
                Address = discovery.TokenEndpoint
            };
            var token = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

            var tokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken(){ Name = OpenIdConnectParameterNames.IdToken, Value = token.IdentityToken },
                new AuthenticationToken(){ Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken },
                new AuthenticationToken(){ Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken },
                new AuthenticationToken(){ Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.UtcNow.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture) } //saat dilimi olarak Türkiye +3 de oldugu için evrensel saati alıyorum
            };

            var authonticationData = await HttpContext.AuthenticateAsync();
            var properties = authonticationData.Properties;
            properties.StoreTokens(tokens);

            await HttpContext.SignInAsync("Cookies", authonticationData.Principal, properties); //burada verilen cookie ismi önemli !. Startupta ne verildiyse o verilecek

            return RedirectToAction("Index");
        }

        [Authorize(Roles ="admin")]
        public IActionResult AdminAction()
        {
            return View();
        }

        [Authorize(Roles ="admin,customer")]
        public IActionResult CustomerAction()
        {
            return View();
        }
    }
}
