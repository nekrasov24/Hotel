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
using RoomService.FileService;
using RoomService.MapperProfile;
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
            services.AddScoped<IRepository<Room, Guid>, Repository<Room, Guid>>();
            services.AddScoped<IRepository<RoomImage, Guid>, Repository<RoomImage, Guid>>();
            services.AddScoped<IRoomService, RoomService.RoomService>();
            services.AddScoped<IFileService, FileService.FileService>();
            services.AddMapper();


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
