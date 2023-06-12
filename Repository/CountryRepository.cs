using AutoMapper;
using PokemonApi.Context;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CountryRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public Country? GetCountry(int id)
        {
            return _context.Countries.FirstOrDefault(c => c.Id == id);
        }

        public Country? GetCountryByOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersByCountryId(int countryId)
        {
            return _context.Owners.Where(c => c.Country.Id == countryId).ToList();
        }

        public bool CountryExists(int countryId)
        {
            return _context.Countries.Any(c => c.Id == countryId);
        }
        public bool CreateCountry(Country country)
        {
            _context.Add(country);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCountry(Country country)
        {
            _context.Update(country);
            return Save();
        }

        public bool DeleteCountry(Country country)
        {
            _context.Remove(country);
            return Save();

        }
    }
}