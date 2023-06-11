using PokemonApi.Models;

namespace PokemonApi.Interfaces;

public interface ICountryRepository
{
    public ICollection<Country> GetCountries();
    public Country? GetCountry(int id);
    public Country? GetCountryByOwner(int ownerId);
    public ICollection<Owner> GetOwnersByCountryId(int countryId);
    public bool CountryExists(int countryId);
    bool CreateCountry(Country country);
    bool Save();

}