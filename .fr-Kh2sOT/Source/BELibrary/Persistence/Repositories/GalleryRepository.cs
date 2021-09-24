using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BELibrary.Persistence.Repositories
{
    public class GalleryRepository : Repository<Gallery>, IGalleryRepository
    {
        public GalleryRepository(CinemaOnlineDbContext context)
           : base(context)
        {
        }

        public CinemaOnlineDbContext CinemaOnlineDbContext
        {
            get { return Context as CinemaOnlineDbContext; }
        }
    }
}