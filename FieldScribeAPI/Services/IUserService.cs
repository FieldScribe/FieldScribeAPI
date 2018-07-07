using FieldScribeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace FieldScribeAPI.Services
{
    public interface IUserService
    {
        Task<PagedResults<User>> GetUsersAsync(
            PagingOptions pagingOptions,
            SortOptions<User, UserEntity> sortOptions,
            SearchOptions<User, UserEntity> searchOptions,
            CancellationToken ct);

        Task<PagedResults<User>> GetScribesAsync(
            PagingOptions pagingOptions,
            SortOptions<User, UserEntity> sortOptions,
            SearchOptions<User, UserEntity> searchOptions,
            string[] searchTerms,
            CancellationToken ct);

        Task<PagedResults<User>> GetScribesByMeetAsync(
            PagingOptions pagingOptions,
            SortOptions<User, UserEntity> sortOptions,
            SearchOptions<User, UserEntity> searchOptions,
            int meetId,
            CancellationToken ct);

        Task<(bool Succeeded, string Error)> CreateUserAsync(RegisterForm form, string role);

        Task<(bool Succeeded, string Error)> EditUserAsync(EditUserForm form, UserEntity user);

        Task<(bool Succeeded, string Error)> AssignToMeetAsync(
            AssignToMeetForm form, CancellationToken ct);

        Task<(bool Succeeded, string Error)> RemoveFromMeetAsync(
            AssignToMeetForm form, CancellationToken ct);

        Task<User> GetUserAsync(ClaimsPrincipal user);

        Task<(bool Succeeded, string Error)> ResetPasswordAsync(
            UserEntity user, string newPassword);
    }
}
