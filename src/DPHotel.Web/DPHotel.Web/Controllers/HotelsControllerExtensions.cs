﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DPHotel.Web.Models;
using DPHotel.Web.Business;
using System.IO;
using DPHotel.Web.Settings;

namespace DPHotel.Web.Controllers
{
    public partial class HotelsController
    {
        [HttpGet]
        public ActionResult AddImage()
        {
            ViewBag.HotelList = new SelectList(db.Hotels, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult AddImage(IEnumerable<HttpPostedFileBase> files, int? hotelId, string description)
        {
            //TODO: Check file is an image.
            var newImgs = new List<HotelImage>();
            var fileTools = new FileHelper();
            int firstId = 0;
            var fileName = "";
            var hotel = db.Hotels.Where(x => x.Id == hotelId).FirstOrDefault();
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
                            fileName = fileTools.CreateFilename(fileType, Constants.PREFIX_HOTEL_IMAGE);
                            if (!db.HotelImages.Any(x => x.FileName == fileName))
                            {
                                isUnique = true;
                            }
                        }
                        var path = Path.Combine(Server.MapPath(Constants.FOLDER_IMAGES), fileName);
                        file.SaveAs(path);
                        img = System.Drawing.Image.FromFile(path); //This could be an issue if the file does not save.
                    }
                    // set attributes for image record
                    newImg.FileName = fileName;
                    newImg.Hotel = hotel;
                    if (hotel.MainImage == null)
                    {
                        // Add a default image. Is this the best place for it?
                        hotel.MainImage = newImg;
                        db.Entry(hotel).State = EntityState.Modified;
                    }

                    // remove this extra step once proper login retrieval is fixed.
                    //if (User.Identity.GetUserId() != null)
                    //{
                    //    newImg.UploadedBy = User.Identity.GetUserId();
                    //}

                    newImg.DateCreated = DateTime.UtcNow;
                    newImg.LastUpdated = DateTime.UtcNow;
                    //newImg.IsHidden = false;
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