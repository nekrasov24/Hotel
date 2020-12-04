using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RoomService.RoomModel;
using RoomService.RoomRepository;
using RoomService.RoomService;

namespace RoomService
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
            services.AddDbContext<RoomContext>(options => options.UseSqlServer(connectionstring));
            services.AddScoped<IRoomRepository, RoomRepository.RoomRepository>();
            services.AddScoped<IRoomService, RoomService.RoomService>();

            var configuration = new MapperConfiguration(cfg =>
            cfg.CreateMap<UpdateRoomModelRequest, Room>());
            configuration.AssertConfigurationIsValid();
            var mapper = configuration.CreateMapper();

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
