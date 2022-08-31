using AutoMapper;

namespace City_info.Profiles
{
    public class PointsofInterestprofile : Profile
    {
        public PointsofInterestprofile() 
        {
            CreateMap<Entites.PointsOfInterest, Models.PointsOfInterestDto>();
            CreateMap<Models.PointsOfInterestCreationDto, Entites.PointsOfInterest>();
            CreateMap<Models.PointsOfInterestUpdateDto, Entites.PointsOfInterest>();
            CreateMap<Entites.PointsOfInterest, Models.PointsOfInterestUpdateDto>();

        }
    }
}
