using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DPHotel.Web.Models;

namespace DPHotel.Web.Controllers
{
    public class HotelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Hotels
        public ActionResult Index()
        {
            return View(db.Hotels.ToList());
        }

        // GET: Hotels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // GET: Hotels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hotels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DateCreated,LastUpdated,Name,City")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Hotels.Add(hotel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hotel);
        }

        // GET: Hotels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateCreated,LastUpdated,Name,City")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hotel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hotel hotel = db.Hotels.Find(id);
            db.Hotels.Remove(hotel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult AddImage(IEnumerable<HttpPostedFileBase> files, int? GalleryId, string description)
        {
            //TODO: Check file is an image.
            var newImgs = new List<HotelImage>();
            var fileTools = new FileHelper();
            //subject editedSubject = null;
            int firstId = 0;
            var fileName = "";
            if (files != null)
            {
                foreach (var file in files)
                {
                    var newImg = new HotelImage();
                    var fileType = fileTools.GetExtension(file.ContentType);
                    System.Drawing.Image img = null;

                    if (file.ContentLength > 0)
                    {
                        var isUnique = false;
                        while (!isUnique)
                        {
                            fileName = fileTools.CreateFilename(fileType);
                            if (!db.HotelImages.Any(x => x.FileName == fileName))
                            {
                                isUnique = true;
                            }
                        }
                        var path = Path.Combine(Server.MapPath("~/Assets/Images"), fileName);
                        file.SaveAs(path);
                        img = System.Drawing.Image.FromFile(path); //This could be an issue if the file does not save.
                    }
                    // set attributes for image record
                    newImg.FileName = fileName;

                    // remove this extra step once proper login retrieval is fixed.
                    //if (User.Identity.GetUserId() != null)
                    //{
                    //    newImg.UploadedBy = User.Identity.GetUserId();
                    //}

                    newImg.UploadDate = DateTime.Now;
                    newImg.IsHidden = false;
                    newImg.Description = description;
                    if (img != null)
                    {
                        newImg.Width = img.Width;
                        newImg.Height = img.Height;
                        newImg.Orientation = img.Width > img.Height ? "Landscape" : "Portrait";
                    }

                    //ViewBag.LocationID = new SelectList(db.site_locations, "LocationID", "Location");
                    //ViewBag.AddedBy = new SelectList(db.AspNetUsers, "Id", "UserName");
                    //ViewBag.PermissionLevel = new SelectList(db.AspNetRoles, "Id", "Name");
                    newImgs.Add(newImg);
                    //if (editedSubject != null)
                    //{
                    //    newImg.subjects1.Add(editedSubject);
                    //}
                }

                foreach (var img in newImgs)
                {
                    db.HotelImages.Add(img);
                }
                if (ModelState.IsValid)
                {
                    var id = db.SaveChanges();
                    //var newImgId = (from a in db.images
                    //    where a.Filename == fileName
                    //    select a.ImageID).FirstOrDefault();
                    //var selImage = db.images.SingleOrDefault(x => x.ImageID == newImgId);
                    if (newImgs.Count == 1)
                    {
                        HotelImage i = db.HotelImages.FirstOrDefault(x => x.FileName == fileName);
                        if (i != null)
                        {
                            firstId = i.Id;
                        }
                        return RedirectToAction("ViewImage", new { Id = firstId });
                    }
                    return RedirectToAction("ViewImage");
                }

                return View("Error");
            }
            return View("Error");
        }
    }
}
