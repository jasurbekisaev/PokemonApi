using PokemonApi.Models;

namespace PokemonApi.Interfaces;

public interface ICategoryRepository
{
    public ICollection<Category> GetCategories();
    public Category? GetCategory(int id);
    public ICollection<Pokemon> GetPokemonByCategoryId(int categoryId);
    public bool CategoryExists(int id);
    bool CreateCategory(Category category);
    bool UpdateCategory(Category category);
    bool DeleteCategory(Category category);
    bool Save();


}