namespace BELibrary.DbContext
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using BELibrary.Entity;

    public partial class CinemaOnlineDbContext : DbContext
    {
        public CinemaOnlineDbContext()
            : base("name=CinemaOnlineDbContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public CinemaOnlineDbContext(string connectString) : base(connectString)
        {
        }

        public static CinemaOnlineDbContext Create()
        {
            return new CinemaOnlineDbContext();
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Banner> Banners { get; set; }
        public virtual DbSet<CinemaRoom> CinemaRooms { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<DaysOfWeek> DaysOfWeeks { get; set; }

        public virtual DbSet<Film> Films { get; set; }

        public virtual DbSet<FilmMovieDisplayType> FilmMovieDisplayTypes { get; set; }

        public virtual DbSet<FilmMovieType> FilmMovieTypes { get; set; }

        public virtual DbSet<MovieCalendar> MovieCalendars { get; set; }

        public virtual DbSet<MovieDisplayType> MovieDisplayTypes { get; set; }

        public virtual DbSet<MovieType> MovieTypes { get; set; }

        public virtual DbSet<News> News { get; set; }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<Promotion> Promotions { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<Seat> Seats { get; set; }

        public virtual DbSet<SeatType> SeatTypes { get; set; }

        public virtual DbSet<TimeFrame> TimeFrames { get; set; }

        public virtual DbSet<Topping> Toppings { get; set; }

        public virtual DbSet<ToppingOrderDetail> ToppingOrderDetails { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Gallery> Galleries { get; set; }
    }
}