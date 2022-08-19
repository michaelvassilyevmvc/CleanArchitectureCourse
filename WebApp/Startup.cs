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
using Email.Interfaces;
using Hangfire;
using Infrastructure.Implementation;
using Infrastructure.Interefaces.WebApp;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Mobile.UseCases.Order.BackgroundJobs;
using UserCases.Order.Commands.CreateOrder;
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
            services.AddScoped<IBackgroundJobService, BackgroundJobService>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<WebApp.Interfaces.ICurrentUserService, CurrentUserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddDbContext<IDbContext, AppDbContext>(builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("MsSql")));

            //Application
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

            

            RecurringJob.AddOrUpdate<UpdateDeliveryStatusJob>("UpdateDeliveryStatusJob",
                (job)=>job.ExecuteAsync(), Cron.Minutely);
        }
    }
}