using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BELibrary.Core.Entity.Repositories;

namespace BELibrary.Core.Entity
{
    public interface IUnitOfWork : IDisposable
    {
        IAdminRepository Admins { get; }
        IBannerRepository Banners { get; }
        ICinemaRoomRepository CinemaRooms { get; }
        ICommentRepository Comments { get; }
        IDaysOfWeekRepository DaysOfWeeks { get; }
        IFilmRepository Films { get; }
        IFilmMovieDisplayTypeRepository FilmMovieDisplayTypes { get; }
        IFilmMovieTypeRepository FilmMovieTypes { get; }
        IMovieCalendarRepository MovieCalendars { get; }
        IMovieDisplayTypeRepository MovieDisplayTypes { get; }
        IMovieTypeRepository MovieTypes { get; }
        INewsRepository News { get; }
        IOrderDetailRepository OrderDetails { get; }
        IOrderRepository Orders { get; }
        IPromotionRepository Promotions { get; }
        IRoleRepository Roles { get; }
        ISeatRepository Seats { get; }
        ISeatTypeRepository SeatTypes { get; }
        ITimeFrameRepository TimeFrames { get; }
        IToppingRepository Toppings { get; }
        IToppingOrderDetailRepository ToppingOrders { get; }
        IUserRepository Users { get; }
        IGalleryRepository Galleries { get; }

        int Complete();
    }
}