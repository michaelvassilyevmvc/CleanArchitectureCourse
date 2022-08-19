using Application;
using ApplicationServices.Implementation;
using ApplicationServices.Interfaces;
using AutoMapper;
using DataAccess;
using DataAccess.Interefaces;
using Delivery.Company;
using Delivery.Interfaces;
using DomainServices.Implementation;
using DomainServices.Interfaces;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Telemetry.Implementation;
using Telemetry.Interfaces;
using UserCases.Order.Commands.CreateOrder;
using Web.ApplicationServices.Implementation;
using Web.ApplicationServices.Interfaces;
using WebApp.Interfaces;
using WebApp.Services;

namespace WebApp
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApp", Version = "v1" });
            });

            //Domain
            services.AddScoped<IOrderDomainService, OrderDomainService>();

            //Infrastructure
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddDbContext<IDbContext, AppDbContext>(builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("MsSql")));

            //Mobile Infrastructure
            services.AddScoped<ITelemetryService, TelemetryService>();

            //Web Infrastructure
            services.AddScoped<IWebApplicationService, WebApplicationService>();


            //UseCases & Application
            services.AddScoped<ISecurityService, SecurityService>();

            //Framework
            services.AddControllers();
            services.AddMediatR(typeof(CreateOrderCommand));
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddHangfire(cfg => cfg.UseSqlServerStorage(Configuration.GetConnectionString("MsSql")));
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApp v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}