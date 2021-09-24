using System;
using System.Linq;
using BELibrary.Core.Entity.Repositories;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Utils;

namespace BELibrary.Persistence.Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(CinemaOnlineDbContext context)
            : base(context)
        {
        }

        public CinemaOnlineDbContext CinemaOnlineDbContext
        {
            get { return Context as CinemaOnlineDbContext; }
        }
    }
}