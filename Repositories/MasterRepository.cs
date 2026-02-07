using studentmanagementsystem.DatabaseContext;
using studentmanagementsystem.Interface;
using studentmanagementsystem.Models;
using Microsoft.EntityFrameworkCore;
namespace studentmanagementsystem.Repositories
{
    public class MasterRepository : IMasterRepository
    {
        private readonly AppDatabaseContext db;

        public MasterRepository(AppDatabaseContext context)
        {
            db = context;
        }

        // Country
        public async Task<List<Country>> GetCountries()
        {
            return await db.Countries.ToListAsync();
        }

        public async Task AddCountry(Country country)
        {
            await db.Countries.AddAsync(country);
        }

        // State
        public async Task<List<State>> GetStates()
        {
            return await db.States.Include(x => x.Country).ToListAsync();
        }

        public async Task AddState(State state)
        {
            await db.States.AddAsync(state);
        }

        // City
        public async Task<List<City>> GetCities()
        {
            return await db.Cities
                .Include(x => x.State)
                .ThenInclude(s => s.Country)
                .ToListAsync();
        }

        public async Task AddCity(City city)
        {
            await db.Cities.AddAsync(city);
        }

        public async Task<List<State>> GetStatesByCountry(int countryId)
        {
            return await db.States
                .Where(x => x.CountryId == countryId)
                .ToListAsync();
        }
        public async Task<List<City>> GetCitiesByState(int stateId)
        {
            return await db.Cities
                .Where(x => x.StateId == stateId)
                .OrderBy(x => x.CityName)
                .ToListAsync();
        }


        public async Task Save()
        {
            await db.SaveChangesAsync();
        }
    }
}
