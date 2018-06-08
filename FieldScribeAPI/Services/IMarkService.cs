using FieldScribeAPI.Models;
using System.Threading;
using System.Threading.Tasks;

namespace FieldScribeAPI.Services
{
    public interface IMarkService
    {
        Task<Collection<Attempt>> GetMarksByEntryAsync(
            int eventId, int entryId,
            CancellationToken ct);

        Task<int> AddOrUpdateVerticalMarksAsync(
            MarksForm<VerticalAttempt> marksForm,
            CancellationToken ct);

        Task<int> AddOrUpdateEHorizontalMarksAsync(
            MarksForm<EHorizontalAttempt> marksForm, 
            CancellationToken ct);

        Task<int> AddOrUpdateMHorizontalMarksAsync(
            MarksForm<MHorizontalAttempt> marksForm,
            CancellationToken ct);
    }
}
