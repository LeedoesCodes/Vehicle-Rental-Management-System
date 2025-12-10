using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Rental_Management_System.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled, Completed
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int CreatedBy { get; set; } // UserId who created

        // Navigation properties (for display)
        public string CustomerName { get; set; }
        public string VehicleInfo { get; set; }
    }
}