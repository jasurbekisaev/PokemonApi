using PokemonApi.Context;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository;

public class ReviewerRepository : IReviewerRepository
{
    private readonly AppDbContext _context;
    public ReviewerRepository(AppDbContext context)
    {
        _context = context;
    }

    public ICollection<Reviewer> GetAllReviewsByReviewer(int reviewerId)
    {
        return _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).Select(r => r.Reviewer).ToList();
    }

    public Reviewer? GetReviewer(int reviewerId)
    {
        return _context.Reviewers.FirstOrDefault(r => r.Id == reviewerId);
    }

    public ICollection<Reviewer> GetReviewers()
    {
        return _context.Reviewers.ToList();
    }

    public bool ReviewerExists(int reviewerId)
    {
        return _context.Reviewers.Any(r => r.Id == reviewerId);
    }
}
