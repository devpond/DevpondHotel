using DPHotel.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPHotel.Web.ViewModels
{
    public class CreateRoomViewModel
    {
        public Room Room { get; set; }
        public HttpPostedFileBase MainImage { get; set; }
    }
}