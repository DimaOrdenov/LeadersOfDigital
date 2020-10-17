using Api.Data.Models;
using AutoMapper;
using DataModels.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Facility, FacilityResponse>();
            CreateMap<Barrier, BarrierResponse>();
            CreateMap<Disability, DisabilityResponse>();
        }
    }
}
