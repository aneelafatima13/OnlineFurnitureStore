using OnlineFurnitureStore.DAL;
using OnlineFurnitureStore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineFurnitureStore.Controllers
{
    public class AdminController : Controller
    {
        private OnlineFurnitureStoreEntities db = new OnlineFurnitureStoreEntities();
        public GenericUnitOfWork _unitofwork = new GenericUnitOfWork();

        public List<SelectListItem> GetCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cat = _unitofwork.GetRepositoryInstance<Tbl_Category>().GetAllRecords();
            foreach (var item in cat)
            {
                list.Add(new SelectListItem
                {
                    Value = item.CategoryId.ToString(),
                    Text = item.CategoryName
                });
            }
            return list;
        }

        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Categories()
        {
            List<Tbl_Category> allcategories = _unitofwork.GetRepositoryInstance<Tbl_Category>().GetAllRecordsIQueryable().Where(i => i.IsDelete == false).ToList();
            return View(allcategories);
            //return View();
        }
        public ActionResult GetList()
        {

            var allcategories = db.Tbl_Category.ToList<Tbl_Category>();
            return Json(new { data = allcategories }, JsonRequestBehavior.AllowGet);
            

        }
        public ActionResult AddCategories()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategories(Tbl_Category tbl)
        {
            _unitofwork.GetRepositoryInstance<Tbl_Category>().Add(tbl);
            return RedirectToAction("Categories");
        }

        public ActionResult EditCategories(int categoryId)
        {
            return View(_unitofwork.GetRepositoryInstance<Tbl_Category>().GetFirstorDefault(categoryId));
        }

        [HttpPost]
        public ActionResult EditCategories(Tbl_Category category)
        {
            _unitofwork.GetRepositoryInstance<Tbl_Category>().Update(category);
            return RedirectToAction("Categories");
        }

        public ActionResult Products()
        {
            return View(_unitofwork.GetRepositoryInstance<Tbl_Product>().GetProduct());
        }

        public ActionResult AddProduct()
        {
            ViewBag.CategoryList = GetCategory();
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(Tbl_Product tbl, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);
                file.SaveAs(path);
            }
            tbl.ProductImage = pic;
            tbl.CreatedDate = DateTime.Now;
            _unitofwork.GetRepositoryInstance<Tbl_Product>().Add(tbl);
            return RedirectToAction("Products");
        }

        public ActionResult EditProduct(int productid)
        {
            ViewBag.CategoryList = GetCategory();
            return View(_unitofwork.GetRepositoryInstance<Tbl_Product>().GetFirstorDefault(productid));
        }

        [HttpPost]
        public ActionResult EditProduct(Tbl_Product tbl, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);
                file.SaveAs(path);
            }
            tbl.ProductImage = file != null ? pic : tbl.ProductImage;
            tbl.ModifiedDate = DateTime.Now;
            _unitofwork.GetRepositoryInstance<Tbl_Product>().Update(tbl);
            return RedirectToAction("Products");
        }

        
    }
}