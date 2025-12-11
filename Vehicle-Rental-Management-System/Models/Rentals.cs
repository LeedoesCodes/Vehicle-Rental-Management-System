using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Vehicle_Rental_Management_System.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }

        // These fields come from the SQL JOINs (for display purposes)
        public string VehicleName { get; set; }
        public string CustomerName { get; set; }
        public string LicensePlate { get; set; }

        public DateTime RentalDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ActualReturnDate { get; set; } // Nullable (null = not returned yet)

        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // "Active", "Returned", "Overdue"

        // === USEFUL HELPER PROPERTIES ===

        // Automatically checks if the rental is late
        public bool IsOverdue
        {
            get
            {
                // If not returned yet AND today is past the due date
                if (ActualReturnDate == null && DateTime.Now > DueDate)
                    return true;
                return false;
            }
        }

        // Calculates how many days the car has been out
        public int DaysRented
        {
            get
            {
                DateTime endDate = ActualReturnDate ?? DateTime.Now;
                int days = (int)(endDate - RentalDate).TotalDays;
                return days > 0 ? days : 1; // Minimum 1 day
            }
        }
    }
}