using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServerProject.Client2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //authorization server la haberleþmemizi yazalým.
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies"; //oluþacak olan cookie adý. Client1 ile giriþ yapýldýgýnda f12 tuþuna bakarak cookie oluþup oluþmadýgýna bakabilirsiniz
                options.DefaultChallengeScheme = "oidc"; //open id connect
            })
                .AddCookie("Cookies", options =>
                {
                    options.AccessDeniedPath = "/Home/AccessDenied"; //customer rolune sahip kiþi adminin görebilecegi metoda eriþmeye çalýþýrsa otomatik accesdenied sayfasýna yönlendiriyo ben burada istedigi sayfaya gitmesin benim istedigim sayfaya gitsin istiyorum
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";
                    options.Authority = "https://localhost:5001"; //tokeni dagýtan yetkili merkez neresi buraya onu yazalým. Yani AuthServer projemin ayaga kalkacagý port
                    options.ClientId = "Client2-MVC";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code id_token"; //code dediðim authorization token id_token ise tokeni dogrulamak için
                    options.GetClaimsFromUserInfoEndpoint = true; //arka planda userinfo endpointine istek atýp cookide yer alan kullanýcý bilgilerini adý rolünü vs getirecek kaynak = "https://identityserver4.readthedocs.io/en/latest/endpoints/userinfo.html",
                    options.SaveTokens = true;
                    options.Scope.Add("api1.read"); //verilen token bilgisi içerisinde bu scopeda göster diyorum
                    options.Scope.Add("offline_access");
                    options.Scope.Add("CountryAndCity"); //kullanýcýnýn ülke ve þehir bilgisini çagýrýyorum burada çagýrmýþ oldugum scope degerleri Config classýnda tanýmlamýþ oldugum IdentityResource degerleridir.

                    options.Scope.Add("Roles");
                    options.ClaimActions.MapUniqueJsonKey("country", "country"); //mapleme iþlemi
                    options.ClaimActions.MapUniqueJsonKey("city", "city");
                    options.ClaimActions.MapUniqueJsonKey("role", "role");

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        RoleClaimType = "role"
                    };
                });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
