using OnlineFurnitureStore.DAL;
using OnlineFurnitureStore.Models.Home;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using OnlineFurnitureStore.Repository;

namespace OnlineFurnitureStore.Controllers
{

    public class HomeController : Controller
    {
        OnlineFurnitureStoreEntities ctx = new OnlineFurnitureStoreEntities();
        public GenericUnitOfWork _unitofwork = new GenericUnitOfWork();
        // GET: Home
        public ActionResult Index(string search, int? page)
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            return View(model.CreateModel(search, 4, page));

        }
        public PartialViewResult CartPartial()
        {
            var cart = (List<Item>)Session["cart"];
            return PartialView("_CartList", cart);
        }
        public ActionResult AddToCart(int productId, int quantity, string url)
        {
            try
            {
                if (Session["cart"] == null)
                {
                    List<Item> cart = new List<Item>();
                    var product = ctx.Tbl_Product.Find(productId);
                    cart.Add(new Item()
                    {
                        Product = product,
                        Quantity = quantity // Use the provided quantity parameter
                    });
                    Session["cart"] = cart;
                }
                else
                {
                    List<Item> cart = (List<Item>)Session["cart"];
                    var existingItem = cart.FirstOrDefault(item => item.Product.ProductId == productId);

                    if (existingItem != null)
                    {
                        // If the product already exists in the cart, update its quantity
                        existingItem.Quantity += quantity;
                    }
                    else
                    {
                        var product = ctx.Tbl_Product.Find(productId);
                        cart.Add(new Item()
                        {
                            Product = product,
                            Quantity = quantity // Use the provided quantity parameter
                        });
                    }
                    Session["cart"] = cart;
                }

                // Return JSON indicating success
                return Json(new { success = true, message = "Item added to cart successfully!" });
            }
            catch (Exception ex)
            {
                // Return JSON indicating failure with error message
                return Json(new { success = false, message = "Error adding item to cart: " + ex.Message });
            }
        }

        //public ActionResult AddToCart(int productId, int quantity, string url)
        //{
        //    if (Session["cart"] == null)
        //    {
        //        List<Item> cart = new List<Item>();
        //        var product = ctx.Tbl_Product.Find(productId);
        //        cart.Add(new Item()
        //        {
        //            Product = product,
        //            Quantity = 1
        //        });
        //        Session["cart"] = cart;
        //    }
        //    else
        //    {
        //        List<Item> cart = (List<Item>)Session["cart"];
        //        var count = cart.Count();
        //        var product = ctx.Tbl_Product.Find(productId);
        //        for (int i = 0; i < count; i++)
        //        {
        //            if (cart[i].Product.ProductId == productId)
        //            {
        //                int prevQty = cart[i].Quantity;
        //                cart.Remove(cart[i]);
        //                cart.Add(new Item()
        //                {
        //                    Product = product,
        //                    Quantity = prevQty + 1
        //                });
        //                break;
        //            }
        //            else
        //            {
        //                var prd = cart.Where(x => x.Product.ProductId == productId).SingleOrDefault();
        //                if (prd == null)
        //                {
        //                    cart.Add(new Item()
        //                    {
        //                        Product = product,
        //                        Quantity = 1
        //                    });
        //                }
        //            }
        //        }
        //        Session["cart"] = cart;
        //    }
        //    return Redirect(url);
        //}

        //public ActionResult AddToCart(int productId, string url)
        //{
        //    if (Session["cart"] == null)
        //    {
        //        AddProductToCart(productId);
        //    }
        //    else
        //    {
        //        List<Item> cart = (List<Item>)Session["cart"];
        //        var existingItem = FindItemInCart(productId, cart);
        //        if (existingItem != null)
        //        {
        //            IncreaseQuantity(existingItem);
        //        }
        //        else
        //        {
        //            AddProductToCart(productId);
        //        }
        //    }
        //    return Redirect(url);
        //}

        //private void AddProductToCart(int productId)
        //{
        //    var product = ctx.Tbl_Product.Find(productId);
        //    List<Item> cart = new List<Item>();
        //    cart.Add(new Item()
        //    {
        //        Product = product,
        //        Quantity = 1
        //    });
        //    Session["cart"] = cart;
        //}

