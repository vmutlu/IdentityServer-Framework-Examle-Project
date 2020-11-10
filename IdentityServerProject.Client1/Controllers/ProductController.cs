using IdentityServerProject.Client1.Models;
using IdentityServerProject.Client1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServerProject.Client1.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IApiResourceHttpClient _apiResourceHttpClient;
        public ProductController(IConfiguration configuration, IApiResourceHttpClient apiResourceHttpClient)
        {
            _configuration = configuration;
            _apiResourceHttpClient = apiResourceHttpClient;
        }

        //Tüm producklarımı bana vermesi gereken api ye istek atılıyor.
        public async Task<IActionResult> Index()
        {
            #region Token Alma İşlemi
            //HttpClient httpClient = new HttpClient(); //artık buna ihtiyacım yok IApiResourceHttpClient bagımlılıgı ile hallettim
            //var discovery = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5001");

            //if (discovery.IsError)
            //    throw new ArgumentException("Error");

            //ClientCredentialsTokenRequest clientCredentials = new ClientCredentialsTokenRequest()
            //{
            //    ClientId = _configuration["Client:ClientId"],
            //    ClientSecret = _configuration["Client:ClientSecret"],
            //    Address = discovery.TokenEndpoint
            //};

            //var token = await httpClient.RequestClientCredentialsTokenAsync(clientCredentials);

            //token almama gerek yok kullanıcı üye oldugunda token gelecek bana yukarıda yorum satırına aldıgım kodlar ile api ye client ıd ve secret yani şifre degerimle istek atıp token alıyordum
            //var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken); 

            #endregion

            //_apiResourceHttpClient.SetBearerToken(accessToken);

            var client = await _apiResourceHttpClient.GetHttpClient();

            var response = await client.GetAsync("https://localhost:5007/api/products/getproducts");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<List<Product>>(content);

                return View(product);
            }

            return View();
        }
    }
}
