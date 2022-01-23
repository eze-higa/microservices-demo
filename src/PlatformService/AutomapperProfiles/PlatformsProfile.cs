using AutoMapper;
using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.AutomapperProfiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            CreateMap<PlatformCreateDTO, Platform>();
            CreateMap<Platform, PlatformReadDTO>();
        }
    }
}