using PlantNest_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PlantNest_Project.Controllers
{
    public class AdminPanelController : Controller
    {
        private PlantNestEntities db = new PlantNestEntities();
        // GET: AdminPanel
        public ActionResult Index()
        {
            ViewBag.TotalUsers = db.Users.Count();
            ViewBag.TotalPlants = db.Plants.Count();
            ViewBag.TotalAccessories = db.Accessories.Count();
            return View();
        }

        // GET: Admin/Login
        public ActionResult Login()
        {
            if (Session["u"] != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // POST: Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminLogin model)
        {
            if (ModelState.IsValid)
            {
                // Validate admin credentials
                var admin = db.AdminLogins.SingleOrDefault(a => a.AdName == model.AdName && a.Password == model.Password);
                if (admin != null)
                {
                    // Set admin session and redirect to dashboard
                    Session["u"] = admin.AdName;
                    Session["Role"] = "User";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid admin credentials.");
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session["u"] = null;
            Session["Role"] = null;
            return RedirectToAction("Login");
        }

        // Create Plant
        public ActionResult CreatePlant()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePlant(Plant plant, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    // Save the file to the server
                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    file.SaveAs(path);

                    // Set the ImageURL property of the plant model
                    plant.ImageURL = "/Uploads/" + fileName;
                }

                db.Plants.Add(plant);
                db.SaveChanges();
                return RedirectToAction("ShowPlant");
            }
            return View(plant);
        }


        // Manage Plants
        public ActionResult ShowPlant()
        {
            if (Session["u"] != null)
            {
                var plants = db.Plants.ToList();
            return View(plants);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        // Edit Plant
        public ActionResult EditPlant(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Plant plant = db.Plants.Find(id);

            if (plant == null)
            {
                return HttpNotFound();
            }

            return View(plant);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPlant(Plant plant, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                // Check if a new file is uploaded
                if (file != null && file.ContentLength > 0)
                {
                    // Save the new file to the server
                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    file.SaveAs(path);

                    // Update the ImageURL property with the new file path
                    plant.ImageURL = "/Uploads/" + fileName;
                }
                else
                {
                    // If no new file is uploaded, retain the old image
                    db.Entry(plant).Property(p => p.ImageURL).IsModified = false;
                }

                // Update the plant details in the database
                db.Entry(plant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShowPlant");
            }

            return View(plant);
        }


        // GET: Admin/DeletePlant/5
        public ActionResult DeletePlant(int id)
        {
            var plant = db.Plants.Find(id);
            if (plant == null)
            {
                return HttpNotFound();
            }
            return View(plant);
        }

        [HttpPost, ActionName("DeletePlant")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var plant = db.Plants.Find(id);
            if (plant != null)
            {
                db.Plants.Remove(plant);
                db.SaveChanges();
            }
            return RedirectToAction("ShowPlant");
        }

        // GET: Admin/ManageAccessories
        public ActionResult ManageAccessories()
        {
            var accessories = db.Accessories.ToList();
            return View(accessories);
        }

        // GET: Admin/CreateAccessory
        public ActionResult CreateAccessory()
        {
            return View();
        }

        // POST: Admin/CreateAccessory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAccessory(Accessory accessory)
        {
            if (ModelState.IsValid)
            {
                db.Accessories.Add(accessory);
                db.SaveChanges();
                return RedirectToAction("ManageAccessories");
            }
            return View(accessory);
        }

        // GET: Admin/EditAccessory/5
        public ActionResult EditAccessory(int id)
        {
            var accessory = db.Accessories.Find(id);
            if (accessory == null)
            {
                return HttpNotFound();
            }
            return View(accessory);
        }

        // POST: Admin/EditAccessory/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAccessory(Accessory accessory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accessory).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManageAccessories");
            }
            return View(accessory);
        }

        // GET: Admin/DeleteAccessory/5
        public ActionResult DeleteAccessory(int id)
        {
            var accessory = db.Accessories.Find(id);
            if (accessory == null)
            {
                return HttpNotFound();
            }
            return View(accessory);
        }

        // POST: Admin/DeleteAccessory/5
        [HttpPost, ActionName("DeleteAccessory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedAcc(int id)
        {
            var accessory = db.Accessories.Find(id);
            if (accessory != null)
            {
                db.Accessories.Remove(accessory);
                db.SaveChanges();
            }
            return RedirectToAction("ManageAccessories");
        }

    }
}