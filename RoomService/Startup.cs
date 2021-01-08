using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RoomService.FileService;
using RoomService.MapperProfile;
using RoomService.Publisher;
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
            string connectionRabbitMQ = Configuration.GetConnectionString("Config");

            services.AddSingleton<IAdvancedBus>(RabbitHutch.CreateBus(connectionRabbitMQ).Advanced);
            services.AddDbContext<RoomContext>(options => options.UseSqlServer(connectionstring));
            services.AddScoped<IRepository<Room, Guid>, Repository<Room, Guid>>();
            services.AddScoped<IRepository<RoomImage, Guid>, Repository<RoomImage, Guid>>();
            services.AddScoped<IRoomService, RoomService.RoomService>();
            services.AddScoped<IFileService, FileService.FileService>();
            services.AddScoped<IPublisher, Publisher.Publisher>();
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

            var path = Path.Combine(env.ContentRootPath, "files");
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = path
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
