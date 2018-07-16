using AutoMapper;
using FieldScribeAPI.Infrastructure;
using FieldScribeAPI.Models;
using FieldScribeAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FieldScribeAPI.Controllers
{
    [Route("/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IAuthorizationService _authService;
        private readonly PagingOptions _defaultPagingOptions;

        public UsersController(
            IUserService userService,
            UserManager<UserEntity> userManager,
            IAuthorizationService authService,
            IOptions<PagingOptions> defaultPagingOptions)
        {
            _userService = userService;
            _userManager = userManager;
            _authService = authService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }


        // [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet(Name = nameof(GetVisibleUsersAsync))]
        public async Task<IActionResult> GetVisibleUsersAsync(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<User, UserEntity> sortOptions,
            [FromQuery] SearchOptions<User, UserEntity> searchOptions,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var users = new PagedResults<User>();

            // if (User.Identity.IsAuthenticated)
            // {
            var canSeeEveryone = true;// await _authService
                                      //.AuthorizeAsync(User, "IsAdminOrTimer");

            if (canSeeEveryone)
            {
                users = await _userService.GetUsersAsync(
                    pagingOptions, sortOptions, searchOptions, ct);
            }
            else
            {
                var myself = await _userService.GetUserAsync(User);
                users.Items = new[] { myself };
                users.TotalSize = 1;
            }
            // }

            var collection = PagedCollection<User>.Create(
                Link.To(nameof(GetVisibleUsersAsync)),
                users.Items.ToArray(),
                users.TotalSize,
                pagingOptions);

            return Ok(collection);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("scribes", Name = nameof(GetScribesAsync))]
        public async Task<IActionResult> GetScribesAsync(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<User, UserEntity> sortOptions,
            [FromQuery] SearchOptions<User, UserEntity> searchOptions,
            [FromBody] SearchTermsForm searchTerms,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var users = new PagedResults<User>();

            if (User.Identity.IsAuthenticated)
            {
                var canSeeEveryone = await _authService
                    .AuthorizeAsync(User, "IsAdminOrTimer");

                if (canSeeEveryone.Succeeded)
                {
                    users = await _userService.GetScribesAsync(
                        pagingOptions, sortOptions, searchOptions,
                        searchTerms.SearchTerms, ct);
                }
                else
                {
                    var myself = await _userService.GetUserAsync(User);
                    users.Items = new[] { myself };
                    users.TotalSize = 1;
                }
            }

            var collection = PagedCollection<User>.Create(
                Link.To(nameof(GetVisibleUsersAsync)),
                users.Items.ToArray(),
                users.TotalSize,
                pagingOptions);

            return Ok(collection);
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("scribes/{meetId}", Name = nameof(GetScribesByMeetAsync))]
        public async Task<IActionResult> GetScribesByMeetAsync(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<User, UserEntity> sortOptions,
            [FromQuery] SearchOptions<User, UserEntity> searchOptions,
            int meetId,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var users = new PagedResults<User>();

            if (User.Identity.IsAuthenticated)
            {
                var canSeeEveryone = await _authService
                    .AuthorizeAsync(User, "IsAdminOrTimer");

                if (canSeeEveryone.Succeeded)
                {
                    users = await _userService.GetScribesByMeetAsync(
                        pagingOptions, sortOptions, searchOptions,
                        meetId, ct);
                }
                else
                {
                    var myself = await _userService.GetUserAsync(User);
                    users.Items = new[] { myself };
                    users.TotalSize = 1;
                }
            }

            var collection = PagedCollection<User>.Create(
                Link.To(nameof(GetVisibleUsersAsync)),
                users.Items.ToArray(),
                users.TotalSize,
                pagingOptions);

            return Ok(collection);
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("me", Name = nameof(GetMeAsync))]
        public async Task<IActionResult> GetMeAsync(CancellationToken ct)
        {
            if (User == null) return BadRequest();

            var user = await _userService.GetUserAsync(User);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{userId}", Name = nameof(GetUserByIdAsync))]
        public async Task<IActionResult> GetUserByIdAsync(Guid userId, CancellationToken ct)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (userId.ToString() == currentUser.Id.ToString())
            {
                if (currentUser == null) return NotFound();

                var user = await _userService.GetUserAsync(User);
                return Ok(user);
            }

            var canViewUsers = await _authService
                .AuthorizeAsync(User, "IsAdminOrTimer");

            if (canViewUsers.Succeeded)
            {
                var entity =
                    await _userManager.FindByIdAsync(userId.ToString());

                if (entity == null) return NotFound();
                return Ok(Mapper.Map<User>(entity));
            }

            return NotFound();
        }

        [HttpPost("scribe", Name = nameof(RegisterOfficialAsync))]
        public async Task<IActionResult> RegisterOfficialAsync(
            [FromBody] RegisterForm form, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            // Check if e-mail already exists
            if (await _userManager.FindByEmailAsync(form.Email) != null)
                return ConflictAction(); // --> Status Code 409: Conflict

            var (succeeded, error) = await _userService.CreateUserAsync(
                    form, DefaultRoles.Scribe);

            if (succeeded)
            {
                UserEntity newUser = await _userManager.FindByEmailAsync(form.Email);
                return Created(nameof(UsersController.GetUserByIdAsync),
                new { newUser.Id });
            }

            return BadRequest(new ApiError
            {
                Message = "Registation failed"
            });
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("timer", Name = nameof(RegisterTimerAsync))]
        public async Task<IActionResult> RegisterTimerAsync(
        [FromBody] RegisterForm form, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var canAddTimer = await _authService
                .AuthorizeAsync(User, "IsAdmin");

            if (canAddTimer.Succeeded)
            {
                var (succeeded, error) = await _userService.CreateUserAsync(
                    form, "Timer");

                if (succeeded) return Created(Url.Link(nameof(GetMeAsync), null), null);
            }

            return BadRequest(new ApiError
            {
                Message = "Registation failed"
            });
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("edit", Name = nameof(EditUserAsync))]
        public async Task<IActionResult> EditUserAsync(
           [FromBody] EditUserForm form, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var currentUser = await _userManager.GetUserAsync(User);

            // Check if UserId is current user's Id (reset own password)
            if (form.UserId == currentUser.Id)
            {
                (bool succeeded, string error) = await _userService.EditUserAsync
                   (form, currentUser);

                if (succeeded) return Ok();
                return BadRequest(new ApiError { Message = error });
            }

            var user = await _userManager.FindByIdAsync(form.UserId.ToString());

            // Check if current user id timer or admin and user (whose
            // password is being reset) is in "Scribe" role
            if (await _userManager.IsInRoleAsync(user, DefaultRoles.Scribe)
                && _authService.AuthorizeAsync(User, "IsAdminOrTimer").Result.Succeeded)
            {
                (bool succeeded, string error) = await _userService.EditUserAsync
                    (form, user);

                if (succeeded) return Ok();
                return BadRequest(new ApiError { Message = error });
            }

            return Unauthorized();
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("assign", Name = nameof(AddToMeetAsync))]
        public async Task<IActionResult> AddToMeetAsync(
            [FromBody] AssignToMeetForm form, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var canAddScribe = await _authService
                .AuthorizeAsync(User, "IsAdminOrTimer");

            if (canAddScribe.Succeeded)
            {
                var (succeeded, error) = await _userService.AssignToMeetAsync(form, ct);
                if (succeeded) return Ok();
            }

            return BadRequest(new ApiError
            {
                Message = "Failed to add scribe to meet"
            });
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("remove", Name = nameof(RemoveFromMeetAsync))]
        public async Task<IActionResult> RemoveFromMeetAsync(
        [FromBody] AssignToMeetForm form, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            AuthorizationResult canRemoveScribe = await _authService
                .AuthorizeAsync(User, "IsAdminOrTimer");

            if (canRemoveScribe.Succeeded)
            {
                var (succeeded, error) = await _userService.RemoveFromMeetAsync(form, ct);
                if (succeeded) return Ok();
            }

            return BadRequest(new ApiError
            {
                Message = "Failed to remove scribe from meet"
            });
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("delete", Name = nameof(DeleteUserAsync))]
        public async Task<IActionResult> DeleteUserAsync(
        [FromBody] Guid userId, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            UserEntity user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return BadRequest(new ApiError
                {
                    Message = "User does not exist"
                });

            var currentUser = await _userManager.GetUserAsync(User);

            // Check if UserId is current user's Id (reset own password)
            if (userId == currentUser.Id)
            {
                var (succeeded, error) = await _userService.DeleteUserAsync(user, ct);

                if (succeeded) return Ok();
                return BadRequest(new ApiError { Message = error });
            }           

            if (await _userManager.IsInRoleAsync(user, DefaultRoles.Scribe) &&
                (await _authService.AuthorizeAsync(User, "IsAdminOrTimer")).Succeeded)
            {
                var (succeeded, error) = await _userService.DeleteUserAsync(user, ct);

                if (succeeded) return Ok();
                return BadRequest(new ApiError { Message = error });
            }

            return BadRequest(new ApiError
            {
                Message = "Failed to delete account"
            });
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("password", Name = nameof(ResetPasswordAsync))]
        public async Task<IActionResult> ResetPasswordAsync(
            [FromBody] ResetPasswordForm form, CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var currentUser = await _userManager.GetUserAsync(User);

            // Check if UserId is current user's Id (reset own password)
            if (form.UserId.ToString() == currentUser.Id.ToString())
            {
                var (succeeded, error) = await _userService.ResetPasswordAsync
                    (currentUser, form.NewPassword);

                if (succeeded) return Ok();
                return BadRequest(new ApiError { Message = error });
            }

            var user = await _userManager.FindByIdAsync(form.UserId.ToString());

            // Else, check if current user id timer or admin and user (whose
            // password is being reset) is in "Scribe" role
            if (await _userManager.IsInRoleAsync(user, DefaultRoles.Scribe)
                && _authService.AuthorizeAsync(User, "IsAdminOrTimer").Result.Succeeded)
            {
                var (succeeded, error) = await _userService.ResetPasswordAsync
                    (user, form.NewPassword);

                if (succeeded) return Ok();
                return BadRequest(new ApiError { Message = error });
            }

            return Unauthorized();
        }

        // Helper Function

        private IActionResult ConflictAction()
        {
            Response.StatusCode = StatusCodes.Status409Conflict;
            return new EmptyResult();
        }
    }
}
