using FieldScribeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FieldScribeAPI.Services
{
    public interface IMeetService
    {
        Task<Meet> GetMeetAsync(
            int id,
            CancellationToken ct);

        Task<PagedResults<Meet>> GetMeetsAsync(
            PagingOptions pagingOptions,
            SortOptions<Meet, MeetEntity> sortOptions,
            SearchOptions<Meet, MeetEntity> searchOptions,
                CancellationToken ct);

        Task<int> CreateMeetAsync(
            string meetName,
            DateTime meetDate,
            string meetLocation,
            string measurementType,
            CancellationToken ct);

        Task<(bool Succeeded, string Error)> EditMeetAsync(
            int meetId,
            string meetName,
            DateTime meetDate,
            string meetLocation,
            string MeasurementType,
            CancellationToken ct);

        Task<(bool Succeeded, string Error)> DeleteMeetAsync(
            int id,
            CancellationToken ct);
    }
}
