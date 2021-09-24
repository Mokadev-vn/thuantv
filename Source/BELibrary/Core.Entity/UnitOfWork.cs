using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Persistence.Repositories;

namespace BELibrary.Core.Entity
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CinemaOnlineDbContext _context;

        public UnitOfWork(CinemaOnlineDbContext context)
        {
            _context = context;
            Admins = new AdminRepository(_context);
            MovieTypes = new MovieTypeRepository(_context);
            Banners = new BannerRepository(_context);
            CinemaRooms = new CinemaRoomRepository(_context);
            Comments = new CommentRepository(_context);
            DaysOfWeeks = new DaysOfWeekRepository(_context);
            Films = new FilmRepository(_context);
            FilmMovieDisplayTypes = new FilmMovieDisplayTypeRepository(_context);
            FilmMovieTypes = new FilmMovieTypeRepository(_context);
            MovieCalendars = new MovieCalendarRepository(_context);
            MovieDisplayTypes = new MovieDisplayTypeRepository(_context);
            MovieTypes = new MovieTypeRepository(_context);
            News = new NewsRepository(_context);
            OrderDetails = new OrderDetailRepository(_context);
            Orders = new OrderRepository(_context);
            Promotions = new PromotionRepository(_context);
            Roles = new RoleRepository(_context);
            Seats = new SeatRepository(_context);
            SeatTypes = new SeatTypeRepository(_context);
            TimeFrames = new TimeFrameRepository(_context);
            Toppings = new ToppingRepository(_context);
            ToppingOrders = new ToppingOrderDetailRepository(_context);
            Users = new UserRepository(_context);
            Galleries = new GalleryRepository(_context);
        }

        public IAdminRepository Admins { get; private set; }
        public IBannerRepository Banners { get; private set; }
        public ICinemaRoomRepository CinemaRooms { get; private set; }
        public ICommentRepository Comments { get; private set; }
        public IDaysOfWeekRepository DaysOfWeeks { get; private set; }
        public IFilmRepository Films { get; private set; }
        public IFilmMovieDisplayTypeRepository FilmMovieDisplayTypes { get; private set; }
        public IFilmMovieTypeRepository FilmMovieTypes { get; private set; }
        public IMovieCalendarRepository MovieCalendars { get; private set; }
        public IMovieDisplayTypeRepository MovieDisplayTypes { get; private set; }
        public IMovieTypeRepository MovieTypes { get; private set; }
        public INewsRepository News { get; private set; }
        public IOrderDetailRepository OrderDetails { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IPromotionRepository Promotions { get; private set; }
        public IRoleRepository Roles { get; private set; }
        public ISeatRepository Seats { get; private set; }
        public ISeatTypeRepository SeatTypes { get; private set; }
        public ITimeFrameRepository TimeFrames { get; private set; }
        public IToppingRepository Toppings { get; private set; }
        public IToppingOrderDetailRepository ToppingOrders { get; private set; }

        public IUserRepository Users { get; private set; }

        public IGalleryRepository Galleries { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}