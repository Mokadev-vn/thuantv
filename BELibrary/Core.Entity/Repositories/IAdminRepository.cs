using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BELibrary.Entity;

namespace BELibrary.Core.Entity.Repositories
{
    //this.Configuration.LazyLoadingEnabled = false;
    public interface IAdminRepository : IRepository<Admin>
    {
        Admin ValidBEAccount(string username, string password);

        Admin GetAccountByUsername(string email);
    }
}