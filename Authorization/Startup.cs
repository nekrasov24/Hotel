using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authorization.Dal;
using Authorization.UserRepository;
using Authorization.UserService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Authorization
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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<UserContext>(options => options.UseSqlServer(connection));
            services.AddControllers();
            services.AddScoped<IUserService, UserService.UserService>();
            services.AddScoped<IUserRepository, UserRepository.UserRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserContext userContext, IUserService service)
        {
            userContext.Database.Migrate();

            if (userContext.Users.All(x => x.Roles != Roles.Admin))
            {
                service.RegisterAdmin(new RegisterRequest()
                {
                    FirstName = "Dmitry", LastName = "Nekrasov",
                    Email = "d.a.04nekrasovgmail.com",
                    DateOfBirth = new DateTime(1992, 10, 04),
                    Password = "Nekrasov22Nekrasov",
                    PasswordConfirm = "Nekrasov22Nekrasov"
                });

            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
