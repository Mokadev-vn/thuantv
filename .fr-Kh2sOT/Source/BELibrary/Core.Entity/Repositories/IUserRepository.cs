using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BELibrary.Entity;

namespace BELibrary.Core.Entity.Repositories
{
    //this.Configuration.LazyLoadingEnabled = false;
    public interface IUserRepository : IRepository<User>
    {
        User ValidFEAccount(string username, string password);

        User GetAccountByUsername(string email);
    }
}