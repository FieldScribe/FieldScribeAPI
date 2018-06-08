using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FieldScribeAPI.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using FieldScribeAPI.Infrastructure;

namespace FieldScribeAPI.Services
{
    public class DefaultEventService : IEventService
    {
        private readonly FieldScribeAPIContext _context;
        private ParamsProcessor _paramsProcessor;

        public DefaultEventService(FieldScribeAPIContext context)
        {
            _context = context;
            _paramsProcessor = new ParamsProcessor(context);
        }

        public async Task<Event> GetEventByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _context
                .Events
                .SingleOrDefaultAsync(r => r.eventId == id, ct);

            if (entity == null) return null;

            var evt = Mapper.Map<Event>(entity);

            evt.Params = await _paramsProcessor
                .GetParameters(evt.eventId, ct);

            if (evt.Params.EventType[0] == 'V')

                evt.BarHeights = await new BarHeightsProcessor(
                    _context, evt.meetId).GetHeights(evt.eventId, ct);

            return evt;
        }

        public async Task<PagedResults<Event>> GetEventsByMeetAsync(
            int meetId,
            PagingOptions pagingOptions,
            SortOptions<Event, EventEntity> sortOptions,
            SearchOptions<Event, EventEntity> searchOptions,
            CancellationToken ct)
        {
            IQueryable<EventEntity> query = _context.Events
                .Where(r => r.meetId == meetId);

            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync(ct);

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<Event>()
                .ToArrayAsync(ct);
            

            BarHeightsProcessor bhp = new BarHeightsProcessor(_context, meetId);

            foreach(var evt in items)
            {
                evt.Params = await _paramsProcessor
                    .GetParameters(evt.eventId, ct);

                if (evt.Params.EventType[0] == 'V')
                
                    evt.BarHeights = await bhp.GetHeights(evt.eventId, ct);                
            }

            return new PagedResults<Event>
            {
                Items = items,
                TotalSize = size,
            };
        }

        public async Task<Collection<Event>> GetEventsByAthleteAsync(
            int athleteId,
            SortOptions<Event, EventEntity> sortOptions,
            SearchOptions<Event, EventEntity> searchOptions,
            CancellationToken ct)
        {

            var entries =
                _context.EventEntries.Where(r => r.athleteId == athleteId)
                .Select(r => r.eventId);

            IQueryable<EventEntity> query =
                _context.Events.Where(e => entries.Contains(e.eventId));
                             
            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var items = await query
                .ProjectTo<Event>()
                .ToArrayAsync(ct);

            BarHeightsProcessor bhp = new BarHeightsProcessor(
                _context, items[0].meetId);

            foreach (var evt in items)
            {
                evt.Params = await _paramsProcessor
                    .GetParameters(evt.eventId, ct);

                if (evt.Params.EventType[0] == 'V')
         
                    evt.BarHeights = await bhp.GetHeights(evt.eventId, ct);
            }

            return new Collection<Event>
            {
                Value = items
            };
        }

        public async Task<int> CreateEventAsync(
            int meetId,
            int eventNum,
            int roundNum,
            int flightNum,
            string eventName,
            CancellationToken ct)
        {
            // If (exists)
            //      Update
            // Else
            //      Add

            var newEvent = _context.Events.Add(new EventEntity
            {
                meetId = meetId,
                EventNum = eventNum,
                RoundNum = roundNum,
                FlightNum = flightNum,
                EventName = eventName
            });

            var created = await _context.SaveChangesAsync();
            if (created < 1) throw new InvalidOperationException("Could not create and add the event");

            return _context.Events.Last().eventId;
        }

        public async Task<int> AddOrUpdateEnglishBarHeightsAsync(
            BarHeightsForm<EnglishBarHeight> barHeightsForm,
            CancellationToken ct)
        {
            var query = _context.EnglishBarHeights
                .Where(e => e.eventId == barHeightsForm.eventId).ToArray();

            _context.EnglishBarHeights.RemoveRange(query);

            foreach (EnglishBarHeight bh in barHeightsForm.Heights)
            {
                _context.EnglishBarHeights
                    .Add(new EBarHeightEntity
                    {
                        eventId = barHeightsForm.eventId,
                        HeightNum = bh.HeightNum,
                        Feet = bh.Feet,
                        Inches = bh.Inches
                    });
            }

            var created = await _context.SaveChangesAsync(ct);

            if (created < 1) throw new InvalidOperationException
                     ("Could not Add/Update bar heights");

            return 0;
        }

        public async Task<int> AddOrUpdateMetricBarHeightsAsync(
            BarHeightsForm<MetricBarHeight> barHeightsForm,
            CancellationToken ct)
        {
            var query = _context.MetricBarHeights
                .Where(e => e.eventId == barHeightsForm.eventId).ToArray();

            _context.MetricBarHeights.RemoveRange(query);

            foreach (MetricBarHeight bh in barHeightsForm.Heights)
            {
                _context.MetricBarHeights
                    .Add(new MBarHeightEntity
                    {
                        eventId = barHeightsForm.eventId,
                        HeightNum = bh.HeightNum,
                        Meters = bh.Meters
                    });
            }

            var created = await _context.SaveChangesAsync(ct);

            if (created < 1) throw new InvalidOperationException
                     ("Could not Add/Update bar heights");

            return 0;
        }
    }
}
