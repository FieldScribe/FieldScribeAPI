using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Formatters;
using FieldScribeAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using FieldScribeAPI.Filters;
using Microsoft.EntityFrameworkCore;
using FieldScribeAPI.Models;
using FieldScribeAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FieldScribeAPI
{
    public class Startup
    {
        private readonly int? _httpsPort;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;

            // Get the HTTPS port (only in development)
            if(env.IsDevelopment())
            {
                var launchJsonConfig = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("Properties\\launchSettings.json")
                    .Build();
                _httpsPort = launchJsonConfig.GetValue<int>("iisSettings:iisExpress:sslPort");
            }
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Use an in-memory database for quick dev and testing
            
            //services.AddDbContext<FieldScribeAPIContext>(opt =>
            //{
            //    opt.UseInMemoryDatabase("FieldScribeDB");
            //    opt.UseOpenIddict();
            //});

            // Use real database
            services.AddDbContext<FieldScribeAPIContext>(opt => {
                opt.UseSqlServer(
                    @"Server=tcp:fieldscribe.database.windows.net,1433;Initial Catalog=OIT_JuniorProject_FieldScribe;"
                    + "Persist Security Info=False;User ID=fieldscribe.admin;Password=trackStats!;MultipleActiveResultSets=False;"
                   + "Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
                    );
                opt.UseOpenIddict();
            });

            // Add ASP.NET Core Identity
            services.AddIdentity<UserEntity, UserRoleEntity>()
                .AddEntityFrameworkStores<FieldScribeAPIContext>()
                .AddDefaultTokenProviders();

            // Map some of the default claim names to the proper OpenID Connect claim names
            services.Configure<IdentityOptions>(opt =>
            {
                opt.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                opt.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                opt.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;

                // Configure password requirements
                opt.Password.RequiredLength = 8;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireNonAlphanumeric = false;
            });

            services.AddAuthentication()
                .AddOAuthValidation();

            // Add OpenIddict services
            services.AddOpenIddict<Guid>(opt =>
            {
                opt.AddEntityFrameworkCoreStores<FieldScribeAPIContext>();
                opt.AddMvcBinders();
            
                opt.EnableTokenEndpoint("/token");
                opt.EnableRevocationEndpoint("/revoke");
                opt.AllowPasswordFlow();
            });

            services.AddAutoMapper();

            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(JsonExceptionFilter));
                opt.Filters.Add(typeof(LinkRewritingFilter));

                // Require HTTPS for all controllers
                opt.SslPort = _httpsPort;
                opt.Filters.Add(typeof(RequireHttpsAttribute));

                var jsonFormatter = opt.OutputFormatters.OfType<JsonOutputFormatter>().Single();
                opt.OutputFormatters.Remove(jsonFormatter);
                opt.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter));
            });

            services.AddRouting(opt => opt.LowercaseUrls = true);

            services.AddApiVersioning(opt =>
            {
                opt.ApiVersionReader = new MediaTypeApiVersionReader();
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.ReportApiVersions = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionSelector = new CurrentImplementationApiVersionSelector(opt);
            });

            services.Configure<PagingOptions>(Configuration.GetSection("DefaultPagingOptions"));
            
            services.AddScoped<IAthleteService, DefaultAthleteService>();
            services.AddScoped<IMeetService, DefaultMeetService>();
            services.AddScoped<IEventService, DefaultEventService>();
            services.AddScoped<IEntryService, DefaultEntryService>();
            services.AddScoped<IMarkService, DefaultMarkService>();
            services.AddScoped<IUserService, DefaultUserService>();
            services.AddScoped<IAuthService, DefaultAuthorizationService>();
            services.AddScoped<ILynxService, DefaultLynxService>();

            services.AddAuthorization(opt =>
           {
               opt.AddPolicy("IsAdminOrTimer",
                   p => p.RequireAuthenticatedUser().RequireRole("Admin","Timer"));
               opt.AddPolicy("IsAdmin",
                    p => p.RequireAuthenticatedUser().RequireRole("Admin"));
               opt.AddPolicy("IsScribe",
                   p => p.RequireAuthenticatedUser().RequireRole("Scribe"));
           });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            RoleManager<UserRoleEntity> roleManager, UserManager<UserEntity> userManager)
        {

            if (env.IsDevelopment())
            {
                //IServiceScopeFactory scopeFactory = app.ApplicationServices
                //    .GetRequiredService<IServiceScopeFactory>();

                //using (IServiceScope scope = scopeFactory.CreateScope())
                //{
                //    RoleManager<UserRoleEntity> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRoleEntity>>();

                //    // Add test role
                //    AddTestRole(roleManager).Wait();
                //}
                //var roleManager = app.ApplicationServices
                //    .GetRequiredService<RoleManager<UserRoleEntity>>();



                
                app.UseDeveloperExceptionPage();

                // Lynda Video Tutorial: Part 4 (seed the database with test data) 
                // InvalidOperationException is thrown
                // Athletes are currently added in the constructor of the AthleteController
                //var context = app.ApplicationServices.GetRequiredService<FieldScribeAPIContext>();               
                //AddTestData(context);
            }

            app.UseHsts(opt =>
            {
                opt.MaxAge(days: 180);
                opt.IncludeSubdomains();
                opt.Preload();
            });

            
            app.UseAuthentication();

            // Add roles to database
            RolesData.CreateRoles(roleManager).Wait();

            // Add admins and test users to database
            Admins.CreateAdmins(userManager).Wait();
            TestUsers.CreateTestUsers(userManager).Wait();

            app.UseMvc();
           
        }

        private static async Task AddTestRole(RoleManager<UserRoleEntity> roleManager)
        {
            await roleManager.CreateAsync(new UserRoleEntity("Admin"));
        }
    }
}
