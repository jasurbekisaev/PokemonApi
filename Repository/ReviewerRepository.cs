using Microsoft.EntityFrameworkCore;
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

    public Reviewer? GetReviewer(int reviewerId)
    {
        return _context.Reviewers.Where(r => r.Id == reviewerId).Include(e => e.Reviews).FirstOrDefault();
    }

    public ICollection<Reviewer> GetReviewers()
    {
        return _context.Reviewers.ToList();
    }

    public bool ReviewerExists(int reviewerId)
    {
        return _context.Reviewers.Any(r => r.Id == reviewerId);
    }

    public ICollection<Review> GetAllReviewsByReviewer(int reviewerId)
    {
        return _context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
    }

    public bool CreateReviewer(Reviewer reviewer)
    {
        _context.Add(reviewer);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }

    public bool UpdateReviewer(Reviewer reviewer)
    {
        _context.Update(reviewer);
        return Save();
    }

    public bool DeleteReviewer(Reviewer reviewer)
    {
        _context.Remove(reviewer);
        return Save();
    }
}
