using PlantNest_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlantNest_Project.Controllers
{
    public class HomeController : Controller
    {
        private PlantNestEntities db = new PlantNestEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // Method to display flowering plants
        public ActionResult Flowering()
        {
            var floweringPlants = db.Plants
                .Where(p => p.Category == "Flowering")
                .ToList();
            return View(floweringPlants);
        }

        // Method to display non-flowering plants
        public ActionResult NonFlowering()
        {
            var nonFloweringPlants = db.Plants
                .Where(p => p.Category == "Non-Flowering")
                .ToList();
            return View(nonFloweringPlants);
        }
        public ActionResult Indoor()
        {
            var floweringPlants = db.Plants
                 .Where(p => p.Category == "Indoor")
                 .ToList();
            return View(floweringPlants);
        }
        public ActionResult Outdoor()
        {
            var floweringPlants = db.Plants
                .Where(p => p.Category == "Outdoor")
                .ToList();
            return View(floweringPlants);
        }
        public ActionResult Succulents()
        {
            var floweringPlants = db.Plants
                .Where(p => p.Category == "Succulents")
                .ToList();
            return View(floweringPlants);
        }
       
        public ActionResult Medicinal()
        {
            var floweringPlants = db.Plants
                .Where(p => p.Category == "Medicinal")
                .ToList();
            return View(floweringPlants);
        }

        // GET: UserRegistration
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingUser = db.Users.SingleOrDefault(u => u.Username == model.Username);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Username", "Username already exists.");
                        return View(model);
                    }

                    db.Users.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }

            return View(model);
        }


        public ActionResult Login()
        {
            ViewBag.ActivePage = "Login";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => (u.Username == model.Username || u.Email == model.Username) && u.PasswordHash == model.PasswordHash);

                if (user != null)
                {
                    Session["UserID"] = user.UserID;
                    Session["u"] = user.Username;
                    Session["Role"] = "User";

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }

            return View(model);
        }

        public ActionResult DeleteAccount()
        {
            var userId = Session["UserID"];
            if (userId != null)
            {
                var user = db.Users.Find((int)userId);
                if (user != null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();

                    Session.Clear();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Login");
        }


        // GET: User/ManageProfile
        public ActionResult ManageProfile()
        {
            var userId = Session["UserID"];
            if (userId != null)
            {
                var user = db.Users.Find((int)userId);
                if (user != null)
                {
                    var userProfile = new User
                    {
                        UserID = user.UserID,
                        Name = user.Name,
                        Email = user.Email,
                        ContactNumber = user.ContactNumber,
                        Username = user.Username,
                        PasswordHash = user.PasswordHash,
                    };
                    return View(userProfile);
                }
            }
            return RedirectToAction("Login", "Home");
        }



        // POST: User/ManageProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProfile(User model, string newPassword, string confirmPassword)
        {
            if (ModelState.IsValid)
            {
                var userId = Session["UserID"] as int?;
                if (userId.HasValue)
                {
                    var user = db.Users.Find(userId.Value);
                    if (user != null)
                    {
                        user.Name = model.Name;
                        user.Email = model.Email;
                        user.ContactNumber = model.ContactNumber;
                        user.Username = model.Username;
                        user.PasswordHash = model.PasswordHash;
                       
                        if (!string.IsNullOrWhiteSpace(newPassword) && newPassword == confirmPassword)
                        {
                            user.PasswordHash = newPassword;
                        }
                        else if (!string.IsNullOrWhiteSpace(newPassword) || !string.IsNullOrWhiteSpace(confirmPassword))
                        {
                            ModelState.AddModelError("", "Passwords do not match.");
                            return View(model);
                        }

                        db.SaveChanges();
                        return RedirectToAction("ManageProfile");
                    }
                }
            }
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var plant = db.Plants.Find(id);
            if (plant == null)
            {
                return HttpNotFound();
            }
            return View(plant);
        }

        [HttpPost]
        public ActionResult AddToWishlist(int id, int quantity)
        {
            var userId = Session["UserID"] as int?;
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var plant = db.Plants.Find(id);
            if (plant == null)
            {
                return HttpNotFound();
            }

            var cartItem = db.CartItems.SingleOrDefault(c => c.PlantID == id && c.UserId == userId.ToString());
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    PlantID = id,
                    Quantity = quantity,
                    UserId = userId.ToString()
                };
                db.CartItems.Add(cartItem);
            }

            db.SaveChanges();

            return RedirectToAction("Details", new { id });
        }


        public ActionResult Cart()
        {
            var userId = Session["UserID"] as int?;
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var cartItems = db.CartItems
                .Where(c => c.UserId == userId.ToString())
                .ToList();

            ViewBag.CartItemCount = cartItems.Sum(c => c.Quantity);

            return View(cartItems);
        }

        [HttpPost]
        public ActionResult UpdateQuantity(int cartItemId, int quantity)
        {
            var cartItem = db.CartItems.Find(cartItemId);
            if (cartItem == null)
            {
                return HttpNotFound();
            }

            cartItem.Quantity = quantity;
            db.SaveChanges();

            return RedirectToAction("Cart");
        }



        [HttpPost]
        public ActionResult RemoveFromCart(int cartItemId)
        {
            var cartItem = db.CartItems.Find(cartItemId);
            if (cartItem == null)
            {
                return HttpNotFound();
            }

            db.CartItems.Remove(cartItem);
            db.SaveChanges();

            return RedirectToAction("Cart");
        }


        public ActionResult Checkout()
        {
            var userId = Session["UserID"] as int?;
            if (userId == null)
            {
                return RedirectToAction("Login"); 
            }

            var cartItems = db.CartItems
                .Where(c => c.UserId == userId.ToString())
                .ToList();

            foreach (var item in cartItems)
            {
                var plant = db.Plants.Find(item.PlantID); 
                if (plant != null && item.Quantity > plant.StockQuantity)
                {
                    
                    item.Quantity = plant.StockQuantity;
                    
                }
            }

            db.SaveChanges();
            return View(cartItems);
        }


        [HttpPost]
        public ActionResult Checkout(Plant model)
        {
            var userId = Session["UserID"] as int?;
            if (userId == null)
            {
                return RedirectToAction("Login"); 
            }

            // Get the cart items for the user
            var cartItems = db.CartItems
                .Where(c => c.UserId == userId.ToString())
                .ToList();

            foreach (var item in cartItems)
            {
                var plant = db.Plants.Find(item.PlantID); 
                if (plant != null)
                {
                    if (item.Quantity > plant.StockQuantity)
                    {
                        
                        item.Quantity = plant.StockQuantity;
                    }

                    
                    plant.StockQuantity -= item.Quantity;
                }
            }

            // Save changes
            db.SaveChanges();

            return RedirectToAction("OrderConfirmation");
        }


        public ActionResult ConfirmOrder()
        {
            var userId = Session["UserID"].ToString();
            var cartItems = db.CartItems
                .Include("Plant")
                .Where(ci => ci.UserId == userId)
                .ToList();

            if (cartItems.Any())
            {
                // Create a new order
                var order = new Order
                {
                    UserID = int.Parse(userId), 
                    OrderDate = DateTime.Now,
                    TotalAmount = cartItems.Sum(ci => ci.Quantity * ci.Plant.Price),
                    Status = "active" 
                };

                
                db.Orders.Add(order);
                db.SaveChanges();

               
                var orderItems = cartItems.Select(ci => new CartItem
                {
                    
                    PlantID = ci.PlantID,
                    Quantity = ci.Quantity,
                }).ToList();

                db.CartItems.AddRange(orderItems);
                db.SaveChanges();

               
                db.CartItems.RemoveRange(cartItems);
                db.SaveChanges();
            }

            return View();
        }

        // GET: Reviews/Create
        public ActionResult CreateReview(int plantId)
        {
            ViewBag.PlantID = plantId;
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReview(Review model)
        {
            if (ModelState.IsValid)
            {
                var userId = Session["UserID"] as int?;
                if (userId.HasValue)
                {
                    model.UserID = userId.Value;
                    model.ReviewDate = DateTime.Now;

                    db.Reviews.Add(model);
                    db.SaveChanges();

                    return RedirectToAction("Details", new { id = model.PlantID });
                }
                else
                {
                    ModelState.AddModelError("", "User not logged in.");
                }
            }

            return View(model);
        }


        // GET: Feedback
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Feedback model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the current user ID from session
                var userId = Session["UserID"] as int?;

                if (userId.HasValue)
                {
                    model.UserID = userId.Value;
                    model.FeedbackDate = DateTime.Now;

                    db.Feedbacks.Add(model);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home"); // Redirect to a confirmation page or home
                }
                else
                {
                    ModelState.AddModelError("", "You must be logged in to submit feedback.");
                }
            }

            return View(model);
        }

        // GET: ContactUs
        public ActionResult ContactUs()
        {
            return View();
        }

        // POST: ContactUs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(Contact msg)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(msg);
                db.SaveChanges();
                return RedirectToAction("ContactUs");
            }

            // If the model state is invalid, re-display the form with validation errors
            return View("ContactUs", msg);
        }

        public ActionResult Search(string search, string category)
        {
            var plants = db.Plants.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                plants = plants.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }

            if (!string.IsNullOrEmpty(category))
            {
                plants = plants.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            var result = plants.ToList();
            return View(result); 
        }

        [HttpGet]
        public ActionResult Buy(int id)
        {
            // Fetch the plant details from the database based on the PlantID
            var plant = db.Plants.Find(id);

            if (plant != null)
            {
                ViewBag.Quantity = 1; // Default quantity, you can adjust this as needed
                return View(plant);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult Buy(int id, int quantity)
        {
            // Logic to handle the purchase process
            var plant = db.Plants.Find(id); // Retrieve the plant details from the database

            if (plant != null)
            {
                // Process the purchase, such as adding to cart or initiating payment
                // You can pass the plant and quantity to the view
                ViewBag.Quantity = quantity;
                return View(plant);
            }
            else
            {
                return HttpNotFound();
            }
        }




    }


}