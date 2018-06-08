using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FieldScribeAPI.Models;
using FieldScribeAPI.Infrastructure;

namespace FieldScribeAPI.Services
{
    public class DefaultMarkService : IMarkService
    {
        private readonly FieldScribeAPIContext _context;
        private AttemptsProcessor _attProcessor;

        public DefaultMarkService(FieldScribeAPIContext context)
        {
            _context = context;            
        }

        public async Task<Collection<Attempt>> GetMarksByEntryAsync(
            int eventId,
            int entryId,
            CancellationToken ct)
        {
            _attProcessor = new AttemptsProcessor(_context, eventId);

            var items = await _attProcessor.GetAttempts(entryId, ct);

            return new Collection<Attempt>
            {
                Value = items.Attempts
            };
        }


        public async Task<int> AddOrUpdateVerticalMarksAsync(
            MarksForm <VerticalAttempt> marksForm, CancellationToken ct)
        {
            var query = _context.VerticalMarks
                .Where(e => e.entryId == marksForm.entryId).ToArray();

            _context.VerticalMarks.RemoveRange(query);

            foreach (VerticalAttempt att in marksForm.Marks)
            {
                _context.VerticalMarks.Add(new VerticalMarkEntity
                {
                    entryId = marksForm.entryId,
                    AttemptNum = att.AttemptNum,
                    Marks = att.Mark
                });
            }

            var created = await _context.SaveChangesAsync(ct);

            if (created < 1) throw new InvalidOperationException
                     ("Could not Add/Update marks");

            return 0;
        }


        public async Task<int> AddOrUpdateEHorizontalMarksAsync(
            MarksForm<EHorizontalAttempt> marksForm, 
            CancellationToken ct)
        {
            var query = _context.EnglishHorizontalMarks
                .Where(e => e.entryId == marksForm.entryId).ToArray();

            _context.EnglishHorizontalMarks.RemoveRange(query);

            foreach (EHorizontalAttempt att in marksForm.Marks)
            {
                _context.EnglishHorizontalMarks
                    .Add(new EHorizontalMarkEntity
                {
                    entryId = marksForm.entryId,
                    AttemptNum = att.AttemptNum,
                    Feet = att.Feet,
                    Inches = att.Inches,
                    Wind = att.Wind
                });
            }

            var created = await _context.SaveChangesAsync(ct);

            if (created < 1) throw new InvalidOperationException
                     ("Could not Add/Update marks");

            return 0;
        }

        public async Task<int> AddOrUpdateMHorizontalMarksAsync(
            MarksForm<MHorizontalAttempt> marksForm, 
            CancellationToken ct)
        {
            var query = _context.MetricHorizontalMarks
                .Where(e => e.entryId == marksForm.entryId).ToArray();

            _context.MetricHorizontalMarks.RemoveRange(query);

            foreach (MHorizontalAttempt att in marksForm.Marks)
            {
                _context.MetricHorizontalMarks
                    .Add(new MHorizontalMarkEntity
                {
                    entryId = marksForm.entryId,
                    AttemptNum = att.AttemptNum,
                    Meters = att.Meters,
                    Wind = att.Wind
                });
            }

            var created = await _context.SaveChangesAsync(ct);

            if (created < 1) throw new InvalidOperationException
                     ("Could not Add/Update marks");

            return 0;
        }
    }
}
