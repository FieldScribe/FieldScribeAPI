using AutoMapper;
using AutoMapper.QueryableExtensions;
using FieldScribeAPI.Infrastructure;
using FieldScribeAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FieldScribeAPI.Services
{
    public class DefaultAthleteService : IAthleteService
    {
        private readonly FieldScribeAPIContext _context;

        public DefaultAthleteService(FieldScribeAPIContext context)
        {
            _context = context;
        }

        public async Task<Athlete> GetAthleteAsync(int id, CancellationToken ct)
        {
            var entity = await _context
                .Athletes
                .SingleOrDefaultAsync(r => r.athleteId == id, ct);

            if (entity == null) return null;

            return Mapper.Map<Athlete>(entity);
        }

        public async Task<int> CreateAthleteAsync(
            int meetId,
            int compNumber,
            string firstName,
            string lastName,
            string teamName,
            string gender,
            CancellationToken ct)
        {
            var newAthlete = _context.Athletes.Add(new AthleteEntity
            {
                meetId = meetId,
                CompNum = compNumber,
                FirstName = firstName,
                LastName = lastName,
                TeamName = teamName,
                Gender = gender
            });

            var created = await _context.SaveChangesAsync();
            if (created < 1) throw new InvalidOperationException("Could not create and add the athlete");

            return _context.Athletes.Last().athleteId;
        }


        public async Task<PagedResults<Athlete>> GetAthletesAsync(
            PagingOptions pagingOptions,
            SortOptions<Athlete, AthleteEntity> sortOptions,
            SearchOptions<Athlete, AthleteEntity> searchOptions,
            CancellationToken ct)
        {
            IQueryable<AthleteEntity> query = _context.Athletes;
            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync(ct);

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<Athlete>()
                .ToArrayAsync(ct);

            return new PagedResults<Athlete>
            {
                Items = items,
                TotalSize = size
            };
        }


        public async Task<PagedResults<Athlete>> GetAthletesByMeetAsync(
            int id,
            PagingOptions pagingOptions,
            SortOptions<Athlete, AthleteEntity> sortOptions,
            SearchOptions<Athlete, AthleteEntity> searchOptions,
            CancellationToken ct)
        {
            IQueryable<AthleteEntity> query = _context.Athletes.Where(r => r.meetId == id);

            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync(ct);

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<Athlete>()
                .ToArrayAsync(ct);

            return new PagedResults<Athlete>
            {
                Items = items,
                TotalSize = size,
            };
        }
    }
}
