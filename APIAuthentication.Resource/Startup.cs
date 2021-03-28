using APIAuthentication.Resource.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace APIAuthentication.Resource
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(ApiKeyAuthNDefaults.SchemaName)
                .AddApiKey(opt =>
                {
                    opt.ApiKey = "Hello-World";
                    opt.QueryStringKey = "key";
                });

            //.AddScheme<ApiKeyAuthNOptions, ApiKeyAuthN>(ApiKeyAuthNDefaults.SchemaName, opt =>
            //{
            //    opt.ApiKey = "Hello-World";
            //    opt.QueryStringKey = "key";
            //});

            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync($"Hello World!{Environment.NewLine}");
                    await WriteClaims(context);

                }).RequireAuthorization();

                endpoints.MapGet("/anonymous", async context =>
                {
                    await context.Response.WriteAsync($"Hello World!{Environment.NewLine}");
                    await WriteClaims(context);
                });
            });
        }

        static async Task WriteClaims(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                await context.Response.WriteAsync($"Hello {context.User.Identity.Name}!{Environment.NewLine}");

                foreach (var item in context.User.Identities.First().Claims)
                {
                    await context.Response.WriteAsync($"Claim {item.Issuer} {item.Type} {item.Value}{Environment.NewLine}");
                }
            }
        }
    }
}
