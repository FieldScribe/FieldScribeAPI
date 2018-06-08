using FieldScribeAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FieldScribeAPI.Services
{
    public class DefaultAuthorizationService : IAuthService
    {
        private readonly FieldScribeAPIContext _context;
        private readonly IAuthorizationService _authService;

        public DefaultAuthorizationService(FieldScribeAPIContext context,
            IAuthorizationService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<bool> CanEditEvent(
            int eventId, ClaimsPrincipal user)
        {
            var meetId = _context.Events.Where(s => 
                s.eventId == eventId).SingleOrDefault().meetId;

            if (user.HasClaim(DefaultClaims.Scribe, meetId.ToString()))
                return true;

            var result = await _authService.AuthorizeAsync(
                user, "IsAdminOrTimer");

            return result.Succeeded;
        }
    }
}
