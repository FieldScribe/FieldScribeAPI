using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FieldScribeAPI.Infrastructure;
using FieldScribeAPI.Models;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using FieldScribeAPI.Services;
using Microsoft.Extensions.Options;

namespace FieldScribeAPI.Controllers
{
    [Route("/")]
    public class AthletesController : Controller
    {
        private readonly IAthleteService _athleteService;
        private readonly PagingOptions _defaultPagingOptions;

        public AthletesController(IAthleteService athleteService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _athleteService = athleteService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpGet("[controller]", Name = nameof(GetAthletesAsync))]
        public async Task<IActionResult> GetAthletesAsync(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<Athlete, AthleteEntity> sortOptions,
            [FromQuery] SearchOptions<Athlete, AthleteEntity> searchOptions,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var athletes = await _athleteService.GetAthletesAsync(
                pagingOptions, 
                sortOptions, 
                searchOptions,
                ct);

            var collection = PagedCollection<Athlete>.Create<AthletesResponse>(
                Link.ToCollection(nameof(GetAthletesAsync)),
                athletes.Items.ToArray(),
                athletes.TotalSize,
                pagingOptions);
            collection.AthletesQuery = FormMetadata.FromResource<Athlete>(
                Link.ToForm(nameof(GetAthletesByMeetAsync), null,
                Link.GetMethod, Form.QueryRelation));


            return Ok(collection);
        }

        // athlete/{athleteId}
        [HttpGet("[controller]/{athleteId}", Name = nameof(GetAthleteByIdAsync))]
        public async Task<IActionResult> GetAthleteByIdAsync(int athleteId, CancellationToken ct)
        {
            var athlete = await _athleteService.GetAthleteAsync(athleteId, ct);
            if (athlete == null) return NotFound();

            return Ok(athlete);
        }


        // meets/{meetId}/athletes
        [HttpGet("meets/{meetId}/[controller]", Name = nameof(GetAthletesByMeetAsync))]
        public async Task<IActionResult> GetAthletesByMeetAsync(
            int meetId,
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<Athlete, AthleteEntity> sortOptions,
            [FromQuery] SearchOptions<Athlete, AthleteEntity> searchOptions,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var athletes = await _athleteService.GetAthletesByMeetAsync(
                meetId,
                pagingOptions,
                sortOptions,
                searchOptions,
                ct);

            var collection = PagedCollection<Athlete>.Create<AthletesResponse>(
                Link.ToCollection(nameof(GetAthletesByMeetAsync)),
                athletes.Items.ToArray(),
                athletes.TotalSize,
                pagingOptions);
            collection.AthletesQuery = FormMetadata.FromResource<Athlete>(
                Link.ToForm(nameof(GetAthletesByMeetAsync), null,
                Link.GetMethod, Form.QueryRelation));

            return Ok(collection);
        }


        // POST /athletes
        [HttpPost("/athlete", Name = nameof(AddAthleteAsync))]
        public async Task<IActionResult> AddAthleteAsync(
            [FromBody] AthleteForm athleteForm,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var athleteId = await _athleteService.CreateAthleteAsync(
             athleteForm.meetId, 
             athleteForm.CompNum, 
             athleteForm.FirstName,
             athleteForm.LastName,
             athleteForm.TeamName, 
             athleteForm.Gender,
             ct);

            return Created(
                Url.Link(nameof(AthletesController.GetAthleteByIdAsync),
                new { athleteId }),
                null);
        }
    }
}