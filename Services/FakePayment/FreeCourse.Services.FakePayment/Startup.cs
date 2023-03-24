using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace FreeCourse.Services.FakePayment
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
            services.AddMassTransit(x =>
            {
                //Default port : 5672
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(Configuration["RabbitMQSettings:Url"]),"/",host =>
                    {
                        host.Username(Configuration["RabbitMQSettings:Username"]);
                        host.Password(Configuration["RabbitMQSettings:Password"]);
                    });
                });
            });
            services.AddMassTransitHostedService();

            var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
            {
                opts.Authority = Configuration["IdentityServerUrl"];
                opts.Audience = "payment_resource";
                opts.RequireHttpsMetadata = false;
            });
            services.AddControllers(opts =>
            {
                opts.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy)); //Tüm controller'lara Authorize attribute'unu ekliyor
            }); services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCourse.Services.FakePayment", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCourse.Services.FakePayment v1"));
            }

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
