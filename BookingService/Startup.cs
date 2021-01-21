using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingService.BookingService;
using BookingService.HeaderService;
using BookingService.MapperProfile;
using BookingService.Model;
using BookingService.Publisher;
using BookingService.Quartz.Job;
using BookingService.Quartz.JobFactory;
using BookingService.Quartz.QuartzHostedService;
using BookingService.Quartz.Scheduler;
using BookingService.Subscriber;
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
using Quartz;
using Quartz.Impl;
using Quartz.Spi;


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
            services.AddSingleton(RabbitHutch.CreateBus(connectionRabbitMQ));
            services.Configure<ReservationSettings>(Configuration.GetSection(nameof(ReservationSettings)));
            services.AddSingleton<IReservationSettings>(sp => sp.GetRequiredService<IOptions<ReservationSettings>>().Value);
            services.AddScoped<IPublisher, Publisher.Publisher>();
            services.AddScoped<IHeaderService, HeaderService.HeaderService>();
            services.AddScoped<IBookingService, BookingService.BookingService>();
            services.AddSingleton<ISubscriber, Subscriber.Subscriber>();
            //services.AddSingleton<IJobFactory, JobFactory>();
            //services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();


            //services.AddTransient<Job>();


            //services.AddSingleton<JobRunner>();
            //services.AddSingleton(new JobSchedule(
            //    jobType: typeof(Job),
            //    cronExpression: "0/10 0 0 ? * * *"));
            //services.AddHostedService<QuartzHostedService>();



            //services.Configure<QuartzOptions>(Configuration.GetSection("Quartz"));


            /*services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionScopedJobFactory();

                var jobKey = new JobKey("awesome job", "awesome group");


                //q.AddJob<Job>(j => j
                //    .WithIdentity(jobKey)
                //    .WithDescription("my awesome job")
                //);

                //q.AddTrigger(t => t
                //    .WithIdentity("Simple Trigger")
                //    .ForJob(jobKey)
                //    .StartNow()
                //    .WithCronSchedule("0/10 0 0 ? * * *")
                //    .WithDescription("my awesome simple trigger")
                //);

                //IJobDetail job = JobBuilder.Create<Job>()
                //    .WithIdentity("job1", "group1")
                //    .Build();

                //// Trigger the job to run now, and then repeat every 10 seconds
                //ITrigger trigger = TriggerBuilder.Create()
                //    .WithIdentity("trigger1", "group1")
                //    .StartNow()
                //    .WithSimpleSchedule(x => x
                //        .WithIntervalInSeconds(10)
                //        .RepeatForever())
                //    .Build();

                q.ScheduleJob<Job>(t => t
                    .WithIdentity("Simple Trigger")
                    .StartNow()
                    .WithCronSchedule("* * * * * ?")
                    .WithDescription("my awesome simple trigger")
                );
            });*/

            // Quartz.Extensions.Hosting hosting
            /*services.AddQuartzServer(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });*/

            services.AddHttpContextAccessor();
            services.AddMapper();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, ISubscriber subscriber)
        {

            subscriber.SubscribeJobMessage();
            await subscriber.SubscribeVerificationReservationId();

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
