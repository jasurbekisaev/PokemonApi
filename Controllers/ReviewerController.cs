using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.DtoModels;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewerController : ControllerBase
{
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IMapper _mapper;

    public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
    {
        _reviewerRepository = reviewerRepository;
        _mapper = mapper;
    }


    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewers()
    {
        var reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reviewers);
    }


    [HttpGet("{reviewerId}")]
    [ProducesResponseType(200, Type = typeof(Reviewer))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewer(int reviewerId)
    {
        if (!_reviewerRepository.ReviewerExists(reviewerId))
            return NotFound();

        var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reviewer);
    }

    [HttpGet("/reviews/{reviewerId}")]
    [ProducesResponseType(200, Type = typeof(Reviewer))]
    [ProducesResponseType(400)]
    public IActionResult GetAllReviewsByReviewer(int reviewerId)
    {
        var reviews = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetAllReviewsByReviewer(reviewerId));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(reviews);
    }

    [HttpGet("/ReviewerExists/{reviewerId}")]
    [ProducesResponseType(200, Type = typeof(Reviewer))]
    [ProducesResponseType(400)]
    public bool ReviewerExists(int reviewerId)
    {
        var reviewerExists = _reviewerRepository.ReviewerExists(reviewerId);
        return reviewerExists;
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerDto)
    {
        if (reviewerDto == null)
            return BadRequest(ModelState);

        var reviewer = _reviewerRepository.GetReviewers()
             .Where(c => c.FirstName.Trim().ToUpper() == reviewerDto.FirstName.TrimEnd().ToUpper())
            .FirstOrDefault();
        if (reviewer != null)
        {
            ModelState.AddModelError("", "Reviewer already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var reviewerMap = _mapper.Map<Reviewer>(reviewerDto);

        if (!_reviewerRepository.CreateReviewer(reviewerMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Reviewer successfully created");
    }

    [HttpPut("{reviewerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto updatedReviewer)
    {
        if (updatedReviewer == null)
            return BadRequest(ModelState);

        if (reviewerId != updatedReviewer.Id)
            return BadRequest(ModelState);

        if (!_reviewerRepository.ReviewerExists(reviewerId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var reviewerMap = _mapper.Map<Reviewer>(updatedReviewer);

        if (!_reviewerRepository.UpdateReviewer(reviewerMap))
        {
            ModelState.AddModelError("", "Something went wrong updating owner");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

}
