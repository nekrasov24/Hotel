using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingService.BookingService;
using BookingService.HeaderService;
using BookingService.Model;
using BookingService.Publisher;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookingService
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
            string connectionRabbitMQ = Configuration.GetConnectionString("Config");
            services.AddSingleton(RabbitHutch.CreateBus(connectionRabbitMQ).Advanced);
            services.Configure<ReservationSettings>(Configuration.GetSection(nameof(ReservationSettings)));
            services.AddSingleton<IReservationSettings>(sp => sp.GetRequiredService<IOptions<ReservationSettings>>().Value);
            services.AddScoped<IPublisher, Publisher.Publisher>();
            services.AddScoped<IHeaderService, HeaderService.HeaderService>();
            services.AddScoped<IBookingService, BookingService.BookingService>();

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
