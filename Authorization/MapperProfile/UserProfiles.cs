using Authorization.Dal;
using Authorization.UserService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.MapperProfile
{
    public class UserProfiles : Profile
    {
        public UserProfiles()
        {
            CreateMap<EditUserRequestModel, User>();
        }
    }
}
