using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using TaskManagement.BusinessLogic.DTOs;
using TaskManagement.BusinessLogic.DTOs.Responses;
using TaskManagement.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaskManagement.BusinessLogic.Mapping
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {

            CreateMap<AppTaskModel, AppTask>()
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<AppTaskCreateUpdateModel, AppTask>();
            CreateMap<AppTask, AppTaskModel>();

        }
    }

}
