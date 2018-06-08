using AutoMapper;
using AutoMapper.QueryableExtensions;
using FieldScribeAPI.Infrastructure;
using FieldScribeAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FieldScribeAPI.Services
{
    public class DefaultLynxService : ILynxService
    {
        private readonly FieldScribeAPIContext _context;
        private int _meetId;
        private AthleteEntity[] _currentAthletes;
        private List<LynxAthlete> _newAthletes;
        private List<LynxEvent> _newEvents;
        private Dictionary<int, int> _oldAthletes;
        private Dictionary<int, EventEntryEntity> _oldEntries;
        private Dictionary<EventKey, int> _oldEvents;
        private EventEntity[] _events;

        public DefaultLynxService(FieldScribeAPIContext context)
        {
            _context = context;
        }

        public async Task<int> UpdateLynxDataAsync(
            int meetId,
            List<LynxEvent> newEvents,
            List<LynxAthlete> newAthletes,
            CancellationToken ct)
        {
            _meetId = meetId;
            _newAthletes = newAthletes;
            _newEvents = newEvents;

            _oldAthletes = new Dictionary<int, int>();
            _oldEvents = new Dictionary<EventKey, int>();
            _oldEntries = new Dictionary<int, EventEntryEntity>();

            GetOldAthletes();
            AddOrUpdateAthletes();
            _context.SaveChanges();

            GetOldEvents();
            AddOrUpdateEvents();
            _context.SaveChanges();

            AddOrUpdateEntries();
            _context.SaveChanges();

            DeleteOldEntries();
            _context.SaveChanges();

            DeleteOldEvents();
            _context.SaveChanges();

            DeleteOldAthletes();
            _context.SaveChanges();

            return 0;
        }


        private void GetOldAthletes()
        {
            // Get old athletes from database

            _currentAthletes = _context.Athletes
                .Where(e => e.meetId == _meetId).ToArray();
          

            foreach (AthleteEntity ae in _currentAthletes)
            {
                _oldAthletes.Add(ae.CompNum, ae.athleteId);
            }

        }


        private void AddOrUpdateAthletes()
        { 
            // Check add or update
            foreach (LynxAthlete la in _newAthletes)
            {
                if (_oldAthletes.ContainsKey(la.CompNum))
                {
                    _oldAthletes.TryGetValue(la.CompNum, out int id);

                    var updateAthlete = _currentAthletes.SingleOrDefault
                        (s => s.athleteId == id);

                    updateAthlete.FirstName = la.FirstName;
                    updateAthlete.LastName = la.LastName;
                    updateAthlete.TeamName = la.TeamName;
                    updateAthlete.Gender = la.Gender;

                    _oldAthletes.Remove(la.CompNum);
                }

                else
                {
                    _context.Athletes.Add(new AthleteEntity
                    {
                        meetId = _meetId,
                        CompNum = la.CompNum,
                        FirstName = la.FirstName,
                        LastName = la.LastName,
                        TeamName = la.TeamName,
                        Gender = la.Gender
                    });
                }
            }
        }


        private void GetOldEvents()
        {
            _events = _context.Events
                .Where(e => e.meetId == _meetId).ToArray();

            foreach (EventEntity evt in _events)
            {
                _oldEvents.Add(
                    new EventKey
                    {
                        EventName = evt.EventName,
                        RoundNum = evt.RoundNum,
                        FlightNum = evt.FlightNum
                    },
                    evt.eventId);
            }
        }


        private void AddOrUpdateEvents()
        {
            foreach (LynxEvent le in _newEvents)
            {
                if (_oldEvents.ContainsKey(le.Key))
                {
                    _oldEvents.TryGetValue(le.Key, out int id);

                    var updateEvent = _events.SingleOrDefault
                        (s => s.eventId == id);

                    updateEvent.EventNum = le.EventNum;

                    _oldEvents.Remove(le.Key);
                }
                else
                {
                    _context.Events.Add(new EventEntity
                    {
                        meetId = _meetId,
                        EventNum = le.EventNum,
                        RoundNum = le.Key.RoundNum,
                        FlightNum = le.Key.FlightNum,
                        EventName = le.Key.EventName
                    });
                }
            }
        }


        private void AddOrUpdateEntries()
        {
            var updatedEvents = _context.Events
            .Where(e => e.meetId == _meetId);

            var eventGroups = _newEvents.GroupBy(
                e => e.EventNum);

            foreach (var result in eventGroups)
            {
                var matchEvents = updatedEvents
                    .Where(e => e.EventName ==
                    result.First().Key.EventName);

                var matchEntries = _context.EventEntries
                    .Where(e => matchEvents.Select(
                        r => r.eventId).Contains(e.eventId))
                    .ToArray();

                foreach (EventEntryEntity ee in matchEntries)
                {
                    _oldEntries.Add(ee.CompNum, ee);
                }

                foreach (LynxEvent le in result)
                {
                    var matchEvent = matchEvents.SingleOrDefault(
                        e => e.RoundNum == le.Key.RoundNum &&
                        e.FlightNum == le.Key.FlightNum);

                    foreach (LynxEntry ent in le.Entries)
                    {
                        if (_oldEntries.ContainsKey(ent.CompNum)) // --> Update entry
                        {
                            var updateEntry = matchEntries
                                .SingleOrDefault(e => e.CompNum == ent.CompNum);

                            updateEntry.eventId = matchEvent.eventId;
                            updateEntry.CompetePosition = ent.CompetePosition;

                            _oldEntries.Remove(ent.CompNum);
                        }
                        else // --> Add new entry
                        {
                            var athlete = _context.Athletes
                                .SingleOrDefault(s => s.CompNum == ent.CompNum
                                && s.meetId == _meetId);

                            _context.Entries.Add(
                                new EntryEntity
                                {
                                    eventId = matchEvent.eventId,
                                    athleteId = athlete.athleteId,
                                    CompetePosition = ent.CompetePosition
                                });
                        }
                    }
                }
            }
        }


        private void DeleteOldEntries()
        {
            foreach (KeyValuePair<int, EventEntryEntity> oe in _oldEntries)
            {
                var removeEntry = _context.Entries.SingleOrDefault(
                    s => s.entryId == oe.Value.entryId);

                _context.Entries.Remove(removeEntry);
            }
        }


        private void DeleteOldEvents()
        {
            foreach (KeyValuePair<EventKey, int> oe in _oldEvents)
            {

                // Delete other stuff first
                _context.Events.Remove(_events.FirstOrDefault(
                    s => s.eventId == oe.Value));
            }
        }


        private void DeleteOldAthletes()
        {
            foreach (KeyValuePair<int, int> oa in _oldAthletes)
            {
                _context.Athletes.Remove(_currentAthletes.FirstOrDefault(
                    s => s.athleteId == oa.Value));
            }
        }
    }
}
