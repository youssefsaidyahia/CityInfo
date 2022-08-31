using City_info.Entites;

namespace City_info.Services
{
    public interface ICityInfoRepository
    {
       Task<IEnumerable<City>> GetCitiesAsync();
        Task<IEnumerable<City>> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pagesize);
        Task<City?> GetCityAsync(int cityid , bool includepointofinterest);
        Task<bool> CityExsistsAsync(int cityid);
       Task<IEnumerable<PointsOfInterest>> GetPointsOfInterestAsync(int cityid);
       Task<PointsOfInterest?> GetPointOfInterestAsync(int cityid , int pointofinterestid);
       Task AddPointOfInterestForCityAsync(int cityid, PointsOfInterest pointsOfInterest);
       void DeletePointsOfInterest(PointsOfInterest pointsOfInterest);
     
        Task<bool> SavechangesAsync();

    }
}
