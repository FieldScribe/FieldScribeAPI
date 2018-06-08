using System.Security.Claims;
using System.Threading.Tasks;

namespace FieldScribeAPI.Services
{
    public interface IAuthService
    {
        Task<bool> CanEditEvent(
            int eventId, ClaimsPrincipal user);
    }
}
