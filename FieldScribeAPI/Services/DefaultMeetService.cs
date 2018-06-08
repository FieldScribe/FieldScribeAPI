using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FieldScribeAPI.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.AspNetCore.Mvc;

namespace FieldScribeAPI.Services
{
    public class DefaultMeetService : IMeetService
    {
        private readonly FieldScribeAPIContext _context;

        public DefaultMeetService(FieldScribeAPIContext context)
        {
            _context = context;
        }

        public async Task<int> CreateMeetAsync(string meetName, DateTime meetDate,
            string meetLocation, string measurementType, CancellationToken ct)
        {
            
            var newMeet = _context.Meets.Add(new MeetEntity
            {
                MeetName = meetName,
                MeetDate = meetDate,
                MeetLocation = meetLocation,
                MeasurementType = measurementType,
            });

            var created = await _context.SaveChangesAsync();
            if (created < 1) throw new InvalidOperationException("Could not create and add the athlete");

            return _context.Meets.Last().meetId;
        }


        public async Task<Meet> GetMeetAsync(int id, CancellationToken ct)
        {
            var entity = await _context.Meets.SingleOrDefaultAsync(r => r.meetId == id, ct);
            if (entity == null)
            {
                return null;
            }

            return Mapper.Map<Meet>(entity);
        }

        public async Task<PagedResults<Meet>> GetMeetsAsync(
            PagingOptions pagingOptions, 
            SortOptions<Meet, MeetEntity> sortOptions, 
            SearchOptions<Meet, MeetEntity> searchOptions, 
            CancellationToken ct)
        {
            IQueryable<MeetEntity> query = _context.Meets;
            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync(ct);

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<Meet>().ToArrayAsync(ct);

            return new PagedResults<Meet>
            {
                Items = items,
                TotalSize = size,
            };
        }


        public async Task<(bool Succeeded, string Error)> EditMeetAsync(
            int meetId, string meetName, DateTime meetDate, string meetLocation,
            string measurementType, CancellationToken ct)
        {
            var meet = await _context.Meets.SingleOrDefaultAsync(
                r => r.meetId == meetId, ct);

            if (meet == null)
            {
                return (false, "Meet not found");
            }

            meet.MeetName = meetName;
            meet.MeetDate = meetDate;
            meet.MeetLocation = meetLocation;
            
            if(measurementType != meet.MeasurementType)
            {
                var events = _context.Events.Where(
                    s => s.meetId == meet.meetId);

                foreach (EventEntity ee in events)
                {
                    var entries = _context.Entries.Where(
                        s => s.eventId == ee.eventId);

                    // Remove Marks and Bar Heights
                    if (meet.MeasurementType == "English")
                    {
                        RemoveEnglishMarks(entries);
                    }
                    else
                    {
                        RemoveMetricMarks(entries);
                    }

                    RemoveVerticalMarks(entries);

                    foreach (EntryEntity ent in entries)
                    {
                        _context.Remove(ent);
                    }
                }
            }

            meet.MeasurementType = measurementType;

            _context.Meets.Update(meet);

            _context.SaveChanges();

            return (true, null);
        }


        public async Task<(bool Succeeded, string Error)> DeleteMeetAsync(
            int id, CancellationToken ct)
        {
            var meet = await _context.Meets.SingleOrDefaultAsync(
                r => r.meetId == id, ct);

            if(meet == null)
            {
                return (false, "Meet not found");
            }

            var claims = _context.UserClaims.Where(
                e => e.ClaimType == "scribe" 
                && e.ClaimValue == meet.meetId.ToString());

            // Remove User Claims for meet
            foreach (var uc in claims)
                _context.Remove(uc);

            // Remove Meet
            _context.Meets.Remove(meet);

            _context.SaveChanges();

            return (true, null);
        }


        private void RemoveEnglishMarks(
            IQueryable<EntryEntity> entries)
        {
            // Remove English Bar Heights
            var e_barheights = _context.EnglishBarHeights.SingleOrDefault(
                e => e.eventId == entries.First().eventId);

            _context.Remove(e_barheights);

            foreach (EntryEntity ent in entries)
            {
                // Remove English Horizontal Marks
                var eh_marks = _context.EnglishHorizontalMarks.Where(
                    e => e.entryId == ent.entryId);

                foreach (EHorizontalMarkEntity ehm in eh_marks)
                    _context.Remove(ehm);
            }
        }

        private void RemoveMetricMarks(
            IQueryable<EntryEntity> entries)
        {
            // Remove Metric Bar Heights
            var m_barheights = _context.MetricBarHeights.SingleOrDefault(
                e => e.eventId == entries.First().eventId);

            _context.Remove(m_barheights);
            foreach (EntryEntity ent in entries)
            {

                // Remove Metric Horizontal Marks
                var mh_marks = _context.MetricHorizontalMarks.Where(
                    e => e.entryId == ent.entryId);

                foreach (MHorizontalMarkEntity mhm in mh_marks)
                    _context.Remove(mhm);
            }
        }

        private void RemoveVerticalMarks(
            IQueryable<EntryEntity> entries)
        {
            foreach (EntryEntity ent in entries)
            {
                var v_marks = _context.VerticalMarks.Where(
                    e => e.entryId == ent.entryId);

                foreach (VerticalMarkEntity vme in v_marks)
                    _context.Remove(vme);
            }
        }
    }
}