        //private Item FindItemInCart(int productId, List<Item> cart)
        //{
        //    return cart.FirstOrDefault(item => item.Product.ProductId == productId);
        //}

        //private void IncreaseQuantity(Item item)
        //{
        //    item.Quantity++;
        //}


        public ActionResult RemoveFromCartIndex(int productId)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            foreach (var item in cart)
            {
                if (item.Product.ProductId == productId)
                {
                    cart.Remove(item);
                    break;
                }
            }
            Session["cart"] = cart;
            return Redirect("Shop");
        }

        public ActionResult RemoveFromCartShop(int productId)
        {
            List<Item> cart = (List<Item>)Session["cart"];
            foreach (var item in cart)
            {
                if (item.Product.ProductId == productId)
                {
                    cart.Remove(item);
                    break;
                }
            }
            Session["cart"] = cart;
            return Redirect("Shop");
        }

        //public ActionResult Checkout()
        //{
        //    return View();
        //}

        //public ActionResult CheckoutDetails()
        //{
        //    return View();
        //}

        public ActionResult DecreaseQty(int productId)
        {
            if (Session["cart"] != null)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var product = ctx.Tbl_Product.Find(productId);
                foreach (var item in cart)
                {
                    if (item.Product.ProductId == productId)
                    {
                        int prevQty = item.Quantity;
                        if (prevQty > 0)
                        {
                            if (prevQty == 1) // If the quantity becomes 0 after decreasing
                            {
                                cart.Remove(item); // Remove the item from the cart
                            }
                            else
                            {
                                item.Quantity = prevQty - 1; // Decrease the quantity by 1
                            }
                        }
                        break;
                    }
                }
                Session["cart"] = cart;
            }
            return Redirect("Shop");
        }
        public ActionResult IncreaseQty(int productId)
        {
            if (Session["cart"] != null)
            {
                List<Item> cart = (List<Item>)Session["cart"];
                var product = ctx.Tbl_Product.Find(productId);
                foreach (var item in cart)
                {
                    if (item.Product.ProductId == productId)
                    {
                        int prevQty = item.Quantity;
                        if (prevQty > 0 && prevQty < product.Quantity)
                        {
                            cart.Remove(item);
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = prevQty + 1
                            });
                        }
                        break;
                    }
                }
                Session["cart"] = cart;
            }
            return Redirect("Shop");
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Shop(string ProductName, string Category, string Price, string Quantity, int? ActiveFlag, int? page)
        {
            var products = _unitofwork.GetProducts(ProductName, Category, Price, Quantity, ActiveFlag, page);

            //if (Request.IsAjaxRequest())
            //    return PartialView("_List", employees);
            //else
                return View(products);
        }
        //public ActionResult Shop(string search, int? page)
        //{
        //    HomeIndexViewModel model = new HomeIndexViewModel();
        //    return View(model.CreateModel(search, 20, page));
        //}

        public ActionResult Furniture()
        {
            return View();
        }

        public ActionResult Membership()
        {
            return View();
        }
        public ActionResult LoginMembership()
        {
            return View();
        }

        public ActionResult AllShippingDetails()
        {
            return View();
        }

       

        [HttpPost]
        public ActionResult LoginMembership(string email, string password)
        {
            // Simple validation (you should implement a more robust validation)
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Email and password are required.";
                return View();
            }

            // Check if the provided email and password match an employee
            var member = ctx.Tbl_Members.SingleOrDefault(e => e.EmailId == email && e.Password == password);

            if (member != null)
            {
                // Store the employee id in TempData
                Session["MemberId"] = member.MemberId;

                // Redirect to AttendanceSheet with the employee's ID
                return RedirectToAction("CheckoutDetails", new { id = member.MemberId });
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View();
            }
        }


        [HttpPost]
        public ActionResult Membership(Tbl_Members member, string url)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    // Server-side validation for ConfirmPassword
                    if (member.Password != member.ConfirmPassword)
                    {
                        ModelState.AddModelError("ConfirmPassword", "Passwords do not match");
                        return View(); // Return the view with validation errors
                    }

                    // Set IsActive to true for all records
                    member.IsActive = true;
                    member.CreateOn = DateTime.Now;

                    ctx.Tbl_Members.Add(member);
                    ctx.SaveChanges();

                    Session["MemberId"] = member.MemberId;

                    // Redirect to appropriate action based on 'url'
                    //if (url == "CheckoutDetails")
                    //{
                    //    return RedirectToAction("CheckoutDetails", new { id = member.MemberId });
                    //}
                    //else
                    if (url == "AllShippingDetails")
                    {
                        return Json(new { MemberId = member.MemberId });
                    }
                    else
                    {
                        return RedirectToAction("CheckoutDetails", new { id = member.MemberId });
                        //ViewBag.ErrorMessage = "Invalid redirection URL";
                        //return View("Error"); // Handle unknown URL
                    }
                //}
                //else
                //{
                //    //if (url == "CheckoutDetails")
                //    //{
                //    //    return View(member);
                //    //    //return RedirectToAction("CheckoutDetails", new { id = member.MemberId });
                //    //}
                //    //else 
                //    if (url == "AllShippingDetails")
                //    {
                //        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                //        return Json(new { Errors = errors });
                //        //return Json(new { MemberId = member.MemberId });
                //    }
                //    else
                //    {
                //        return View(member);
                //        //ViewBag.ErrorMessage = "Invalid redirection URL";
                //        //return View("Error"); // Handle unknown URL
                //    }
                //    // Model is invalid, return the view with validation errors
                //    //return View(member);
                //}
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exception
                ViewBag.ErrorMessage = "An error occurred while updating the entries. See the inner exception for details.";
                return View("Error");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                ViewBag.ErrorMessage = "An error occurred while adding the member.";
                return View("Error");
            }
        }

        public ActionResult CheckoutDetails()
        {
            // Retrieve the member ID from the session
            int? memberId = Session["MemberId"] as int?;

            if (memberId.HasValue)
            {
                // Retrieve the member details based on memberId
                var member = ctx.Tbl_Members.Find(memberId);

                if (member != null)
                {
                    ViewBag.MemberName = $"{member.FirstName} {member.LastName}";
                    ViewBag.MemberId = member.MemberId;
                    // Retrieve existing shipping details for the member, if any
                    var shippingDetails = ctx.Tbl_ShippingDetails.FirstOrDefault(sd => sd.MemberId == memberId);

                    // Create a model or use ViewBag to pass data to the view
                    var shippingDetailsModel = new Tbl_ShippingDetails
                    {
                        MemberId = memberId.Value,
                        Adress = shippingDetails?.Adress,
                        Country = shippingDetails?.Country,
                        State = shippingDetails?.State,
                        City = shippingDetails?.City,
                        ZipCode = shippingDetails?.ZipCode,
                        PaymentType = shippingDetails?.PaymentType,
                        // Other properties...

                        // Set AmountPaid to null to ensure it's displayed as blank in the view
                        AmountPaid = null
                    };

                    return View(shippingDetailsModel);
                }
                else
                {
                    // Handle the case when the member is not found
                    ViewBag.ErrorMessage = "Member not found.";
                    return View("Error");
                }
            }
            else
            {
                // Handle the case when the member ID is missing or invalid
                ViewBag.ErrorMessage = "Member ID is missing or invalid.";
                return View("Error");
            }
        }

        public ActionResult EditCheckoutDetails()
        {
            // Retrieve the member ID from the session
            int? memberId = Session["MemberId"] as int?;

            if (memberId.HasValue)
            {
                // Retrieve the member details based on memberId
                var member = ctx.Tbl_Members.Find(memberId);

                if (member != null)
                {
                    ViewBag.MemberName = $"{member.FirstName} {member.LastName}";
                    ViewBag.MemberId = member.MemberId;
                    // Retrieve existing shipping details for the member, if any
                    var shippingDetails = ctx.Tbl_ShippingDetails.FirstOrDefault(sd => sd.MemberId == memberId);

                    // Create a model or use ViewBag to pass data to the view
                    var shippingDetailsModel = new Tbl_ShippingDetails
                    {
                        MemberId = memberId.Value,
                        Adress = shippingDetails?.Adress,
                        Country = shippingDetails?.Country,
                        State = shippingDetails?.State,
                        City = shippingDetails?.City,
                        ZipCode = shippingDetails?.ZipCode,
                        PaymentType = shippingDetails?.PaymentType,
                        // Other properties...

                        // Set AmountPaid to null to ensure it's displayed as blank in the view
                        AmountPaid = null
                    };

                    return PartialView("_ShippingDetails",shippingDetailsModel);
                }
                else
                {
                    // Handle the case when the member is not found
                    ViewBag.ErrorMessage = "Member not found.";
                    return View("Error");
                }
            }
            else
            {
                // Handle the case when the member ID is missing or invalid
                ViewBag.ErrorMessage = "Member ID is missing or invalid.";
                return View("Error");
            }
        }

        //[HttpPost]
        //public ActionResult CheckoutDetails(Tbl_ShippingDetails shippingDetails)
        //{
        //    try
        //    {
        //        // Check if the shipping details already exist for the member
        //        var existingShippingDetails = ctx.Tbl_ShippingDetails.FirstOrDefault(sd => sd.MemberId == shippingDetails.MemberId);

        //        if (existingShippingDetails == null)
        //        {
        //            // Set the shipping date for new shipping details
        //            shippingDetails.ShippingDate = DateTime.Now;

        //            // Add new shipping details to the database
        //            ctx.Tbl_ShippingDetails.Add(shippingDetails);
        //        }
        //        else
        //        {
        //            // Update existing shipping details
        //            existingShippingDetails.Adress = shippingDetails.Adress;
        //            existingShippingDetails.Country = shippingDetails.Country;
        //            existingShippingDetails.State = shippingDetails.State;
        //            existingShippingDetails.City = shippingDetails.City;
        //            existingShippingDetails.ZipCode = shippingDetails.ZipCode;
        //            existingShippingDetails.PaymentType = shippingDetails.PaymentType;
        //            // Update other properties as needed

        //            // Set the shipping date for existing shipping details
        //            existingShippingDetails.ShippingDate = DateTime.Now;
        //        }

        //        // Save changes to the database
        //        ctx.SaveChanges();

        //        Session["MemberId"] = shippingDetails.MemberId;
        //        Session["ShippingId"] = shippingDetails.ShippingDetailId;

        //        // Prepare the data you want to return
        //        var data = new
        //        {
        //            MemberId = shippingDetails.MemberId,
        //            ShippingId = shippingDetails.ShippingDetailId
        //        };

        //        // Return a JSON result with the data
        //        return Json(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions or errors
        //        ViewBag.ErrorMessage = "An error occurred while saving shipping details.";
        //        return View("Error");
        //    }
        //}

        [HttpPost]
        public ActionResult CheckoutDetails(Tbl_ShippingDetails shippingDetails)
        {
            try
            {
                // Check if the model state is valid
                //if (ModelState.IsValid)
                //{
                    // Retrieve the MemberId from the form
                    int memberId = Convert.ToInt32(Request.Form["MemberId"]);

                    // Set the MemberId for the shippingDetails
                    shippingDetails.MemberId = memberId;
                    shippingDetails.ShippingDate = DateTime.Now;

                    // Save the shipping details to the database
                    ctx.Tbl_ShippingDetails.Add(shippingDetails);
                    ctx.SaveChanges();

                    Session["MemberId"] = shippingDetails.MemberId;
                    Session["ShippingId"] = shippingDetails.ShippingDetailId;

                    // Prepare the data you want to return
                    var data = new
                    {
                        MemberId = shippingDetails.MemberId,
                        ShippingId = shippingDetails.ShippingDetailId
                    };

                    // Return a JSON result with the data
                    return Json(data);
                //}
                //else
                //{
                //    // If model state is not valid, return validation errors
                //    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                //    return Json(new { Errors = errors });
                //}
            }
            catch (Exception ex)
            {
                // Handle exceptions or errors
                ViewBag.ErrorMessage = "An error occurred while saving shipping details.";
                return View("Error");
            }
        }

        public ActionResult ConformDetails(int? MemberId, int? ShippingId)
        {
            if (MemberId.HasValue && ShippingId.HasValue)
            {
                // Retrieve the member details based on memberId
                var member = ctx.Tbl_Members.Find(MemberId);
                var shipping = ctx.Tbl_ShippingDetails.Find(ShippingId);

                if (member != null && shipping != null)
                {
                    ViewBag.MemberName = $"{member.FirstName} {member.LastName}";
                    ViewBag.MemberId = member.MemberId;
                    ViewBag.ShippingId = shipping.ShippingDetailId;
                    ViewBag.ShippingAddress = shipping.Adress;
                    ViewBag.PaymentType = shipping.PaymentType;
                    return PartialView("_ConfirmDetails");
                }
                else
                {
                    // Handle the case when the member is not found
                    ViewBag.ErrorMessage = "Member not found.";
                    return View("Error");
                }
            }
            else
            {
                // Handle the case when the member ID is missing or invalid
                ViewBag.ErrorMessage = "Member ID is missing or invalid.";
                return View("Error");
            }
        }
        public ActionResult ShowShippingDetails(int? memberId, int? shippingId)
        {
            if (memberId == null || shippingId == null)
            {
                ViewBag.ErrorMessage = "Member ID or Shipping ID is missing.";
                return View("Error");
            }

            // Retrieve the shipping details from the database based on the member ID and shipping ID
            var shippingDetails = ctx.Tbl_ShippingDetails
                                    .Include(s => s.Tbl_Members) // Include the Member navigation property
                                    .FirstOrDefault(s => s.MemberId == memberId && s.ShippingDetailId == shippingId);

            if (shippingDetails == null)
            {
                ViewBag.ErrorMessage = "Shipping details not found.";
                return View("Error");
            }

            // Combine first name and last name into one name
            string memberName = $"{shippingDetails.Tbl_Members.FirstName} {shippingDetails.Tbl_Members.LastName}";

            // Pass the combined name along with other shipping details to the view
            ViewBag.MemberName = memberName;
            return PartialView("_ShowShippingDetails",shippingDetails);
        }


        [HttpPost]
        public ActionResult SaveCartItems(List<Tbl_Cart> cartItems)
        {
            try
            {
                // Validate if cartItems is not null and has items
                if (cartItems != null && cartItems.Count > 0)
                {
                    //var cartStatus = ctx.Tbl_CartStatus.FirstOrDefault(c => c.CartStatusId == 1);

                    //if (cartStatus == null)
                    //{
                    //    return Json(new { success = false, message = "Cart status with ID 1 not found" });
                    //}
                    // Process and save each received cart item to the database
                    foreach (var cartItem in cartItems)
                    {
                        cartItem.CartStatusId = 1;
                        cartItem.CartDate = DateTime.Now;
                        ctx.Tbl_Cart.Add(cartItem);
                        ctx.SaveChanges();

                    }

                    return Json(new { success = true, message = "Cart items saved successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "No cart items received" });
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return Json(new { success = false, message = "Error saving cart items: " + ex.Message });
            }
            
        }

        //[HttpPost]
        //public ActionResult AllShippingDetails(Tbl_ShippingDetails shippingDetails)
        //{
        //    try
        //    {
        //        // Create a new Tbl_Members instance and set its properties
        //        Tbl_Members member = new Tbl_Members
        //        {
        //            FirstName = shippingDetails.Tbl_Members.FirstName,
        //            LastName = shippingDetails.Tbl_Members.LastName,
        //            EmailId = shippingDetails.Tbl_Members.EmailId,
        //            // Set other properties as needed
        //        };

        //        // Add the new member to the Tbl_Members DbSet
        //        ctx.Tbl_Members.Add(member);

        //        // Save changes to generate MemberId
        //        ctx.SaveChanges();

        //        // Now that MemberId is generated, associate it with the Tbl_ShippingDetails
        //        shippingDetails.MemberId = member.MemberId;
        //        shippingDetails.ShippingDate = DateTime.Now;
        //        // Add Tbl_ShippingDetails to the DbSet
        //        ctx.Tbl_ShippingDetails.Add(shippingDetails);

        //        // Save changes to the database
        //        ctx.SaveChanges();

        //        return RedirectToAction("ConformDetails", "Home");
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        // Log or display the detailed information from the inner exception
        //        ViewBag.ErrorMessage = $"DbUpdateException: {ex.Message}, Inner Exception: {ex.InnerException?.Message}";
        //        return View("Error");
        //    }

        //    catch (Exception ex)
        //    {
                
        //        ViewBag.ErrorMessage = "An error occurred while adding the member.";
        //        return View("Error"); // You can create an Error view to display the error message
        //    }
        //}

    }
}