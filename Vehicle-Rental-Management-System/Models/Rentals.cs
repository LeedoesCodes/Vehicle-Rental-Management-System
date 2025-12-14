using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Rental_Management_System.Models
{
    public class Rental
    {
        // === PRIMARY KEYS & FOREIGN KEYS ===
        public int RentalId { get; set; }
        public int ReservationId { get; set; } // <--- CRITICAL ADDITION: This is the actual link in your DB

        // === DATABASE COLUMNS (Mapped via Stored Procedure) ===
        // DB Column: PickupDate -> Mapped to RentalDate
        public DateTime RentalDate { get; set; }

        // DB Column: ScheduledReturnDate -> Mapped to DueDate
        public DateTime DueDate { get; set; }

        // DB Column: ActualReturnDate
        public DateTime? ActualReturnDate { get; set; }

        // DB Column: TotalAmount
        public decimal TotalAmount { get; set; }

        // DB Column: Status ("Active", "Returned", "Overdue")
        public string Status { get; set; }

        // DB Column: InitialCondition / FinalCondition (Optional, good to have)
        public string InitialCondition { get; set; }
        public string FinalCondition { get; set; }


        // === DISPLAY FIELDS (Populated via SQL JOINS) ===
        // These do not exist in the 'Rentals' table but are fetched by sp_GetAllRentals
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }
        public string VehicleName { get; set; }
        public string CustomerName { get; set; }

        // Note: Your Vehicle table has 'Make' and 'Model', so 'VehicleName' is a composite string
        // public string LicensePlate { get; set; } // Only add this if you update the SP to SELECT v.LicensePlate


        // === HELPER PROPERTIES (Read-Only Logic) ===

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
                DateTime start = RentalDate;
                DateTime end = ActualReturnDate ?? DateTime.Now;

                // .Date ensures we compare calendar days, ignoring exact hours/minutes
                int days = (int)(end.Date - start.Date).TotalDays;

                // If returned same day, count as 1 day
                return days > 0 ? days : 1;
            }
        }
    }
}