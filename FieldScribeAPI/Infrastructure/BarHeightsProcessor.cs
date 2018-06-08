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
    public class BarHeightsProcessor
    {
        private readonly FieldScribeAPIContext _context;
        private IQueryable heights;
        private bool measurementTypeEnglish;

        public BarHeightsProcessor(FieldScribeAPIContext context,
            int meetId)
        {
            _context = context;

            measurementTypeEnglish = (_context.Meets
                .SingleOrDefault(s => s.meetId == meetId)
                .MeasurementType[0] == 'E');
        }

        public async Task<BarHeight[]> GetHeights(int eventId,
            CancellationToken ct)
        {

            if (measurementTypeEnglish)
                heights = _context.EnglishBarHeights.Where(
                    r => r.eventId == eventId);
            else
                heights = _context.MetricBarHeights.Where(
                    r => r.eventId == eventId);

            // To Collection
            var items = await heights
                .ProjectTo<BarHeight>()
                .ToArrayAsync(ct);

            return items;
        }
    }
}
