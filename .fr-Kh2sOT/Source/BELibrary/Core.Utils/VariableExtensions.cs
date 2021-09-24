using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BELibrary.Core.Utils
{
    public static class SessionKey
    {
        public const string User = "user";
        public const string RegUser = "RegUser";
        public const string Admin = "admin";
    }

    public static class FileKey
    {
        public const int MaxLength = 1024 * 1000;

        public static List<string> FileExtensionApprove()
        {
            return new List<string>(new[] { "png", "jpg", "jpeg" });
        }

        public static List<string> FileContentTypeApprove()
        {
            return new List<string>(new[]
            {
                "application/pdf",
                "application/vnd.ms-excel",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            });
        }
    }

    public static class CookiesKey
    {
        public const string LangCode = "cinema_lang_code";
        public const string Admin = "cinema_admin_cookies";
        public const string Client = "cinema_client_cookies";
    }

    public static class LangCode
    {
        public const string English = "en";
        public const string VietNam = "vi";
        public const string Japan = "ja";
        public const string Default = "en";

        public static string GetText(string code)
        {
            switch (code)
            {
                case "en":
                    return "English";

                case "vi":
                    return "Việt Nam";

                case "ja":
                    return "Japan";

                default:
                    return "Unknown";
            }
        }

        public static List<string> GetList()
        {
            return new List<string>(new[] { "en", "vi", "ja" });
        }

        public static List<SelectListModel> GetDic()
        {
            return new List<SelectListModel>() {
                new SelectListModel{Value="vi",Text="Tiếng việt" },
                new SelectListModel{Value="en",Text="Tiếng anh" },
                new SelectListModel{Value="ja",Text="Tiếng nhật" }
            };
        }
    }

    public static class RoleKey
    {
        public const int Admin = 1;
        public const int Employee = 2;

        public static List<int> GetList()
        {
            return new List<int>(new[] { 1, 2 });
        }

        public static bool Any(int role)
        {
            return GetList().Any(x => x == role);
        }

        public static string GetRole(int role)
        {
            switch (role)
            {
                case 1:
                    return "Admin";

                case 2:
                    return "Employee";

                default:
                    return "Unknown";
            }
        }

        public static string GetRoleText(int role)
        {
            switch (role)
            {
                case 1:
                    return "Quản trị";

                case 2:
                    return "Nhân viên";

                default:
                    return "Unknown";
            }
        }
    }

    public static class GenderKey
    {
        public const int Male = 1;
        public const int FeMale = 0;

        public static List<SelectListModel> GetDic()
        {
            return new List<SelectListModel>() {
                new SelectListModel{Value=1,Text="Nam" },
                new SelectListModel{Value=0,Text="Nữ" }
            };
        }

        public static string GetEmployeeType(int type)
        {
            switch (type)
            {
                case 0:
                    return "Nữ";

                case 1:
                    return "Nam";

                default:
                    return "Unknown";
            }
        }
    }

    public static class VariableExtensions
    {
        public static int PageSize = 2;

        public static string KeyCryptor = "#!2020";
        public static string KeyCryptorClient = "#!2020_Client##";
        public static string DefautlPassword = "Oanh@123";
    }

    public static class StatusOrder
    {
        public const int Success = 1;
        public const int Pending = 0;
        public const int Failure = -1;

        public static string GetText(int stt)
        {
            switch (stt)
            {
                case 1:
                    return "Thành công";

                case 0:
                    return "Đang chờ";

                case -1:
                    return "Thất bại";

                default:
                    return "Unknown";
            }
        }

        public static string GetRoleText(int stt)
        {
            switch (stt)
            {
                case 1:
                    return "Đã duyệt";

                case 0:
                    return "Đang chờ";

                case -1:
                    return "Từ chối";

                default:
                    return "Unknown";
            }
        }
    }

    public enum StatusCode
    {
        Success = 200,
        NotFound = 404,
        NotForbidden = 403,
        ServerError = 500
    }

    public static class CountryKey
    {
        public static List<SelectListModel> GetAll()
        {
            List<SelectListModel> lst = new List<SelectListModel>();
            CultureInfo[] cultureInfos = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (var item in cultureInfos)
            {
                RegionInfo regionInfo = new RegionInfo(item.LCID);
                if (!(lst.Any(x => x.Text == regionInfo.EnglishName)))
                {
                    lst.Add(new SelectListModel
                    {
                        Text = regionInfo.EnglishName,
                        Value = regionInfo.EnglishName
                    });
                }
            }
            return lst.OrderBy(o => o.Text).ToList();
        }
    }

    public class SelectListModel
    {
        public object Value { get; set; }
        public string Text { get; set; }
    }
}