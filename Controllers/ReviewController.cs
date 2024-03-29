﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApi.DtoModels;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;
    private readonly IPokemonRepository _pokeRepository;
    private readonly IReviewerRepository _reviewerRepository;

    public ReviewController(IReviewRepository reviewRepository,
        IMapper mapper, IPokemonRepository pokeRepository, IReviewerRepository reviewerRepository)
    {
        _reviewerRepository = reviewerRepository;
        _pokeRepository = pokeRepository;
        _reviewRepository = reviewRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
    public IActionResult GetReviews()
    {
        var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(reviews);
    }


    [HttpGet("/reviews/{ownerId}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType(400)]
    public IActionResult GetReview(int reviewId)
    {
        if (!_reviewRepository.ReviewExists(reviewId))
            return NotFound();

        var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(review);
    }


    [HttpGet("/reviews/{pokeId}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType(400)]
    public IActionResult GetReviewOfAPokemon(int pokeId)
    {
        var reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(reviews);
    }


    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId, [FromBody] ReviewDto reviewDto)
    {
        if (reviewDto == null)
            return BadRequest(ModelState);

        var review = _reviewRepository.GetReviews()
            .Where(c => c.Title.Trim().ToUpper() == reviewDto.Title.TrimEnd().ToUpper())
            .FirstOrDefault();

        if (review != null)
        {
            ModelState.AddModelError("", "Review already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var reviewMap = _mapper.Map<Review>(reviewDto);
        reviewMap.Pokemon = _pokeRepository.GetPokemon(pokeId);
        reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

        if (!_reviewRepository.CreateReview(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Review successfully created");
    }

    [HttpPut("{reviewId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
    {
        if (updatedReview == null)
            return BadRequest(ModelState);

        if (reviewId != updatedReview.Id)
            return BadRequest(ModelState);

        if (!_reviewRepository.ReviewExists(reviewId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var reviewMap = _mapper.Map<Review>(updatedReview);

        if (!_reviewRepository.UpdateReview(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong updating review");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }


    [HttpDelete("{reviewId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteReview(int reviewId)
    {
        if (!_reviewRepository.ReviewExists(reviewId))
        {
            return NotFound();
        }

        var reviewToDelete = _reviewRepository.GetReview(reviewId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!_reviewRepository.DeleteReview(reviewToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting owner");
        }

        return NoContent();
    }

    [HttpDelete("/DeleteReviewsByReviewer/{reviewerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteReviewsByReviewer(int reviewerId)
    {
        if (!_reviewerRepository.ReviewerExists(reviewerId))
            return NotFound();

        var reviewsToDelete = _reviewerRepository.GetAllReviewsByReviewer(reviewerId).ToList();
        if (!ModelState.IsValid)
            return BadRequest();

        if (!_reviewRepository.DeleteReviews(reviewsToDelete))
        {
            ModelState.AddModelError("", "error deleting reviews");
            return StatusCode(500, ModelState);
        }
        return NoContent();
    }
}
