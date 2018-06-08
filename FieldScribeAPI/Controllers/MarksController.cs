using FieldScribeAPI.Models;
using FieldScribeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class MarksController : Controller
    {
        private readonly IMarkService _markService;
        private readonly IEntryService _entryService;
        private readonly IEventService _eventService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IAuthService _authService;

        public MarksController(IMarkService markService, 
            IEntryService entryService,
            IEventService eventService,
            UserManager<UserEntity> userManager,
            IAuthService authService)
        {
            _markService = markService;
            _entryService = entryService;
            _eventService = eventService;
            _userManager = userManager;
            _authService = authService;
        }


        [HttpGet("entries/{entryId}/[controller]",
            Name = nameof(GetMarksByEntryAsync))]

        public async Task<IActionResult> GetMarksByEntryAsync(
            int entryId, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var ent = await _entryService.GetEntryByIdAsync(entryId, ct);

            var marks = await _markService.GetMarksByEntryAsync(
                ent.eventId,
                entryId, ct);

            marks.Self = Link.To(nameof(GetMarksByEntryAsync), new { entryId });

            return Ok(marks);
        }

        [HttpPost("[controller]/vertical",
            Name = nameof(AddOrUpdateVerticalMarksAsync))]
        public async Task<IActionResult> AddOrUpdateVerticalMarksAsync(
            [FromBody] MarksForm<VerticalAttempt> marksForm,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var entry = await _entryService.GetEntryByIdAsync(marksForm.entryId, ct);
            if (entry == null) return NotFound("Entry not found");

            var evt = await _eventService.GetEventByIdAsync(marksForm.eventId, ct);
            if (evt.Params.EventType[0] != 'V') return BadRequest("Event type not vertical");

            await _markService.AddOrUpdateVerticalMarksAsync(marksForm, ct);

            return Created("ok", null);
        }

        [HttpPost("[controller]/horizontal/english",
            Name = nameof(AddOrUpdateEHorizontalMarksAsync))]
        public async Task<IActionResult> AddOrUpdateEHorizontalMarksAsync(
            [FromBody] MarksForm<EHorizontalAttempt> marksForm,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var entry = await _entryService.GetEntryByIdAsync(marksForm.entryId, ct);
            if (entry == null) return NotFound("Entry not found");

            var evt = await _eventService.GetEventByIdAsync(marksForm.eventId, ct);
            if (evt.Params.EventType[0] != 'H') return BadRequest("Event type not horizontal");
            if (evt.Params.MeasurementType[0] != 'E')
                return BadRequest("Measurement type not English");

            await _markService.AddOrUpdateEHorizontalMarksAsync(marksForm, ct);

            return Created(Url.Link(nameof(MarksController.GetMarksByEntryAsync),
                new { marksForm.entryId }),
                null);
        }

        // Commenting out token authorization for testing purposes.
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("[controller]/horizontal/metric",
            Name = nameof(AddOrUpdateMHorizontalMarksAsync))]
        public async Task<IActionResult> AddOrUpdateMHorizontalMarksAsync(
            [FromBody] MarksForm<MHorizontalAttempt> marksForm,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            //if (!await _authService.CanEditEvent(marksForm.eventId, User))
            //    return BadRequest("Not authorized to post marks");

            var entry = await _entryService.GetEntryByIdAsync(marksForm.entryId, ct);
            if (entry == null) return NotFound("Entry not found");

            var evt = await _eventService.GetEventByIdAsync(marksForm.eventId, ct);

            if (evt.Params.EventType[0] != 'H')
                return BadRequest("Event type not horizontal");
            if (evt.Params.MeasurementType[0] != 'M')
                return BadRequest("Measurement type not metric");

            await _markService.AddOrUpdateMHorizontalMarksAsync(marksForm, ct);

            return Created(Url.Link(nameof(MarksController.GetMarksByEntryAsync),
                new { marksForm.entryId }),
                null);
        }
    }
}
