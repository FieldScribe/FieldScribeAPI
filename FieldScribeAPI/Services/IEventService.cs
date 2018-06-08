using FieldScribeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FieldScribeAPI.Services
{
    public interface IEventService
    {
        Task<Event> GetEventByIdAsync(int id, CancellationToken ct);

        Task<PagedResults<Event>> GetEventsByMeetAsync(
            int id,
            PagingOptions pagingOptions,
            SortOptions<Event, EventEntity> sortOptions,
            SearchOptions<Event, EventEntity> searchOptions,
            CancellationToken ct);

        Task<Collection<Event>> GetEventsByAthleteAsync(
            int athleteId,
            SortOptions<Event, EventEntity> sortOptions,
            SearchOptions<Event, EventEntity> searchOptions,
            CancellationToken ct);

        Task<int> CreateEventAsync(
             int meetId,
             int EventNum,
             int RoundNum,
             int FlightNum,
             string EventName,
             CancellationToken ct);

        Task<int> AddOrUpdateEnglishBarHeightsAsync(
            BarHeightsForm<EnglishBarHeight> barHeightsForm,
            CancellationToken ct);

        Task<int> AddOrUpdateMetricBarHeightsAsync(
            BarHeightsForm<MetricBarHeight> barHeightsForm,
            CancellationToken ct);
    }
}
