using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderService.HeaderService;
using OrderService.MapperProfile;
using OrderService.Model;
using OrderService.OrderRepository;
using OrderService.OrderService;
using OrderService.Publisher;

namespace OrderService
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
            string connectionstring = Configuration.GetConnectionString("DefaultConnectionString");
            string connectionRabbitMQ = Configuration.GetConnectionString("Config");

            services.AddSingleton(RabbitHutch.CreateBus(connectionRabbitMQ).Advanced);
            services.AddSingleton(RabbitHutch.CreateBus(connectionRabbitMQ));
            services.AddDbContext<OrderContext>(options => options.UseSqlServer(connectionstring));
            services.AddScoped<IOrderService, OrderService.OrderService>();
            services.AddScoped<IHeaderService, HeaderService.HeaderService>();
            services.AddScoped<IOrderRepository<Order, Guid>, OrderRepository<Order, Guid>>();
            services.AddScoped<IPublisher, Publisher.Publisher>();
            services.AddMapper();

            services.AddHttpContextAccessor();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
