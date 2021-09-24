using System;
using System.Linq;
using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Utils;

namespace BELibrary.Persistence.Repositories
{
    public class SeatTypeRepository : Repository<SeatType>, ISeatTypeRepository
    {
        public SeatTypeRepository(CinemaOnlineDbContext context)
            : base(context)
        {
        }

        public CinemaOnlineDbContext CinemaOnlineDbContext
        {
            get { return Context as CinemaOnlineDbContext; }
        }
    }
}