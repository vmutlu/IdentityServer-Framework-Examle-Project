using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServerProject.API1
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
            //alýnan tokeni jwt.io sitesi ile içerisinde taþýnan bilgilere bakýlabilir.

            //kimlik dogrulamasý yapalým
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options =>
            {
                options.Authority = "https://localhost:5001";
                options.Audience = "resource_api1"; //bana bir token geldiði zaman aut alanýnda bu olmalý. Yani tokeni alan kullanýcý bu alana eriþebilirmi onu kontrol ediyor
            }); //schema girmek zorundayým

            //kimliði dogrulanmýþ bir kullanýcýnýn yetkilendirmesi
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ReadProduct", options => // ReadProduct bu yetkiye sahip olan client bu api'de sadece read metodlarýna istek atabilsin.
                {
                     options.RequireClaim("scope", "api1.read");
                 });

                options.AddPolicy("UpdateOrCreate", options =>
                 {
                     options.RequireClaim("scope", new[] { "api1.update", "api1.create" });
                 });
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
