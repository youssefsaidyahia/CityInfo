using AutoMapper;

namespace City_info.Profiles
{
    public class CityProfile : Profile
    {
        
        public CityProfile()
        {
            CreateMap<Entites.City, Models.cityWithoutPointsOfIntereDto>();
            CreateMap<Entites.City, Models.cityDto>();
        }
    }
}
