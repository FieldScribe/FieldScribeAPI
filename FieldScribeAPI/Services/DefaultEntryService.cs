using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FieldScribeAPI.Models;
using FieldScribeAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore.Extensions.Internal;

namespace FieldScribeAPI.Services
{
    public class DefaultEntryService : IEntryService
    {
        private readonly FieldScribeAPIContext _context;

        public DefaultEntryService(FieldScribeAPIContext context)
        {
            _context = context;
        }

        public async Task<EventEntry> GetEntryByIdAsync(int entryId, CancellationToken ct)
        {
            var entity = await _context.EventEntries.SingleOrDefaultAsync
                (r => r.entryId == entryId, ct);

            if (entity == null) return null;

            var entry = Mapper.Map<EventEntry>(entity);

            EventMarks em = await new AttemptsProcessor(_context, entry.eventId)
                .GetAttempts(entryId, ct);

            entry.Marks = em.Attempts;

            entry.MarkType = em.MarkType;

            return entry;
        }

        public async Task<Collection<EventEntry>> GetEntriesByEventAsync(
            int eventId,
            SortOptions<EventEntry, EventEntryEntity> sortOptions,
            SearchOptions<EventEntry, EventEntryEntity> searchOptions,
            CancellationToken ct)
        {
            IQueryable<EventEntryEntity> query =
                _context.EventEntries.Where(r => r.eventId == eventId);

            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var items = await query
                .ProjectTo<EventEntry>()
                .ToArrayAsync(ct);

            AttemptsProcessor ap = new AttemptsProcessor(_context, eventId);

            foreach (var ent in items)
            {
                EventMarks em = await ap.GetAttempts(ent.entryId, ct);
                ent.Marks = em.Attempts;
                ent.MarkType = em.MarkType;
            }
            
                        
            return new Collection<EventEntry>
            {
                Value = items
            };
        }     
    }
}
