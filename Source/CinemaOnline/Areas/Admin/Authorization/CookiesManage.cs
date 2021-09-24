using BELibrary.Core.Entity;
using BELibrary.DbContext;
using BELibrary.Entity;
using BELibrary.Utils;
using System;
using System.Web;
using BELibrary.Core.Utils;

namespace CinemaOnline.Areas.Admin.Authorization
{
    public class CookiesManage
    {
        public static bool Logined()
        {
            var cookiesClient = HttpContext.Current.Request.Cookies.Get(CookiesKey.Admin);
            if (cookiesClient != null)
            {
                var decodeCookie = CryptorEngine.Decrypt(cookiesClient.Value, true);

                var vals = decodeCookie.Split('|');

                var host = HttpContext.Current.Request.Url.Authority;

                if (host.ToLower() != vals[1].ToLower())
                {
                    return false;
                }

                using (var unitofwork = new UnitOfWork(new CinemaOnlineDbContext()))
                {
                    var us = unitofwork.Admins.GetAccountByUsername(vals[0]);
                    if (us != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public static bool ClientLogined()
        {
            var cookiesClient = HttpContext.Current.Request.Cookies.Get(CookiesKey.Client);
            if (cookiesClient != null)
            {
                var decodeCookie = CryptorEngine.Decrypt(cookiesClient.Value, true);

                var vals = decodeCookie.Split('|');

                var host = HttpContext.Current.Request.Url.Authority;

                if (host.ToLower() != vals[1].ToLower())
                {
                    return false;
                }

                using (var unitofwork = new UnitOfWork(new CinemaOnlineDbContext()))
                {
                    var us = unitofwork.Users.GetAccountByUsername(vals[0]);
                    if (us != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public static BELibrary.Entity.Admin GetUser()
        {
            var cookiesClient = HttpContext.Current.Request.Cookies.Get(CookiesKey.Admin);
            if (cookiesClient != null)
            {
                var decodeCookie = CryptorEngine.Decrypt(cookiesClient.Value, true);
                var vals = decodeCookie.Split('|');

                var host = HttpContext.Current.Request.Url.Authority;

                if (host.ToLower() != vals[1].ToLower())
                {
                    return null;
                }
                using (var unitofwork = new UnitOfWork(new CinemaOnlineDbContext()))
                {
                    var us = unitofwork.Admins.GetAccountByUsername(vals[0]);
                    if (us != default)
                    {
                        return us;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public static BELibrary.Entity.User GetClientUser()
        {
            var cookiesClient = HttpContext.Current.Request.Cookies.Get(CookiesKey.Client);
            if (cookiesClient != null)
            {
                var decodeCookie = CryptorEngine.Decrypt(cookiesClient.Value, true);
                var vals = decodeCookie.Split('|');

                var host = HttpContext.Current.Request.Url.Authority;

                if (host.ToLower() != vals[1].ToLower())
                {
                    return null;
                }
                using (var unitofwork = new UnitOfWork(new CinemaOnlineDbContext()))
                {
                    var us = unitofwork.Users.GetAccountByUsername(vals[0]);
                    if (us != default)
                    {
                        return us;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        //clear all session
        public static void ClearAll()
        {
            HttpContext.Current.Session.RemoveAll();
        }
    }
}