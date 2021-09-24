using System;
using System.Linq;
using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Utils;

namespace BELibrary.Persistence.Repositories
{
    public class MovieDisplayTypeRepository : Repository<MovieDisplayType>, IMovieDisplayTypeRepository
    {
        public MovieDisplayTypeRepository(CinemaOnlineDbContext context)
            : base(context)
        {
        }

        public CinemaOnlineDbContext CinemaOnlineDbContext
        {
            get { return Context as CinemaOnlineDbContext; }
        }
    }
}