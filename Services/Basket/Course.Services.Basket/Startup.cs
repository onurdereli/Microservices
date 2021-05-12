using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Course.Services.Basket.Services.Abstract;
using Course.Services.Basket.Services.Concrete;
using Course.Services.Basket.Settings;
using Course.Shared.Services.Abstract;
using Course.Shared.Services.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;

namespace Course.Services.Basket
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
            // giriş yapmış bir kullanıcı beklediğini belirtir
            var requireAutorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //kimin dağıtıcı olduğunu belirtir
                    options.Authority = Configuration["IdentityServerUrl"];
                    options.Audience = "resource_basket";
                    options.RequireHttpsMetadata = false;
                });
            services.AddHttpContextAccessor();
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();
            services.AddScoped<IBasketService, BasketService>();
            services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));
            services.AddSingleton<RedisService>(sp =>
            {
                var redisSettings =  sp.GetRequiredService<IOptions<RedisSettings>>().Value;
                var redisService = new RedisService(redisSettings.Host, redisSettings.Port);
                redisService.Connect();

                return redisService;
            });
            //autorize olmuş kullanıcı için ekleme yapılır
            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter(requireAutorizePolicy));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Course.Services.Basket", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Course.Services.Basket v1"));
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