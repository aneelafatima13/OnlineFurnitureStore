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
    }
}