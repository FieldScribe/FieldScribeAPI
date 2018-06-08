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
    public class LynxController : Controller
    {
        private readonly ILynxService _lynxService;
        private readonly PagingOptions _defaultPagingOptions;

        public LynxController(ILynxService lynxService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _lynxService = lynxService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpPost(Name = nameof(AddLynxDataAsync))]

        public async Task<IActionResult> AddLynxDataAsync(
            [FromBody] LynxDataForm dataForm,
            CancellationToken ct)
        {
            List<LynxAthlete> athletes = new LynxAthleteReader(
                new FileTextReader(dataForm.PplFileText))
                .ReadAthletes();

            List<int> fieldEvtNums = new LynxSchReader(
                new FileTextReader(dataForm.SchFileText))
                .ReadFieldEvtNums();

            List<LynxEvent> fieldEvents = new LynxEventReader(
                fieldEvtNums,
                new FileTextReader(dataForm.EvtFileText))
                .ReadEvents();

            await _lynxService.UpdateLynxDataAsync(
                dataForm.meetId, fieldEvents, athletes, ct);

            return Created(Url.Link(nameof(EventsController.GetEventsByMeetAsync),
                new { dataForm.meetId }),
                null);
        }
    }
}
