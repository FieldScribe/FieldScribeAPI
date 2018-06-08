using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FieldScribeAPI.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace FieldScribeAPI.Infrastructure
{
    // Get collection of marks for a single event entry
    public class AttemptsProcessor
    {
        private readonly FieldScribeAPIContext _context;
        private IQueryable marks;
        private bool measurementTypeEnglish;
        private bool eventTypeVertical;

        public AttemptsProcessor(FieldScribeAPIContext context, int eventId)
        {
            _context = context;

            eventTypeVertical = (_context.Parameters.
                SingleOrDefault(s => s.eventId == eventId)
                .EventType[0] == 'V');

            measurementTypeEnglish = (_context.Meets
                .SingleOrDefault(s => s.meetId == (_context.Events
                .SingleOrDefault(r => r.eventId == eventId)).meetId)
                .MeasurementType[0] == 'E');
        }

        public async Task<EventMarks> GetAttempts(int entryId, CancellationToken ct)
        {
            EventMarks em = new EventMarks();

            if (eventTypeVertical)
            {
                marks = _context.VerticalMarks.Where(
                    r => r.entryId == entryId);

                em.MarkType = MarkTypes.Vertical;
            }

            else if (measurementTypeEnglish)
            {
                marks = _context.EnglishHorizontalMarks.Where(
                    r => r.entryId == entryId);

                em.MarkType = MarkTypes.English;
            }

            else
            {
                marks = _context.MetricHorizontalMarks.Where(
                     r => r.entryId == entryId);

                em.MarkType = MarkTypes.Metric;
            }


            // To Collection
            em.Attempts = await marks
                .ProjectTo<Attempt>()
                .ToArrayAsync(ct);

            return em;
        }
    }
}
