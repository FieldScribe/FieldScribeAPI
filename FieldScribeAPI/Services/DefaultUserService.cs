using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using FieldScribeAPI.Models;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace FieldScribeAPI.Services
{
    public class DefaultUserService : IUserService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly IAuthorizationService _authService;

        public DefaultUserService(UserManager<UserEntity> userManager,
            IAuthorizationService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        public async Task<PagedResults<User>> GetUsersAsync(
            PagingOptions pagingOptions,
            SortOptions<User, UserEntity> sortOptions,
            SearchOptions<User, UserEntity> searchOptions,
            CancellationToken ct)
        {
            IQueryable<UserEntity> query = _userManager.Users;
            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync(ct);

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<User>()
                .ToArrayAsync(ct);

            foreach (User u in items)
                u.Roles = await _userManager
                    .GetRolesAsync(query.FirstOrDefault(x => x.Id == u.Id));

            return new PagedResults<User>
            {
                Items = items,
                TotalSize = size
            };
        }


        public async Task<PagedResults<User>> GetScribesAsync(
            PagingOptions pagingOptions,
            SortOptions<User, UserEntity> sortOptions,
            SearchOptions<User, UserEntity> searchOptions,
            string[] searchTerms,
            CancellationToken ct)
        {

            IQueryable<UserEntity> query = _userManager.Users;

            var scribes = await _userManager.GetUsersInRoleAsync(DefaultRoles.Scribe);

            query = query.Where(x => scribes.Contains(x));

            if(searchTerms != null && searchTerms.Length > 0)
                query = query.Where(x => searchTerms.Any(y => x.Email.Contains(y))
                || searchTerms.Any(y => x.FirstName.Contains(y)));

            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync(ct);

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<User>()
                .ToArrayAsync(ct);

            return new PagedResults<User>
            {
                Items = items,
                TotalSize = size
            };
        }


        public async Task<(bool Succeeded, string Error)> CreateUserAsync(RegisterForm form, string role)
        {
            var entity = new UserEntity
            {
                Email = form.Email,
                UserName = form.Email,
                FirstName = form.FirstName,
                LastName = form.LastName,
                CreatedAt = DateTimeOffset.UtcNow
            };

            var result = await _userManager.CreateAsync(entity, form.Password);
            if (!result.Succeeded)
            {
                var firstError = result.Errors.FirstOrDefault()?.Description;
                return (false, firstError);
            }

            await _userManager.AddToRoleAsync(entity, role);

            return (true, null);
        }


        public async Task<(bool Succeeded, string Error)> EditUserAsync(
            EditUserForm form, UserEntity user)
        {
            user.Email = form.Email;
            user.FirstName = form.FirstName;
            user.LastName = form.LastName;

            IdentityResult ir = await _userManager.UpdateAsync(user);

            return (ir.Succeeded, (ir.Succeeded ? null : "Failed to update user"));
        }


        public async Task<(bool Succeeded, string Error)> AssignToMeetAsync(
            AssignToMeetForm form, CancellationToken ct)
        {
            var user = await _userManager.FindByIdAsync(form.UserId.ToString());

            if(user != null)
            {
                IdentityResult ir = await _userManager
                    .AddClaimAsync(user, new Claim(DefaultClaims.Scribe, form.meetId.ToString()));

                return (ir.Succeeded, (ir.Succeeded? null : "Failed to add user as scribe" ));
            }

            return (false, "User not found");
        }


        public async Task<(bool Succeeded, string Error)> RemoveFromMeetAsync(
            AssignToMeetForm form, CancellationToken ct)
        {
            var user = await _userManager.FindByIdAsync(form.UserId.ToString());

            if (user != null)
            {
                IdentityResult ir = await _userManager
                    .RemoveClaimAsync(user, new Claim(DefaultClaims.Scribe, form.meetId.ToString()));

                return (true, null);
            }

            return (false, "User not found");
        }


        public async Task<User> GetUserAsync(ClaimsPrincipal user)
        {
            var entity = await _userManager.GetUserAsync(user);

            var roles = await _userManager.GetRolesAsync(entity);

            var returnUser = Mapper.Map<User>(entity);

            returnUser.Roles = roles;

            return returnUser;
        }

        
        public async Task<(bool Succeeded, string Error)> ResetPasswordAsync(
            UserEntity user, string newPassword)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var reset = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (reset.Succeeded) return (true, "Password reset succeeded");
            return (false, "Password reset failed");
        }
    }
}
