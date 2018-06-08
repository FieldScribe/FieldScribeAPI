using FieldScribeAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI
{
    public class FieldScribeAPIContext : IdentityDbContext<UserEntity, UserRoleEntity, Guid>
    {
        public FieldScribeAPIContext(DbContextOptions options)
            : base(options)
        {
            
        }

        public DbSet<AthleteEntity> Athletes { get; set; }

        public DbSet<MeetEntity> Meets { get; set; }

        public DbSet<EventEntity> Events { get; set; }

        public DbSet<EntryEntity> Entries { get; set; }

        public DbSet<EventEntryEntity> EventEntries { get; set; }

        public DbSet<EHorizontalMarkEntity> EnglishHorizontalMarks { get; set; }

        public DbSet<MHorizontalMarkEntity> MetricHorizontalMarks { get; set; }

        public DbSet<VerticalMarkEntity> VerticalMarks { get; set; }

        public DbSet<EBarHeightEntity> EnglishBarHeights { get; set; }

        public DbSet<MBarHeightEntity> MetricBarHeights { get; set; }

        public DbSet<ParametersEntity> Parameters { get; set; }
    }
}
