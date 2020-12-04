using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomService.MapperProfile
{
    public static class MapperExtensions
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            if(services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            var mappingConfig = new MapperConfiguration(
                    mc =>
                    {
                        mc.AddProfile<RoomProfiles>();
                    }
            );

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
    }
}
