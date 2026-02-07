using studentmanagementsystem.Models;

namespace studentmanagementsystem.Interface
{
    public interface IMasterRepository
    {
        public Task<List<Country>> GetCountries();
        public Task AddCountry(Country country);

        public Task<List<State>> GetStates();
        public Task AddState(State state);

        public Task<List<City>> GetCities();
        public Task AddCity(City city);

        public Task<List<State>> GetStatesByCountry(int countryId);
        public Task<List<City>> GetCitiesByState(int stateId);
        public Task Save();
    }
}
