using OnlineFurnitureStore.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineFurnitureStore.Controllers
{
    public class Admin2Controller : Controller
    {
        OnlineFurnitureStoreEntities db = new OnlineFurnitureStoreEntities();
        // GET: Admin2
        public ActionResult MembersList()
        {
            var members = db.Tbl_Members.ToList();
            return View(members);
        }

        public ActionResult MembershipMembersList()
        {
            var members = db.Tbl_Members.Where(m => m.Password != null).ToList();
            return View(members);
        }

        public ActionResult NonMembershipMembersList()
        {
            var members = db.Tbl_Members.Where(m => m.Password == null).ToList();
            return View(members);
        }

        public ActionResult DailySaleReport()
        {
            var shippingDetails = db.Tbl_ShippingDetails.ToList();
            //var shippingDetails = db.Database.SqlQuery<Tbl_ShippingDetails>("EXEC GetShippingDetails").ToList();

            // Pass the shipping details to the view
            return View(shippingDetails);
        }

        public ActionResult DailySaleReportbyProduct()
        {
            var cartProductDetails = db.Tbl_Cart.ToList();
            return View(cartProductDetails);
        }

        public ActionResult DailySaleReportbyMembers()
        {
            var cartProductDetails = db.Tbl_Cart.ToList();
            return View(cartProductDetails);
        }
        public ActionResult DailySaleReportbyCity()
        {
            var cartProductDetails = db.Tbl_Cart.Where(c => c.Tbl_ShippingDetails.City == "Lahore").ToList();
            return View(cartProductDetails);
        }

        public ActionResult GetCartDetailsByCity(string city)
        {
            // Filter cart details based on the selected city
            var cartDetails = db.Tbl_Cart
                                .Where(c => c.Tbl_ShippingDetails.City == city)
                                .Select(c => new
                                {
                                    City = c.Tbl_ShippingDetails.City,
                                    MemberName = c.Tbl_Members.FirstName + " " + c.Tbl_Members.LastName,
                                    ProductName = c.Tbl_Product.ProductName,
                                    CartProductQuantity = c.CartProductQuantity,
                                    CartProductTotalPrice = c.CartProductTotalPrice,
                                    CartDate = c.CartDate,
                                    Address = c.Tbl_ShippingDetails.Adress,
                                    Country = c.Tbl_ShippingDetails.Country,
                                    AmountPaid = c.Tbl_ShippingDetails.AmountPaid,
                                    PaymentType = c.Tbl_ShippingDetails.PaymentType
                                })
                                .ToList();

            return Json(cartDetails, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DailySaleReportbyCountry()
        {
            var cartProductDetails = db.Tbl_Cart.Where(c => c.Tbl_ShippingDetails.Country == "Pakistan").ToList();
            return View(cartProductDetails);
        }
        public ActionResult GetCartDetailsByCountry(string country)
        {
            // Filter cart details based on the selected city
            var cartDetails = db.Tbl_Cart
                                .Where(c => c.Tbl_ShippingDetails.Country == country)
                                .Select(c => new
                                {
                                    Country = c.Tbl_ShippingDetails.Country,
                                    MemberName = c.Tbl_Members.FirstName + " " + c.Tbl_Members.LastName,
                                    ProductName = c.Tbl_Product.ProductName,
                                    CartProductQuantity = c.CartProductQuantity,
                                    CartProductTotalPrice = c.CartProductTotalPrice,
                                    CartDate = c.CartDate,
                                    Address = c.Tbl_ShippingDetails.Adress,
                                    City = c.Tbl_ShippingDetails.City,
                                    AmountPaid = c.Tbl_ShippingDetails.AmountPaid,
                                    PaymentType = c.Tbl_ShippingDetails.PaymentType
                                })
                                .ToList();

            return Json(cartDetails, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DailySaleReportbyPaymentType()
        {
            var cartProductDetails = db.Tbl_Cart.Where(c => c.Tbl_ShippingDetails.PaymentType == "CashOnDelivery").ToList();
            return View(cartProductDetails);
        }
        public ActionResult DailySaleReportbyCartStatus()
        {
            var cartProductDetails = db.Tbl_Cart.Where(c => c.CartStatusId == 1).ToList();
            return View(cartProductDetails);
        }

        public ActionResult RecentOrders()
        {
            var cartProductDetails = db.Tbl_Cart.ToList();
            return View(cartProductDetails);
        }

        public ActionResult PreviousMonthOrders()
        {
            // Get the current date
            DateTime currentDate = DateTime.Now;

            // Get the first day of the current month
            DateTime firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

            // Get the first day of the previous month
            DateTime firstDayOfPreviousMonth = firstDayOfCurrentMonth.AddMonths(-1);

            // Get the last day of the previous month
            DateTime lastDayOfPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);

            // Retrieve orders with CartDate in the previous month
            var cartProductDetails = db.Tbl_Cart
                                        .Where(c => c.CartDate >= firstDayOfPreviousMonth && c.CartDate <= lastDayOfPreviousMonth)
                                        .ToList();

            return View(cartProductDetails);
        }

    }
}