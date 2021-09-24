using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BELibrary.Models;

namespace BELibrary.Constant
{
    public class KindOfPromotion
    {
        public static List<ItemModel> UsingFilm = new List<ItemModel>
            {new ItemModel {Name = "Đồng giá", Value = 0}, new ItemModel {Name = "Trên phim", Value = 1}, new ItemModel {Name = "Hóa đơn", Value = 2}};

        public static List<ItemModel> UsingTopping = new List<ItemModel>
            {new ItemModel {Name = "Đồng giá", Value = 3}, new ItemModel {Name = "Trên topping", Value = 4}, new ItemModel {Name = "Hóa đơn", Value = 5}};

        public static List<ItemModel> TypeList = new List<ItemModel>
            {new ItemModel {Name = "Phim", Value = "true", Checked = "checked"}, new ItemModel {Name = "Topping", Value = "false", Checked = ""}};
    }
}