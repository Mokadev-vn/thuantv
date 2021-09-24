using System;
using System.Collections.Generic;
using System.Linq;
using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Utils;

namespace BELibrary.Persistence.Repositories
{
    public class FilmMovieDisplayTypeRepository : Repository<FilmMovieDisplayType>, IFilmMovieDisplayTypeRepository
    {
        public FilmMovieDisplayTypeRepository(CinemaOnlineDbContext context)
            : base(context)
        {
        }

        public List<string> GetListFilmMovieDisplayType(Guid filmId)
        {
            using (var db = new CinemaOnlineDbContext())
            {
                var filmMovieDisplayTypes = db.FilmMovieDisplayTypes.Where(x => x.FilmId == filmId && !x.IsDelete);
                var movieDisplayTypes = db.MovieDisplayTypes.Where(x => !x.IsDelete);

                var result = (from fmdt in filmMovieDisplayTypes
                              join mdt in movieDisplayTypes on fmdt.MovieDisplayTypeId equals mdt.Id
                              select mdt.Name).ToList();

                return result;
            }
        }

        public CinemaOnlineDbContext CinemaOnlineDbContext
        {
            get { return Context as CinemaOnlineDbContext; }
        }
    }
}