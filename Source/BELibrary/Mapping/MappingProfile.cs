using AutoMapper;
using BELibrary.Entity;
using BELibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BELibrary.Extendsions;
using BELibrary.Models.View;
using ToppingDto = BELibrary.Models.ToppingDto;

namespace BELibrary.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMapFromEntitiesToViewModels();
            CreateMapFromViewModelsToEntites();
        }

        private void CreateMapFromViewModelsToEntites()
        {
        }

        private void CreateMapFromEntitiesToViewModels()
        {
            CreateMap<MovieType, MovieTypeDto>();
            CreateMap<MovieTypeDto, MovieType>();
            CreateMap<MovieDisplayType, MovieDisplayTypeDto>();
            CreateMap<MovieDisplayTypeDto, MovieDisplayType>();
            CreateMap<CinemaRoom, CinemaRoomDto>();
            CreateMap<CinemaRoomDto, CinemaRoom>();
            CreateMap<Seat, SeatDto>();
            CreateMap<SeatDto, Seat>();
            CreateMap<SeatType, SeatTypeDto>();
            CreateMap<SeatTypeDto, SeatType>();
            CreateMap<FilmView, Film>();
            CreateMap<NewsDto, News>();
            CreateMap<News, NewsDto>();

            CreateMap<Topping, ToppingDto>()
                .ForMember(t => t.KindOfTopping, options => options.MapFrom(input => input.GetKindOfTopping()));
            CreateMap<ToppingDto, Topping>()
                .ForMember(t => t.KindOfTopping, options => options.MapFrom(input => input.GetKindOfToppingStr()))
                .ForMember(t => t.KindOfToppingEnum, options => options.MapFrom(input => input.GetKindOfToppingEnum()));

            CreateMap<Promotion, PromotionDto>()
                .ForMember(t => t.KindOfPromotion, options => options.MapFrom(input => input.GetKindOfPromotion()));
            CreateMap<PromotionDto, Promotion>()
                .ForMember(t => t.KindOfPromotion, options => options.MapFrom(input => input.GetKindOfPromotionStr()))
                .ForMember(t => t.KindOfPromotionEnum, options => options.MapFrom(input => input.GetKindOfPromotionEnum()));

            CreateMap<MovieCalendar, MovieCalendarDto>()
                .ForMember(u => u.FilmName, options => options.MapFrom(input => input.Film.Name))
                .ForMember(u => u.DaysOfWeekName, options => options.MapFrom(input => input.DaysOfWeek.Name))
                .ForMember(u => u.TimeFrameName, options => options.MapFrom(input => input.TimeFrame.Time.ToString()))
                .ForMember(u => u.MovieDisplayTypeName, options => options.MapFrom(input => input.MovieDisplayType.Name));
        }
    }
}