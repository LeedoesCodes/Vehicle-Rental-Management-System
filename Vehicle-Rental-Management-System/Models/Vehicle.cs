using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Rental_Management_System.Models
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public string LicensePlate { get; set; }
        public string VIN { get; set; }
        public int CategoryId { get; set; }  
        public string CategoryName { get; set; } 
        public string Transmission { get; set; } = "Automatic";
        public string FuelType { get; set; } = "Gasoline";
        public int SeatingCapacity { get; set; } = 5;
        public decimal CurrentMileage { get; set; }
        public string Status { get; set; } = "Available";
        public decimal DailyRate { get; set; }
        public decimal BaseDailyRate { get; set; } 
        public string ImagePath { get; set; }
        public string Features { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        
        public string DisplayName => $"{Year} {Make} {Model} - {LicensePlate}";
        public string CategoryDisplay => !string.IsNullOrEmpty(CategoryName) ? CategoryName : "Unknown Category";
    }

 
    public class VehicleCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal BaseDailyRate { get; set; }
        public string Description { get; set; }
    }
}