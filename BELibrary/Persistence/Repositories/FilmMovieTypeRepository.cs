using System;
using System.Collections.Generic;
using System.Linq;
using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Utils;

namespace BELibrary.Persistence.Repositories
{
    public class FilmMovieTypeRepository : Repository<FilmMovieType>, IFilmMovieTypeRepository
    {
        public FilmMovieTypeRepository(CinemaOnlineDbContext context)
            : base(context)
        {
        }

        public List<string> GetListFilmMovieType(Guid filmId)
        {
            using (var db = new CinemaOnlineDbContext())
            {
                var filmMovieTypes = db.FilmMovieTypes.Where(x => x.FilmId == filmId && !x.IsDelete);
                var movieTypes = db.MovieTypes.Where(x => !x.IsDelete);

                var result = (from fmt in filmMovieTypes
                              join mt in movieTypes on fmt.MovieTypeId equals mt.Id
                              select mt.Name).ToList();

                return result;
            }
        }

        public CinemaOnlineDbContext CinemaOnlineDbContext
        {
            get { return Context as CinemaOnlineDbContext; }
        }
    }
}