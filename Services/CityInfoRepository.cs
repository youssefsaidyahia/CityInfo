using City_info.DbContexts;
using City_info.Entites;
using Microsoft.EntityFrameworkCore;

namespace City_info.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c =>c.Name).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityid , bool includepointofinterest)
        {
            if (includepointofinterest) {
                return await _context.Cities.Include(c => c.PointsOfInterest).Where(c =>c.Id==cityid).FirstOrDefaultAsync();
            }
            return await _context.Cities.Where(c => c.Id == cityid).FirstOrDefaultAsync();    
        }
        public async Task<IEnumerable<City>> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pagesize)
        {
            var collection = _context.Cities as IQueryable<City>;
            if (!String.IsNullOrWhiteSpace(name))
            {
                name=name.Trim();   
                collection = collection.Where(c => c.Name == name);  
            }
            if (!String.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(c => c.Name.Contains(searchQuery)||
                    (c.Description !=null && c.Description.Contains(searchQuery)));   
            }
            return await collection.OrderBy(c=>c.Name).
                Skip(pagesize*(pageNumber-1)).Take(pagesize) .ToListAsync();

                
        }

        public async Task<bool> CityExsistsAsync(int cityid)
        {
            return await _context.Cities.AnyAsync(c =>c.Id==cityid);
        }
        public async Task AddPointOfInterestForCityAsync(int cityid, PointsOfInterest pointsOfInterest)
        {
            var city = await GetCityAsync(cityid, false);
            if (city != null)
            {
                city.PointsOfInterest.Add(pointsOfInterest);
            }
        }
        public async Task<bool> SavechangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
        public async Task<PointsOfInterest?> GetPointOfInterestAsync(int cityid, int pointofinterestid)
        {
            return await _context.PointsOfInterests.Where(p => p.CityId == cityid && p.Id==pointofinterestid).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointsOfInterest>> GetPointsOfInterestAsync(int cityid)
        {
            return await _context.PointsOfInterests.Where(p => p.CityId == cityid ).ToListAsync();

        }

        public void DeletePointsOfInterest(PointsOfInterest pointsOfInterest)
        {
             _context.PointsOfInterests.Remove(pointsOfInterest);
        }
    }
}
