using AutoMapper;
using FieldScribeAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldScribeAPI.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AthleteEntity, Athlete>()
                // Link to self
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                 Link.To(nameof(Controllers.AthletesController.GetAthleteByIdAsync), 
                 new { athleteId = src.athleteId })))
                 // Link to Meet
                 .ForMember(dest => dest.Meet, opt => opt.MapFrom(src =>
                 Link.To(nameof(Controllers.MeetsController.GetMeetByIdAsync),
                 new { meetId = src.meetId })))
                 // Link to events for athletes
                 .ForMember(dest => dest.AthleteEvents, opt => opt.MapFrom(src =>
                 Link.To(nameof(Controllers.EventsController.GetEventsByAthleteAsync), 
                 new { athleteId = src.athleteId })));

            CreateMap<MeetEntity, Meet>()
                // Link to self
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                Link.To(nameof(Controllers.MeetsController.GetMeetByIdAsync), 
                new { meetId = src.meetId })))
                // Link to athletes for meet
                .ForMember(dest => dest.Athletes, opt => opt.MapFrom(src =>
                Link.To(nameof(Controllers.AthletesController.GetAthletesByMeetAsync), 
                new { meetId = src.meetId })))
                // Link to events for meet
                .ForMember(dest => dest.Events, opt => opt.MapFrom(src =>
                Link.To(nameof(Controllers.EventsController.GetEventsByMeetAsync), 
                new { meetId = src.meetId })));

            CreateMap<EventEntity, Event>()
                // Link to self
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                Link.To(nameof(Controllers.EventsController.GetEventByIdAsync),
                new { eventId = src.eventId })))
                // Link to meet
                .ForMember(dest => dest.Meet, opt => opt.MapFrom(src =>
                Link.To(nameof(Controllers.MeetsController.GetMeetByIdAsync),
                new { meetId = src.meetId })))
                .ForMember(dest => dest.Events, opt => opt.MapFrom(src =>
                Link.To(nameof(Controllers.EventsController.GetEventsByMeetAsync),
                new { meetId = src.meetId })))
                // Link to entries for event
                .ForMember(dest => dest.Entries, opt => opt.MapFrom(src =>
                Link.To(nameof(Controllers.EntriesController.GetEntriesByEventAsync),
                new { eventId = src.eventId })));

            CreateMap<EventEntryEntity, EventEntry>()
                // Link to self
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                Link.To(nameof(Controllers.EntriesController.GetEntryByIdAsync),
                new { entryId = src.entryId })))
                // Link to events
                .ForMember(dest => dest.Event, opt => opt.MapFrom(src =>
                Link.To(nameof(Controllers.EventsController.GetEventByIdAsync), 
                new { eventId = src.eventId})))
                // Link to entries for athlete
                .ForMember(dest => dest.AthleteEvents, opt => opt.MapFrom(src =>
                Link.To(nameof(Controllers.EventsController.GetEventsByAthleteAsync),
                new { athleteId = src.athleteId })));


            CreateMap<EHorizontalMarkEntity, Attempt>()
                .ForMember(dest => dest.Mark, opts => opts.MapFrom(
                    src => string.Format("{0}-{1}", src.Feet, src.Inches)));
            CreateMap<MHorizontalMarkEntity, Attempt>()
                .ForMember(dest => dest.Mark, opts => opts.MapFrom(
                    src => string.Format("{0}", src.Meters)));
            CreateMap<VerticalMarkEntity, Attempt>()
                .ForMember(dest => dest.Mark, opts => opts.MapFrom(
                    src => string.Format(src.Marks)));

            CreateMap<EBarHeightEntity, BarHeight>()
                .ForMember(dest => dest.Height, opts => opts.MapFrom(
                    src => string.Format("{0}-{1}", src.Feet, src.Inches)));
            CreateMap<MBarHeightEntity, BarHeight>()
                .ForMember(dest => dest.Height, opts => opts.MapFrom(
                    src => string.Format("{0}", src.Meters)));

            CreateMap<ParametersEntity, Parameters>();

            CreateMap<UserEntity, User>()
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                 Link.To(nameof(Controllers.UsersController.GetUserByIdAsync),
                 new { userId = src.Id })));
        }
    }
}
