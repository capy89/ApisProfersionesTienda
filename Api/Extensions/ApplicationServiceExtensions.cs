﻿using AspNetCoreRateLimit;
using Core.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;

namespace Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                 builder.AllowAnyOrigin() //WithOrigins("http://dominio.com")
                 .AllowAnyMethod()       //WithMethods("GET","POST")
                 .AllowAnyHeader());     //WithHeaders("accept","content-type")
            });


        public static void AddAplicationServices(this IServiceCollection services)
        {
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<IProductoRepository, ProductoRepository>();
            //services.AddScoped<IMarcaRepository, MarcaRepository>();
            //services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureRateLimitation(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();

            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests= true;
                options.HttpStatusCode = 409;
                options.RealIpHeader = "X-Real-IP";
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint="*",
                        Period="10s",
                        Limit=10
                    }
                };
            });
        }
    }
}
