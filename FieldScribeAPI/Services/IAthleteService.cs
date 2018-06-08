using FieldScribeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using FieldScribeAPI.Infrastructure;

namespace FieldScribeAPI.Services
{
    public interface IAthleteService
    {
        Task<Athlete> GetAthleteAsync(
            int id,
            CancellationToken ct);

        Task<PagedResults<Athlete>> GetAthletesAsync(
            PagingOptions pagingOptions,
            SortOptions<Athlete, AthleteEntity> sortOptions,
            SearchOptions<Athlete, AthleteEntity> searchOptions,
            CancellationToken ct);

        Task<PagedResults<Athlete>> GetAthletesByMeetAsync(
            int id,
            PagingOptions pagingOptions,
            SortOptions<Athlete, AthleteEntity> sortOptions,
            SearchOptions<Athlete, AthleteEntity> searchOptions,
            CancellationToken ct);

        Task<int> CreateAthleteAsync(
            int meetId,
            int compNumber,
            string firstName,
            string lastName,
            string teamName,
            string gender,
            CancellationToken ct);

    }
}
