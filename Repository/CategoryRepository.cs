using PokemonApi.Context;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;


    public CategoryRepository(AppDbContext context)
    {
        _context = context;

    }
    public bool CategoryExists(int id)
    {
        return _context.Categories.Any(c => c.Id == id);
    }

    public ICollection<Category> GetCategories()
    {
        return _context.Categories.ToList();
    }

    public Category? GetCategory(int id)
    {
        return _context.Categories.Where(c => c.Id == id)!.FirstOrDefault()!;

    }

    public ICollection<Pokemon> GetPokemonByCategoryId(int categoryId)
    {
        return _context.PokemonCategories.Where(c => c.CategoryId == categoryId)
            .Select(c => c.Pokemon)
            .ToList();
    }

    public bool CreateCategory(Category category)
    {
        _context.Add(category);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }

    public bool UpdateCategory(Category category)
    {
        _context.Update(category);
        return Save();
    }
}