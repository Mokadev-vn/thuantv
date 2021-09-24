using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BELibrary.Models;
using BELibrary.Models.View;
using ToppingDto = BELibrary.Models.ToppingDto;

namespace BELibrary.Extendsions
{
    public static class Model
    {
        public static string GetKindOfToppingStr(this ToppingDto obj)
        {
            return obj.KindOfTopping.Split('|')[0];
        }

        public static int GetKindOfToppingEnum(this ToppingDto obj)
        {
            return Convert.ToInt32(obj.KindOfTopping.Split('|')[1]);
        }

        public static string GetKindOfPromotionStr(this PromotionDto obj)
        {
            return obj.KindOfPromotion.Split('|')[0];
        }

        public static int GetKindOfPromotionEnum(this PromotionDto obj)
        {
            return Convert.ToInt32(obj.KindOfPromotion.Split('|')[1]);
        }

        public static string GetKeyItemModel(this ItemModel obj)
        {
            return obj.Name + '|' + obj.Value;
        }

        public static string GetId(this ReserveTicketDto obj)
        {
            return obj.UserId.ToString() + obj.MovieCalendarId.ToString();
        }
    }
}