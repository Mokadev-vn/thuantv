using System.Linq;
using BELibrary.Core.Entity.Repositories;
using BELibrary.Core.Utils;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Utils;

namespace BELibrary.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CinemaOnlineDbContext context)
            : base(context)
        {
        }

        public User ValidFEAccount(string username, string password)
        {
            var db = CinemaOnlineDbContext;

            var passwordFactory = password + VariableExtensions.KeyCryptorClient;
            var passwordCryptor = CryptorEngine.Encrypt(passwordFactory, true);

            var account =
                  db.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower()
                  && x.Password == passwordCryptor
                  && !x.IsDelete);

            return account;
        }

        public User GetAccountByUsername(string username)
        {
            //need implement ,bad
            var db = CinemaOnlineDbContext;

            var account =
                  db.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower()
                  && !x.IsDelete);

            return account;
        }

        public CinemaOnlineDbContext CinemaOnlineDbContext => Context;
    }
}