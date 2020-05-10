using EasyNetQ;
using LM.Domain.UnitOfWork;
using LM.Infra.UnitOfWork;
using LM.MSEmail.Api.Config.Services;
using LM.MSEmail.Api.Config.Services.Contracts;
using LM.MSEmail.Api.Domain.Repositories;
using LM.MSEmail.Api.Domain.Services;
using LM.MSEmail.Api.Domain.Services.Contracts;
using LM.MSEmail.Api.Domain.Settings;
using LM.MSEmail.Api.Handles;
using LM.MSEmail.Api.Infra.Context;
using LM.MSEmail.Api.Infra.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LM.MSEmail.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContextPool<EmailContext>(options => options.UseMySql(Configuration.GetConnectionString("Me")));

            services.AddScoped<IUnitOfWork, UnitOfWork<EmailContext>>();

            services.Configure<EmailSettings>(Configuration);

            services.AddTransient<IHistoryService, HistoryService>();

            services.AddTransient<IEmailService, EmailService>();

            services.AddTransient<IHistoryRepository, HistoryRepository>();

            services.AddSingleton(RabbitHutch.CreateBus(Configuration.GetSection("RabbitMQ:Connection").Value).Advanced);

            services.AddHostedService<EmailHandle>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}