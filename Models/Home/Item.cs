using OnlineFurnitureStore.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineFurnitureStore.Models.Home
{
    public class Item
    {
        public Tbl_Product Product { get; set; }

        public Tbl_Cart Cart { get; set; }
        public int Quantity { get; set; }
    }
}