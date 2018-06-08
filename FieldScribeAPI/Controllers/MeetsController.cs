using FieldScribeAPI.Infrastructure;
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
    [Route("/[controller]")]
    public class MeetsController : Controller
    {
        private readonly IMeetService _meetService;
        private readonly PagingOptions _defaultPagingOptions;

        public MeetsController(IMeetService meetService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _meetService = meetService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpGet(Name = nameof(GetMeetsAsync))]
        public async Task<IActionResult> GetMeetsAsync(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<Meet, MeetEntity> sortOptions,
            [FromQuery] SearchOptions<Meet, MeetEntity> searchOptions,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var meets = await _meetService.GetMeetsAsync(
                pagingOptions,
                sortOptions,
                searchOptions,
                ct);

            var collection = PagedCollection<Meet>.Create<MeetsResponse>(
                Link.ToCollection(nameof(GetMeetsAsync)),
                meets.Items.ToArray(),
                meets.TotalSize,
                pagingOptions);
            collection.MeetsQuery = FormMetadata.FromResource<Meet>(
                Link.ToForm(nameof(GetMeetsAsync), null,
                Link.GetMethod, Form.QueryRelation));


            return Ok(collection);
        }

        // meets/{meetId}
        [HttpGet("{meetId}", Name = nameof(GetMeetByIdAsync))]
        public async Task<IActionResult> GetMeetByIdAsync(int meetId, CancellationToken ct)
        {
            var meet = await _meetService.GetMeetAsync(meetId, ct);
            if (meet == null) return NotFound();

            return Ok(meet);
        }


        // POST /meets
        [HttpPost("/meets", Name = nameof(AddMeetAsync))]
        public async Task<IActionResult> AddMeetAsync(
            [FromBody] MeetForm meetForm,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var meetId = await _meetService.CreateMeetAsync(
             meetForm.MeetName,
             meetForm.MeetDate,
             meetForm.MeetLocation,
             meetForm.MeasurementType,
             ct);

            return Created(
                Url.Link(nameof(MeetsController.GetMeetByIdAsync),
                new { meetId }),
                null);
        }

        // Delete Meet
        [HttpPost("{meetId}/delete", Name = nameof(DeleteMeetAsync))]
        public async Task<IActionResult> DeleteMeetAsync(int meetId, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var (succeeded, error) = await _meetService.DeleteMeetAsync(meetId, ct);

            if (succeeded) return Ok();
            return NotFound(error);
        }

        // Edit Meet
        [HttpPost("{meetId}/edit", Name = nameof(EditMeetAsync))]
        public async Task<IActionResult> EditMeetAsync(
            int meetId,
            [FromBody] MeetForm meetForm,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var (succeeded, error) = await _meetService.EditMeetAsync(
                meetId,
                meetForm.MeetName,
                meetForm.MeetDate,
                meetForm.MeetLocation,
                meetForm.MeasurementType,
                ct);

            if (succeeded) return Ok();
            return NotFound(error);
        }
    }
}
