using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DPHotel.Web.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        public ICollection<Room> Rooms { get; set; }
        public ICollection<HotelImage> Images { get; set; }
    }

    public class HotelImage
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Name { get; set; }
        public int Description { get; set; }
        public Hotel Hotel { get; set; }
    }
    public class Room
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        public RoomType RoomType { get; set; }
        public int Number { get; set; }
        public Hotel Hotel { get; set; }

        public ICollection<RoomImage> Images { get; set; }

    }
    public class RoomImage
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Name { get; set; }
        public int Description { get; set; }
        public Room Room { get; set; }
    }
    public class RoomType
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }

    public class Guest
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsPrimaryGuest { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }

    public class Booking
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public Room Room { get; set; }

        public ICollection<Payment> Payments { get; set; }
        public ICollection<Guest> Guests { get; set; }
    }

    public class Payment
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        public Booking Booking { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal Amount { get; set; }
        public DateTime DatePaid { get; set; }
    }
    public class PaymentType
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}