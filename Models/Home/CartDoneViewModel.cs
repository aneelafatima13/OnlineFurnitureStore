using OnlineFurnitureStore.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineFurnitureStore.Models.Home
{
    public class CartDoneViewModel
    {
        public string MemberName { get; set; }
        public Tbl_ShippingDetails ShippingDetails { get; set; }
        public List<Tbl_Cart> LatestOrder { get; set; }
        public List<Tbl_Cart> PreviousOrders { get; set; }
    }
}