using FieldScribeAPI.Models;
using FieldScribeAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FieldScribeAPI.Controllers
{
    [Route("/")]
    public class EntriesController : Controller
    {
        private readonly IEntryService _entryService;

        public EntriesController(IEntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpGet("[controller]/{entryId}",
            Name = nameof(GetEntryByIdAsync))]
        public async Task<IActionResult> GetEntryByIdAsync(int entryId, CancellationToken ct)
        {
            var entry = await _entryService.GetEntryByIdAsync(entryId, ct);
            if (entry == null) return NotFound();

            return Ok(entry);
        }

        [HttpGet("events/{eventId}/entries", 
            Name = nameof(GetEntriesByEventAsync))]
        public async Task<IActionResult> GetEntriesByEventAsync(
            int eventId,
            [FromQuery] SortOptions<EventEntry, EventEntryEntity> sortOptions,
            [FromQuery] SearchOptions<EventEntry, EventEntryEntity> searchOptions,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var entries = await _entryService.GetEntriesByEventAsync(
                eventId, sortOptions, searchOptions, ct);

            return Ok(entries);
        }
    }
}