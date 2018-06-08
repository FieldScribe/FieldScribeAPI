using FieldScribeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FieldScribeAPI.Services
{
    public interface ILynxService
    {
        Task<int> UpdateLynxDataAsync(
            int meetId,
            List<LynxEvent> newEvents,
            List<LynxAthlete> newAthletes,
            CancellationToken ct);
    }
}
