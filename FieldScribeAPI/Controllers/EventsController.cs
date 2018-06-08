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
    [Route("/")]
    public class EventsController : Controller
    {
        private readonly IEventService _eventService;
        private readonly PagingOptions _defaultPagingOptions;

        public EventsController(IEventService eventService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _eventService = eventService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpGet("[controller]/{eventId}", Name = nameof(GetEventByIdAsync))]
        public async Task<IActionResult> GetEventByIdAsync(
            int eventId, CancellationToken ct)
        {
            var evt = await _eventService.GetEventByIdAsync(eventId, ct);
            if (evt == null) return NotFound();

            return Ok(evt);
        }


        [HttpGet("meets/{meetId}/[controller]", Name = nameof(GetEventsByMeetAsync))]
        public async Task<IActionResult> GetEventsByMeetAsync(
            int meetId,
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<Event, EventEntity> sortOptions,
            [FromQuery] SearchOptions<Event, EventEntity> searchOptions,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var events = await _eventService.GetEventsByMeetAsync(
                meetId,
                pagingOptions,
                sortOptions,
                searchOptions,
                ct);

            var collection = PagedCollection<Event>.Create<EventsResponse>(
                Link.ToCollection(nameof(GetEventsByMeetAsync)),
                events.Items.ToArray(),
                events.TotalSize,
                pagingOptions);
            collection.EventsQuery = FormMetadata.FromResource<Event>(
                Link.ToForm(nameof(GetEventsByMeetAsync), null,
                Link.GetMethod, Form.QueryRelation));

            return Ok(collection);
        }


        [HttpGet("athletes/{athleteId}/[controller]", Name = nameof(GetEventsByAthleteAsync))]
        public async Task<IActionResult> GetEventsByAthleteAsync(
            int athleteId,
            [FromQuery] SortOptions<Event, EventEntity> sortOptions,
            [FromQuery] SearchOptions<Event, EventEntity> searchOptions,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var events = await _eventService.GetEventsByAthleteAsync(
                athleteId,
                sortOptions,
                searchOptions,
                ct);

            events.Self = Link.To(nameof(GetEventsByAthleteAsync), new { athleteId });

            return Ok(events);
        }


        // POST /event
        [HttpPost("/event", Name = nameof(AddEventAsync))]
        public async Task<IActionResult> AddEventAsync(
            [FromBody] EventForm eventForm,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var eventId = await _eventService.CreateEventAsync(
             eventForm.meetId,
             eventForm.EventNum,
             eventForm.RoundNum,
             eventForm.FlightNum,
             eventForm.EventName,
             ct);

            return Created(
                Url.Link(nameof(EventsController.GetEventByIdAsync),
                new { eventId }),
                null);
        }


        [HttpPost("barheights/english", Name = 
            nameof(AddOrUpdateEnglishBarHeightsAsync))]

        public async Task<IActionResult> AddOrUpdateEnglishBarHeightsAsync(
            [FromBody] BarHeightsForm<EnglishBarHeight> barHeightsForm,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var evt = await _eventService.GetEventByIdAsync(barHeightsForm.eventId, ct);

            if (evt.Params.EventType[0] != 'V') return BadRequest("Event type not vertical");
            if (evt.Params.MeasurementType[0] != 'E')
                return BadRequest("Measurement type not English");

            await _eventService.AddOrUpdateEnglishBarHeightsAsync(
                barHeightsForm, ct);

            return Created(Url.Link(nameof(EventsController.GetEventByIdAsync),
                new { barHeightsForm.eventId }),
                null);
        }


        [HttpPost("barheights/metric", Name =
            nameof(AddOrUpdateMetricBarHeightsAsync))]

        public async Task<IActionResult> AddOrUpdateMetricBarHeightsAsync(
            [FromBody] BarHeightsForm<MetricBarHeight> barHeightsForm,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var evt = await _eventService.GetEventByIdAsync(barHeightsForm.eventId, ct);

            if (evt.Params.EventType[0] != 'V') return BadRequest("Event type not vertical");
            if (evt.Params.MeasurementType[0] != 'M')
                return BadRequest("Measurement type not metric");

            await _eventService.AddOrUpdateMetricBarHeightsAsync(
                barHeightsForm, ct);

            return Created(Url.Link(nameof(EventsController.GetEventByIdAsync),
                new { barHeightsForm.eventId }),
                null);
        }
    }
}
