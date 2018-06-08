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
    public class ParamsProcessor
    {
        private readonly FieldScribeAPIContext _context;

        public ParamsProcessor(FieldScribeAPIContext context)
        {
            _context = context;
        }

        public async Task<Parameters> GetParameters(int eventId,
            CancellationToken ct)
        {
            IQueryable<ParametersEntity> query = _context.Parameters
                .Where(s => s.eventId == eventId);

            Parameters[] p = await query.ProjectTo<Parameters>()
                .ToArrayAsync(ct);

            return p[0];
        }
    }
}
