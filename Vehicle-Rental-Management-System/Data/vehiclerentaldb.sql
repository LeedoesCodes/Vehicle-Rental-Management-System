-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               8.4.3 - MySQL Community Server - GPL
-- Server OS:                    Win64
-- HeidiSQL Version:             12.8.0.6908
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dumping database structure for vehiclerentaldb
DROP DATABASE IF EXISTS `vehiclerentaldb`;
CREATE DATABASE IF NOT EXISTS `vehiclerentaldb` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `vehiclerentaldb`;

-- Dumping structure for table vehiclerentaldb.customers
DROP TABLE IF EXISTS `customers`;
CREATE TABLE IF NOT EXISTS `customers` (
  `CustomerId` int NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(50) NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Phone` varchar(20) NOT NULL,
  `Address` text,
  `DateOfBirth` date NOT NULL,
  `LicenseNumber` varchar(50) NOT NULL,
  `LicenseExpiry` date NOT NULL,
  `DOB` datetime DEFAULT NULL,
  `LicenseState` varchar(50) DEFAULT NULL,
  `IsBlacklisted` tinyint(1) DEFAULT '0',
  `CreatedDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `CustomerType` varchar(20) DEFAULT 'Individual',
  `EmergencyContactName` varchar(100) DEFAULT NULL,
  `EmergencyContactPhone` varchar(20) DEFAULT NULL,
  `PhotoPath` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`CustomerId`),
  UNIQUE KEY `Email` (`Email`),
  KEY `idx_email` (`Email`),
  KEY `idx_phone` (`Phone`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table vehiclerentaldb.customers: ~3 rows (approximately)
INSERT INTO `customers` (`CustomerId`, `FirstName`, `LastName`, `Email`, `Phone`, `Address`, `DateOfBirth`, `LicenseNumber`, `LicenseExpiry`, `DOB`, `LicenseState`, `IsBlacklisted`, `CreatedDate`, `CustomerType`, `EmergencyContactName`, `EmergencyContactPhone`, `PhotoPath`) VALUES
	(1, 'Lee', 'Singson', 'leesingeon@mail.com', '09053414599', 'San Miguel, Tagum', '2005-11-04', '16459861985', '2028-12-11', NULL, 'Philippines', 0, '2025-12-11 14:44:13', 'Individual', 'Ranel Singson', '09368656885', 'Cust_Lee_Singson_639032038903908295.jpg'),
	(2, 'Dustin', 'Angway', 'duskun@gmail.com', '09273472872', 'Briz, Tagum', '2005-05-07', '2342356576', '2028-12-11', NULL, 'Philippines', 0, '2025-12-11 14:52:56', 'Individual', '', '', ''),
	(3, 'Chael Gabriel', 'Lusaya', 'Chalenluz@gmail.com', '09128673869', 'Visayan Village, Tagum', '2004-08-16', '23364363', '2028-12-11', NULL, 'Philippines', 0, '2025-12-11 14:54:10', 'Individual', '', '', '');

-- Dumping structure for table vehiclerentaldb.damages
DROP TABLE IF EXISTS `damages`;
CREATE TABLE IF NOT EXISTS `damages` (
  `DamageId` int NOT NULL AUTO_INCREMENT,
  `RentalId` int NOT NULL,
  `Description` text NOT NULL,
  `RepairCost` decimal(10,2) DEFAULT '0.00',
  `ReportedDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `IsPaid` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`DamageId`),
  KEY `RentalId` (`RentalId`),
  CONSTRAINT `damages_ibfk_1` FOREIGN KEY (`RentalId`) REFERENCES `rentals` (`RentalId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table vehiclerentaldb.damages: ~0 rows (approximately)

-- Dumping structure for table vehiclerentaldb.maintenancerecords
DROP TABLE IF EXISTS `maintenancerecords`;
CREATE TABLE IF NOT EXISTS `maintenancerecords` (
  `MaintenanceId` int NOT NULL AUTO_INCREMENT,
  `VehicleId` int NOT NULL,
  `MaintenanceDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `Description` text NOT NULL,
  `Cost` decimal(10,2) DEFAULT NULL,
  `OdometerReading` decimal(10,2) DEFAULT NULL,
  `NextDueDate` date DEFAULT NULL,
  `Status` enum('Scheduled','InProgress','Completed') DEFAULT 'Scheduled',
  PRIMARY KEY (`MaintenanceId`),
  KEY `VehicleId` (`VehicleId`),
  CONSTRAINT `maintenancerecords_ibfk_1` FOREIGN KEY (`VehicleId`) REFERENCES `vehicles` (`VehicleId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table vehiclerentaldb.maintenancerecords: ~0 rows (approximately)

-- Dumping structure for table vehiclerentaldb.payments
DROP TABLE IF EXISTS `payments`;
CREATE TABLE IF NOT EXISTS `payments` (
  `PaymentId` int NOT NULL AUTO_INCREMENT,
  `RentalId` int NOT NULL,
  `Amount` decimal(10,2) NOT NULL,
  `PaymentDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `PaymentMethod` enum('Cash','CreditCard','DebitCard','BankTransfer') NOT NULL,
  `PaymentType` enum('Deposit','Rental','Damage','LateFee','Fuel','Other') NOT NULL,
  `Status` enum('Pending','Completed','Refunded') DEFAULT 'Completed',
  `TransactionId` varchar(100) DEFAULT NULL,
  `Notes` text,
  PRIMARY KEY (`PaymentId`),
  KEY `RentalId` (`RentalId`),
  CONSTRAINT `payments_ibfk_1` FOREIGN KEY (`RentalId`) REFERENCES `rentals` (`RentalId`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table vehiclerentaldb.payments: ~2 rows (approximately)
INSERT INTO `payments` (`PaymentId`, `RentalId`, `Amount`, `PaymentDate`, `PaymentMethod`, `PaymentType`, `Status`, `TransactionId`, `Notes`) VALUES
	(1, 5, 8500.00, '2026-01-05 11:32:39', 'Cash', 'Deposit', 'Completed', NULL, 'Return processed via App'),
	(2, 6, 17900.00, '2026-01-05 15:13:15', 'Cash', 'Deposit', 'Completed', NULL, 'Return processed via App');

-- Dumping structure for table vehiclerentaldb.rentals
DROP TABLE IF EXISTS `rentals`;
CREATE TABLE IF NOT EXISTS `rentals` (
  `RentalId` int NOT NULL AUTO_INCREMENT,
  `VehicleId` int DEFAULT NULL,
  `CustomerId` int DEFAULT NULL,
  `ReservationId` int DEFAULT NULL,
  `PickupDate` datetime NOT NULL,
  `ScheduledReturnDate` datetime NOT NULL,
  `ActualReturnDate` datetime DEFAULT NULL,
  `OdometerStart` decimal(10,2) NOT NULL,
  `OdometerEnd` decimal(10,2) DEFAULT NULL,
  `FuelLevelStart` varchar(50) DEFAULT NULL,
  `FuelLevelEnd` varchar(50) DEFAULT NULL,
  `InitialCondition` text,
  `FinalCondition` text,
  `Status` varchar(50) DEFAULT NULL,
  `ProcessedBy` int DEFAULT NULL,
  `TotalAmount` decimal(10,2) DEFAULT '0.00',
  PRIMARY KEY (`RentalId`),
  KEY `ReservationId` (`ReservationId`),
  KEY `ProcessedBy` (`ProcessedBy`),
  KEY `idx_status` (`Status`),
  CONSTRAINT `rentals_ibfk_1` FOREIGN KEY (`ReservationId`) REFERENCES `reservations` (`ReservationId`),
  CONSTRAINT `rentals_ibfk_2` FOREIGN KEY (`ProcessedBy`) REFERENCES `users` (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table vehiclerentaldb.rentals: ~8 rows (approximately)
INSERT INTO `rentals` (`RentalId`, `VehicleId`, `CustomerId`, `ReservationId`, `PickupDate`, `ScheduledReturnDate`, `ActualReturnDate`, `OdometerStart`, `OdometerEnd`, `FuelLevelStart`, `FuelLevelEnd`, `InitialCondition`, `FinalCondition`, `Status`, `ProcessedBy`, `TotalAmount`) VALUES
	(1, 8, 3, 2, '2025-12-11 14:45:51', '2025-12-12 14:45:51', '2025-12-14 21:41:05', 1500.00, 456678.00, 'Empty', 'Full', NULL, ' (Damage Fee Detected)', 'Completed', 2, 13500.00),
	(2, NULL, NULL, 3, '2025-12-11 14:54:33', '2025-12-19 14:54:32', '2026-01-02 14:48:19', 28000.00, 28341.00, 'Full', 'Full', NULL, '', 'Completed', 2, NULL),
	(3, NULL, NULL, 4, '2025-12-30 01:12:08', '2026-01-03 01:12:08', NULL, 245.00, NULL, 'Full', NULL, NULL, NULL, 'Ongoing', 2, NULL),
	(4, NULL, NULL, 5, '2026-01-02 14:34:35', '2026-01-04 14:34:35', NULL, 12000.00, NULL, 'Full', NULL, NULL, NULL, 'Ongoing', 1, NULL),
	(5, 4, 2, 1, '2026-01-02 15:37:13', '2026-01-03 15:37:13', '2026-01-05 11:32:16', 15000.00, 15000.00, 'Full', '1/4', NULL, ' (Damage Fee Detected)', 'Completed', 1, 8500.00),
	(6, 9, 1, NULL, '2026-01-02 16:06:13', '2026-01-03 16:06:13', '2026-01-05 15:11:28', 28341.00, 28501.00, 'Full', '1/4', '', ' (Damage Fee Detected)', 'Completed', 1, 17900.00),
	(7, 8, 3, NULL, '2025-12-31 14:56:15', '2026-01-01 14:56:16', NULL, 35000.00, NULL, 'Full', NULL, '', NULL, 'Ongoing', 1, 4500.00),
	(8, 5, 1, NULL, '2026-01-05 10:44:48', '2026-01-06 10:44:48', NULL, 22000.00, NULL, 'Full', NULL, '', NULL, 'Ongoing', 1, 1500.00);

-- Dumping structure for table vehiclerentaldb.reservations
DROP TABLE IF EXISTS `reservations`;
CREATE TABLE IF NOT EXISTS `reservations` (
  `ReservationId` int NOT NULL AUTO_INCREMENT,
  `CustomerId` int NOT NULL,
  `VehicleId` int NOT NULL,
  `StartDate` datetime NOT NULL,
  `EndDate` datetime NOT NULL,
  `Status` varchar(50) DEFAULT NULL,
  `TotalAmount` decimal(10,2) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `CreatedBy` int DEFAULT NULL,
  `Notes` text,
  PRIMARY KEY (`ReservationId`),
  KEY `CustomerId` (`CustomerId`),
  KEY `VehicleId` (`VehicleId`),
  KEY `CreatedBy` (`CreatedBy`),
  KEY `idx_dates` (`StartDate`,`EndDate`),
  KEY `idx_status` (`Status`),
  CONSTRAINT `reservations_ibfk_1` FOREIGN KEY (`CustomerId`) REFERENCES `customers` (`CustomerId`),
  CONSTRAINT `reservations_ibfk_2` FOREIGN KEY (`VehicleId`) REFERENCES `vehicles` (`VehicleId`),
  CONSTRAINT `reservations_ibfk_3` FOREIGN KEY (`CreatedBy`) REFERENCES `users` (`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table vehiclerentaldb.reservations: ~3 rows (approximately)
INSERT INTO `reservations` (`ReservationId`, `CustomerId`, `VehicleId`, `StartDate`, `EndDate`, `Status`, `TotalAmount`, `CreatedDate`, `CreatedBy`, `Notes`) VALUES
	(1, 2, 4, '2026-01-02 15:37:13', '2026-01-03 15:37:13', 'Confirmed', 2000.00, '2026-01-02 15:37:19', 1, ''),
	(2, 3, 8, '2026-01-02 15:37:51', '2026-01-03 15:37:51', 'Confirmed', 4500.00, '2026-01-02 15:37:59', 1, ''),
	(3, 3, 5, '2026-01-02 16:06:27', '2026-01-03 16:06:27', 'Confirmed', 1500.00, '2026-01-02 16:06:36', 1, '');

-- Dumping structure for procedure vehiclerentaldb.sp_AddCustomer
DROP PROCEDURE IF EXISTS `sp_AddCustomer`;
DELIMITER //
CREATE PROCEDURE `sp_AddCustomer`(
    IN p_FirstName VARCHAR(100),
    IN p_LastName VARCHAR(100),
    IN p_Email VARCHAR(100),
    IN p_Phone VARCHAR(20),
    IN p_Address TEXT,
    IN p_LicenseNumber VARCHAR(50),
    IN p_LicenseExpiry DATETIME,
    IN p_LicenseState VARCHAR(50), -- New Column
    IN p_DOB DATETIME,             -- Fixes the 'Default Value' error
    IN p_CustomerType VARCHAR(50),
    IN p_EmergencyName VARCHAR(100),
    IN p_EmergencyPhone VARCHAR(20),
    IN p_PhotoPath VARCHAR(255)
)
BEGIN
    INSERT INTO Customers (
        FirstName, 
        LastName, 
        Email, 
        Phone, 
        Address, 
        LicenseNumber, 
        LicenseExpiry, 
        LicenseState,      -- Inserting into the new column
        DateOfBirth,       -- Inserting the DOB here
        CustomerType, 
        EmergencyContactName, 
        EmergencyContactPhone, 
        PhotoPath,
        CreatedDate,
        IsBlacklisted
    )
    VALUES (
        p_FirstName, 
        p_LastName, 
        p_Email, 
        p_Phone, 
        p_Address, 
        p_LicenseNumber, 
        p_LicenseExpiry, 
        p_LicenseState,    -- Value from C#
        p_DOB,             -- Value from C#
        p_CustomerType, 
        p_EmergencyName, 
        p_EmergencyPhone, 
        p_PhotoPath,
        NOW(),             -- Auto-set CreatedDate
        0                  -- Default Not Blacklisted
    );
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_AddReservation
DROP PROCEDURE IF EXISTS `sp_AddReservation`;
DELIMITER //
CREATE PROCEDURE `sp_AddReservation`(
    IN p_CustomerId INT,
    IN p_VehicleId INT,
    IN p_StartDate DATETIME,
    IN p_EndDate DATETIME,
    IN p_TotalAmount DECIMAL(10,2),
    IN p_Notes TEXT
)
BEGIN
    -- Insert into Reservations table
    INSERT INTO Reservations (
        CustomerId, VehicleId, StartDate, EndDate, 
        TotalAmount, Status, Notes, CreatedBy
    ) VALUES (
        p_CustomerId, p_VehicleId, p_StartDate, p_EndDate, 
        p_TotalAmount, 'Confirmed', p_Notes, 1 -- Default Admin ID
    );
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_AddVehicle
DROP PROCEDURE IF EXISTS `sp_AddVehicle`;
DELIMITER //
CREATE PROCEDURE `sp_AddVehicle`(
    IN p_Make VARCHAR(50), IN p_Model VARCHAR(50), IN p_Year INT, 
    IN p_Color VARCHAR(30), IN p_LicensePlate VARCHAR(20), IN p_VIN VARCHAR(50), 
    IN p_CategoryId INT, IN p_Transmission VARCHAR(20), IN p_FuelType VARCHAR(20), 
    IN p_SeatingCapacity INT, IN p_CurrentMileage DECIMAL(10,2), 
    IN p_DailyRate DECIMAL(10,2), IN p_Status VARCHAR(20)
)
BEGIN
    IF EXISTS (SELECT 1 FROM Vehicles WHERE LicensePlate = p_LicensePlate) THEN
         SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'License Plate already exists.';
    ELSE
        INSERT INTO Vehicles (
            Make, Model, Year, Color, LicensePlate, VIN, CategoryId, 
            Transmission, FuelType, SeatingCapacity, CurrentMileage, 
            DailyRate, Status
        ) VALUES (
            p_Make, p_Model, p_Year, p_Color, p_LicensePlate, p_VIN, p_CategoryId, 
            p_Transmission, p_FuelType, p_SeatingCapacity, p_CurrentMileage, 
            p_DailyRate, p_Status
        );
        
        -- CRITICAL FIX: Return the ID so C# can save the image!
        SELECT LAST_INSERT_ID(); 
    END IF;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_CancelReservation
DROP PROCEDURE IF EXISTS `sp_CancelReservation`;
DELIMITER //
CREATE PROCEDURE `sp_CancelReservation`(IN p_ReservationId INT)
BEGIN
    UPDATE Reservations 
    SET Status = 'Cancelled' 
    WHERE ReservationId = p_ReservationId;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_CreateRental
DROP PROCEDURE IF EXISTS `sp_CreateRental`;
DELIMITER //
CREATE PROCEDURE `sp_CreateRental`(
    IN p_VehicleId INT,
    IN p_CustomerId INT,
    IN p_PickupDate DATETIME,
    IN p_ReturnDate DATETIME,
    IN p_TotalAmount DECIMAL(10,2),
    IN p_OdometerStart DECIMAL(10,2),
    IN p_FuelLevelStart VARCHAR(20),
    IN p_Notes TEXT,
    IN p_UserId INT
)
BEGIN
    -- FIXED: Using 'OdometerStart' and 'FuelLevelStart' to match your table
    INSERT INTO Rentals (
        VehicleId, 
        CustomerId, 
        PickupDate, 
        ScheduledReturnDate, 
        TotalAmount, 
        OdometerStart,    -- Was StartOdometer
        FuelLevelStart,   -- Was StartFuelLevel
        InitialCondition, -- Maps to Notes/Condition
        Status, 
        ProcessedBy, 
        ReservationId
    ) VALUES (
        p_VehicleId, 
        p_CustomerId, 
        p_PickupDate, 
        p_ReturnDate, 
        p_TotalAmount, 
        p_OdometerStart, 
        p_FuelLevelStart, 
        p_Notes, 
        'Ongoing', 
        p_UserId, 
        NULL
    );

    -- Mark Vehicle as Rented
    UPDATE Vehicles 
    SET Status = 'Rented' 
    WHERE VehicleId = p_VehicleId;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_DeleteCustomer
DROP PROCEDURE IF EXISTS `sp_DeleteCustomer`;
DELIMITER //
CREATE PROCEDURE `sp_DeleteCustomer`(IN p_CustomerId INT)
BEGIN
    DELETE FROM Customers WHERE CustomerId = p_CustomerId;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_DeleteVehicle
DROP PROCEDURE IF EXISTS `sp_DeleteVehicle`;
DELIMITER //
CREATE PROCEDURE `sp_DeleteVehicle`(
    IN p_VehicleId INT
)
BEGIN
    -- First, check if vehicle has any active rentals
    IF EXISTS (SELECT 1 FROM rentals WHERE VehicleId = p_VehicleId AND ReturnDate IS NULL) THEN
        SIGNAL SQLSTATE '45000' 
        SET MESSAGE_TEXT = 'Cannot delete vehicle with active rentals.';
    ELSE
        -- Soft delete (or hard delete if you prefer)
        UPDATE vehicles 
        SET IsActive = 0 
        WHERE VehicleId = p_VehicleId;
        
        -- Or hard delete:
        -- DELETE FROM vehicles WHERE VehicleId = p_VehicleId;
    END IF;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_GetAllCategories
DROP PROCEDURE IF EXISTS `sp_GetAllCategories`;
DELIMITER //
CREATE PROCEDURE `sp_GetAllCategories`()
BEGIN
    -- We select directly from your correct table name
    SELECT CategoryId, CategoryName 
    FROM vehiclecategories 
    ORDER BY CategoryName;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_GetAllCustomers
DROP PROCEDURE IF EXISTS `sp_GetAllCustomers`;
DELIMITER //
CREATE PROCEDURE `sp_GetAllCustomers`()
BEGIN
    SELECT * FROM Customers ORDER BY CustomerId DESC;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_GetAllRentals
DROP PROCEDURE IF EXISTS `sp_GetAllRentals`;
DELIMITER //
CREATE PROCEDURE `sp_GetAllRentals`()
BEGIN
    SELECT 
        r.RentalId,
        r.VehicleId, 
        r.CustomerId,
        CONCAT(v.Make, ' ', v.Model, ' - ', v.LicensePlate) AS VehicleName, 
        CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName,
        r.PickupDate AS RentalDate, 
        r.ScheduledReturnDate AS ScheduledReturn,
        
        -- NEW COLUMN ADDED HERE
        r.ActualReturnDate AS ActualReturn, 
        
        r.TotalAmount, 
        r.Status,
        v.ImagePath 
    FROM Rentals r
    JOIN Vehicles v ON r.VehicleId = v.VehicleId
    JOIN Customers c ON r.CustomerId = c.CustomerId
    ORDER BY r.RentalId DESC;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_GetAllReservations
DROP PROCEDURE IF EXISTS `sp_GetAllReservations`;
DELIMITER //
CREATE PROCEDURE `sp_GetAllReservations`()
BEGIN
    SELECT 
        r.ReservationId,
        r.CustomerId,
        r.VehicleId,
        -- Combine names for easier display
        CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName,
        CONCAT(v.Year, ' ', v.Make, ' ', v.Model) AS VehicleName,
        r.StartDate,
        r.EndDate,
        r.TotalAmount,
        r.Status,
        -- THIS IS THE MISSING COLUMN THAT FIXES YOUR IMAGE
        v.ImagePath 
    FROM Reservations r
    JOIN Customers c ON r.CustomerId = c.CustomerId
    JOIN Vehicles v ON r.VehicleId = v.VehicleId
    ORDER BY r.StartDate ASC;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_GetAllVehicleCategories
DROP PROCEDURE IF EXISTS `sp_GetAllVehicleCategories`;
DELIMITER //
CREATE PROCEDURE `sp_GetAllVehicleCategories`()
BEGIN
    SELECT 
        CategoryId,
        CategoryName,
        BaseDailyRate,
        Description
    FROM vehiclecategories
    ORDER BY CategoryName;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_GetAllVehicles
DROP PROCEDURE IF EXISTS `sp_GetAllVehicles`;
DELIMITER //
CREATE PROCEDURE `sp_GetAllVehicles`()
BEGIN
    SELECT 
        v.VehicleId, v.Make, v.Model, v.Year, v.Color, v.LicensePlate, 
        v.VIN, v.CategoryId, c.CategoryName, v.Transmission, v.FuelType, 
        v.SeatingCapacity, v.CurrentMileage, v.DailyRate, v.Status, 
        v.ImagePath  -- <--- FIX: Added ImagePath
    FROM Vehicles v
    JOIN VehicleCategories c ON v.CategoryId = c.CategoryId
    WHERE v.Status != 'Retired' -- <--- FIX: Hide retired vehicles
    ORDER BY v.VehicleId DESC;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_GetCustomerStats
DROP PROCEDURE IF EXISTS `sp_GetCustomerStats`;
DELIMITER //
CREATE PROCEDURE `sp_GetCustomerStats`(IN p_CustomerId INT)
BEGIN
    SELECT 
        (SELECT COUNT(*) FROM Rentals WHERE CustomerId = p_CustomerId) AS TotalRentals,
        (SELECT IFNULL(SUM(TotalAmount), 0) FROM Rentals WHERE CustomerId = p_CustomerId) AS TotalSpent,
        (SELECT COUNT(*) FROM Rentals WHERE CustomerId = p_CustomerId AND Status = 'Returned') AS CompletedRentals;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_GetDashboardOverdue
DROP PROCEDURE IF EXISTS `sp_GetDashboardOverdue`;
DELIMITER //
CREATE PROCEDURE `sp_GetDashboardOverdue`()
BEGIN
    SELECT 
        r.RentalId, 
        v.LicensePlate, 
        CONCAT(c.FirstName, ' ', c.LastName) AS CustomerName, 
        r.ScheduledReturnDate AS DueDate, 
        DATEDIFF(NOW(), r.ScheduledReturnDate) AS DaysLate
    FROM Rentals r
    -- WE JOIN 3 TABLES TO FIND THE CUSTOMER
    JOIN Reservations res ON r.ReservationId = res.ReservationId
    JOIN Customers c ON res.CustomerId = c.CustomerId
    JOIN Vehicles v ON res.VehicleId = v.VehicleId
    WHERE r.Status = 'Active' AND r.ScheduledReturnDate < NOW()
    ORDER BY r.ScheduledReturnDate ASC 
    LIMIT 10;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_GetDashboardStats
DROP PROCEDURE IF EXISTS `sp_GetDashboardStats`;
DELIMITER //
CREATE PROCEDURE `sp_GetDashboardStats`()
BEGIN
    SELECT 
        -- 1. Count Total Vehicles
        (SELECT COUNT(*) FROM Vehicles) AS TotalVehicles,
        
        -- 2. Count Available Vehicles
        (SELECT COUNT(*) FROM Vehicles WHERE Status = 'Available') AS AvailableVehicles,
        
        -- 3. Count Rented Vehicles
        (SELECT COUNT(*) FROM Vehicles WHERE Status = 'Rented') AS RentedVehicles,
        
        -- 4. Calculate Revenue for the CURRENT Month
        (SELECT COALESCE(SUM(TotalAmount), 0) 
         FROM Rentals 
         WHERE MONTH(PickupDate) = MONTH(CURRENT_DATE()) 
           AND YEAR(PickupDate) = YEAR(CURRENT_DATE())
        ) AS RevenueMonth,
        
        -- 5. Count Overdue Rentals (Active rentals past return date)
        (SELECT COUNT(*) 
         FROM Rentals 
         WHERE Status = 'Rented' 
           AND ScheduledReturnDate < NOW()
        ) AS OverdueCount;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_GetMonthlyRevenue
DROP PROCEDURE IF EXISTS `sp_GetMonthlyRevenue`;
DELIMITER //
CREATE PROCEDURE `sp_GetMonthlyRevenue`()
BEGIN
    SELECT 
        DATE_FORMAT(StartDate, '%b') AS MonthName, 
        MONTH(StartDate) AS MonthNum,
        SUM(TotalAmount) AS TotalRevenue 
    FROM Reservations 
    WHERE YEAR(StartDate) = YEAR(CURDATE())
    GROUP BY MONTH(StartDate), DATE_FORMAT(StartDate, '%b')
    ORDER BY MONTH(StartDate) ASC;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_GetRecentCustomers
DROP PROCEDURE IF EXISTS `sp_GetRecentCustomers`;
DELIMITER //
CREATE PROCEDURE `sp_GetRecentCustomers`()
BEGIN
    SELECT CustomerId, CONCAT(FirstName, ' ', LastName) AS Name, Email, Phone, DATE_FORMAT(CreatedDate, '%Y-%m-%d') AS Added 
    FROM Customers ORDER BY CustomerId DESC LIMIT 5;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_GetVehicleById
DROP PROCEDURE IF EXISTS `sp_GetVehicleById`;
DELIMITER //
CREATE PROCEDURE `sp_GetVehicleById`(IN p_VehicleId INT)
BEGIN
    SELECT * FROM Vehicles WHERE VehicleId = p_VehicleId;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_GetVehicleDetails
DROP PROCEDURE IF EXISTS `sp_GetVehicleDetails`;
DELIMITER //
CREATE PROCEDURE `sp_GetVehicleDetails`(IN p_VehicleId INT)
BEGIN
    SELECT 
        v.VehicleId,
        v.Make,
        v.Model,
        v.Year,
        v.Color,
        v.LicensePlate,
        v.VIN,
        v.CategoryId,
        c.CategoryName,
        c.BaseDailyRate,
        c.Description AS CategoryDescription,
        v.Transmission,
        v.FuelType,
        v.SeatingCapacity,
        v.CurrentMileage,
        v.Status,
        v.DailyRate,
        v.Features,
        v.CreatedDate,
        vi.ImagePath
    FROM vehicles v
    LEFT JOIN vehiclecategories c ON v.CategoryId = c.CategoryId
    LEFT JOIN vehicle_images vi ON v.VehicleId = vi.VehicleId 
        AND vi.IsPrimary = 1
    WHERE v.VehicleId = p_VehicleId;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_RegisterUser
DROP PROCEDURE IF EXISTS `sp_RegisterUser`;
DELIMITER //
CREATE PROCEDURE `sp_RegisterUser`(
    IN p_Username VARCHAR(50), 
    IN p_Password VARCHAR(255), 
    IN p_Role VARCHAR(20), 
    IN p_Email VARCHAR(100), 
    IN p_Phone VARCHAR(20)
)
BEGIN
    -- Check for duplicates
    IF EXISTS (SELECT 1 FROM Users WHERE Username = p_Username) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Username already taken.';
    ELSE
        -- Insert new user
        INSERT INTO Users (Username, PasswordHash, FullName, Role, Email, Phone, IsActive)
        VALUES (p_Username, p_Password, 'New User', p_Role, p_Email, p_Phone, 1);
    END IF;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_RetireVehicle
DROP PROCEDURE IF EXISTS `sp_RetireVehicle`;
DELIMITER //
CREATE PROCEDURE `sp_RetireVehicle`(IN p_VehicleId INT)
BEGIN
    -- Check if the vehicle has any reservation history
    IF EXISTS (SELECT 1 FROM Reservations WHERE VehicleId = p_VehicleId) THEN
        -- HAS HISTORY: Don't delete, just mark as Retired (Soft Delete)
        UPDATE Vehicles SET Status = 'Retired' WHERE VehicleId = p_VehicleId;
    ELSE
        -- NO HISTORY: Safe to permanently delete
        DELETE FROM Vehicles WHERE VehicleId = p_VehicleId;
    END IF;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_ReturnVehicle
DROP PROCEDURE IF EXISTS `sp_ReturnVehicle`;
DELIMITER //
CREATE PROCEDURE `sp_ReturnVehicle`(
    IN p_RentalId INT,
    IN p_VehicleId INT,
    IN p_ReturnDate DATETIME,
    IN p_OdometerEnd DECIMAL(10,2),
    IN p_FuelLevelEnd VARCHAR(50), 
    IN p_FinalCondition TEXT
)
BEGIN
    DECLARE v_ReservationId INT;

    -- Find the Reservation ID connected to this Rental
    SELECT ReservationId INTO v_ReservationId 
    FROM Rentals 
    WHERE RentalId = p_RentalId;

    START TRANSACTION;

    -- 1. Update Rental: Save return details and mark as Completed
    UPDATE Rentals 
    SET ActualReturnDate = p_ReturnDate,
        OdometerEnd = p_OdometerEnd,
        FuelLevelEnd = p_FuelLevelEnd,
        FinalCondition = p_FinalCondition,
        Status = 'Completed' 
    WHERE RentalId = p_RentalId;

    -- 2. Update Vehicle: Make it available again & update total mileage
    UPDATE Vehicles 
    SET Status = 'Available', 
        CurrentMileage = p_OdometerEnd 
    WHERE VehicleId = p_VehicleId;

    -- 3. Update Reservation: Close the reservation
    UPDATE Reservations 
    SET Status = 'Completed' 
    WHERE ReservationId = v_ReservationId;

    COMMIT;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_SearchCustomers
DROP PROCEDURE IF EXISTS `sp_SearchCustomers`;
DELIMITER //
CREATE PROCEDURE `sp_SearchCustomers`(IN p_SearchTerm VARCHAR(100))
BEGIN
    SELECT * FROM Customers 
    WHERE FirstName LIKE CONCAT('%', p_SearchTerm, '%') 
       OR LastName LIKE CONCAT('%', p_SearchTerm, '%')
       OR Phone LIKE CONCAT('%', p_SearchTerm, '%')
       OR LicenseNumber LIKE CONCAT('%', p_SearchTerm, '%')
    ORDER BY CustomerId DESC;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_UpdateCustomer
DROP PROCEDURE IF EXISTS `sp_UpdateCustomer`;
DELIMITER //
CREATE PROCEDURE `sp_UpdateCustomer`(
    IN p_CustomerId INT,
    IN p_FirstName VARCHAR(100),
    IN p_LastName VARCHAR(100),
    IN p_Email VARCHAR(100),
    IN p_Phone VARCHAR(20),
    IN p_Address TEXT,
    IN p_LicenseNumber VARCHAR(50),
    IN p_LicenseExpiry DATETIME,
    IN p_LicenseState VARCHAR(50), -- New Column
    IN p_DOB DATETIME,
    IN p_CustomerType VARCHAR(50),
    IN p_EmergencyName VARCHAR(100),
    IN p_EmergencyPhone VARCHAR(20),
    IN p_PhotoPath VARCHAR(255),
    IN p_IsBlacklisted BIT
)
BEGIN
    UPDATE Customers 
    SET 
        FirstName = p_FirstName,
        LastName = p_LastName,
        Email = p_Email,
        Phone = p_Phone,
        Address = p_Address,
        LicenseNumber = p_LicenseNumber,
        LicenseExpiry = p_LicenseExpiry,
        LicenseState = p_LicenseState, -- Update the state
        DateOfBirth = p_DOB,
        CustomerType = p_CustomerType,
        EmergencyContactName = p_EmergencyName,
        EmergencyContactPhone = p_EmergencyPhone,
        PhotoPath = p_PhotoPath,
        IsBlacklisted = p_IsBlacklisted
    WHERE CustomerId = p_CustomerId;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_UpdateVehicle
DROP PROCEDURE IF EXISTS `sp_UpdateVehicle`;
DELIMITER //
CREATE PROCEDURE `sp_UpdateVehicle`(
    IN p_VehicleId INT,
    IN p_Make VARCHAR(50),
    IN p_Model VARCHAR(50),
    IN p_Year INT,
    IN p_Color VARCHAR(30),
    IN p_LicensePlate VARCHAR(20),
    IN p_VIN VARCHAR(50),
    IN p_CategoryId INT,
    IN p_DailyRate DECIMAL(10,2),
    IN p_Transmission VARCHAR(20),
    IN p_FuelType VARCHAR(20),
    IN p_SeatingCapacity INT,
    IN p_CurrentMileage INT,
    IN p_ImagePath VARCHAR(255) -- <--- New Parameter
)
BEGIN
    UPDATE Vehicles SET 
        Make = p_Make, Model = p_Model, Year = p_Year, Color = p_Color,
        LicensePlate = p_LicensePlate, VIN = p_VIN, CategoryId = p_CategoryId,
        DailyRate = p_DailyRate, Transmission = p_Transmission, FuelType = p_FuelType,
        SeatingCapacity = p_SeatingCapacity, CurrentMileage = p_CurrentMileage,
        ImagePath = p_ImagePath -- <--- Update Column
    WHERE VehicleId = p_VehicleId;
END//
DELIMITER ;

-- Dumping structure for procedure vehiclerentaldb.sp_UpdateVehicleWithImage
DROP PROCEDURE IF EXISTS `sp_UpdateVehicleWithImage`;
DELIMITER //
CREATE PROCEDURE `sp_UpdateVehicleWithImage`(
    IN p_VehicleId INT,
    IN p_Make VARCHAR(50),
    IN p_Model VARCHAR(50),
    IN p_Year INT,
    IN p_Color VARCHAR(30),
    IN p_LicensePlate VARCHAR(20),
    IN p_VIN VARCHAR(50),
    IN p_CategoryId INT,
    IN p_Transmission VARCHAR(20),
    IN p_FuelType VARCHAR(20),
    IN p_SeatingCapacity INT,
    IN p_CurrentMileage DECIMAL(10,2),
    IN p_DailyRate DECIMAL(10,2),
    IN p_ImagePath VARCHAR(255) -- This parameter was missing logic
)
BEGIN
    UPDATE Vehicles
    SET 
        Make = p_Make,
        Model = p_Model,
        Year = p_Year,
        Color = p_Color,
        LicensePlate = p_LicensePlate,
        VIN = p_VIN,
        CategoryId = p_CategoryId,
        Transmission = p_Transmission,
        FuelType = p_FuelType,
        SeatingCapacity = p_SeatingCapacity,
        CurrentMileage = p_CurrentMileage,
        DailyRate = p_DailyRate,
        ImagePath = p_ImagePath -- Actually save the image path!
    WHERE VehicleId = p_VehicleId;
END//
DELIMITER ;

-- Dumping structure for table vehiclerentaldb.users
DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
  `UserId` int NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `FullName` varchar(100) NOT NULL,
  `Role` varchar(50) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `Phone` varchar(20) DEFAULT NULL,
  `IsActive` tinyint(1) DEFAULT '1',
  `CreatedDate` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `Username` (`Username`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table vehiclerentaldb.users: ~4 rows (approximately)
INSERT INTO `users` (`UserId`, `Username`, `PasswordHash`, `FullName`, `Role`, `Email`, `Phone`, `IsActive`, `CreatedDate`) VALUES
	(1, 'admin', '$2a$12$YourHashedPasswordHere', 'System Administrator', 'Admin', 'admin@rental.com', NULL, 1, '2025-12-11 14:19:16'),
	(2, 'leebag', '123456', '', 'Admin', 'lee@gmail.com', '09053414599', 1, '2025-12-11 14:25:58'),
	(3, 'dustinkun', '123456', 'New User', 'Admin', 'duskun@gmail.com', '0938652385442', 1, '2025-12-11 18:58:09'),
	(4, 'lee', 'MtViIulINroPTFLlzhXjrsOxoIe4ouvUj6RgrNmzJMK9LN9zvnkKN+/GwRj9pp68', 'New User', 'Admin', 'lee@gmail.com', '09053414599', 1, '2026-01-05 14:00:03');

-- Dumping structure for table vehiclerentaldb.vehiclecategories
DROP TABLE IF EXISTS `vehiclecategories`;
CREATE TABLE IF NOT EXISTS `vehiclecategories` (
  `CategoryId` int NOT NULL AUTO_INCREMENT,
  `CategoryName` varchar(50) NOT NULL,
  `BaseDailyRate` decimal(10,2) DEFAULT '0.00',
  `Description` text,
  PRIMARY KEY (`CategoryId`),
  UNIQUE KEY `CategoryName` (`CategoryName`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table vehiclerentaldb.vehiclecategories: ~5 rows (approximately)
INSERT INTO `vehiclecategories` (`CategoryId`, `CategoryName`, `BaseDailyRate`, `Description`) VALUES
	(1, 'Hatchback', 35.00, 'Compact city car'),
	(2, 'Sedan', 45.00, 'Standard 4-door car'),
	(3, 'SUV', 65.00, 'Sports Utility Vehicle'),
	(4, 'Pickup', 75.00, 'Truck with cargo bed'),
	(5, 'Van/Minibus', 90.00, 'Passenger or cargo van');

-- Dumping structure for table vehiclerentaldb.vehicles
DROP TABLE IF EXISTS `vehicles`;
CREATE TABLE IF NOT EXISTS `vehicles` (
  `VehicleId` int NOT NULL AUTO_INCREMENT,
  `Make` varchar(50) NOT NULL,
  `Model` varchar(50) NOT NULL,
  `Year` int NOT NULL,
  `Color` varchar(30) DEFAULT NULL,
  `LicensePlate` varchar(20) NOT NULL,
  `VIN` varchar(17) DEFAULT NULL,
  `CategoryId` int NOT NULL,
  `Transmission` enum('Manual','Automatic') DEFAULT 'Automatic',
  `FuelType` enum('Gasoline','Diesel','Electric','Hybrid') DEFAULT 'Gasoline',
  `SeatingCapacity` int DEFAULT '5',
  `CurrentMileage` decimal(10,2) DEFAULT '0.00',
  `Status` enum('Available','Rented','Reserved','Maintenance','OutOfService') DEFAULT 'Available',
  `DailyRate` decimal(10,2) NOT NULL,
  `ImagePath` varchar(255) DEFAULT NULL,
  `Features` text,
  `CreatedDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `CurrentFuelLevel` varchar(20) DEFAULT 'Full',
  PRIMARY KEY (`VehicleId`),
  UNIQUE KEY `LicensePlate` (`LicensePlate`),
  UNIQUE KEY `VIN` (`VIN`),
  KEY `idx_status` (`Status`),
  KEY `idx_category` (`CategoryId`),
  CONSTRAINT `vehicles_ibfk_1` FOREIGN KEY (`CategoryId`) REFERENCES `vehiclecategories` (`CategoryId`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table vehiclerentaldb.vehicles: ~15 rows (approximately)
INSERT INTO `vehicles` (`VehicleId`, `Make`, `Model`, `Year`, `Color`, `LicensePlate`, `VIN`, `CategoryId`, `Transmission`, `FuelType`, `SeatingCapacity`, `CurrentMileage`, `Status`, `DailyRate`, `ImagePath`, `Features`, `CreatedDate`, `CurrentFuelLevel`) VALUES
	(1, 'Toyota', 'Corolla', 2022, 'White', 'ABC123', '1HGCM82633A123456', 2, 'Automatic', 'Gasoline', 5, 0.00, 'Available', 45.00, NULL, NULL, '2025-12-11 14:19:16', 'Full'),
	(2, 'Honda', 'Civic', 2023, 'Maroon', 'DEF456', '2HGCM82633B654321', 2, 'Automatic', 'Gasoline', 5, 456678.00, 'Available', 48.00, NULL, NULL, '2025-12-11 14:19:16', 'Full'),
	(3, 'Ford', 'Explorer', 2022, 'Black', 'GHI789', '3FCM82633C789012', 3, 'Automatic', 'Gasoline', 5, 3453.00, 'Available', 65.00, 'C:\\Users\\lee\\Desktop\\Vehicle-Rental-Management-System\\Vehicle-Rental-Management-System\\bin\\Debug\\VehicleImages\\vehicle_3_20251230010954.jpg', NULL, '2025-12-11 14:19:16', 'Full'),
	(4, 'Toyota', 'Vios', 2023, 'Silver', 'NCA-1023', 'TV1234567890', 1, 'Automatic', 'Gasoline', 5, 15000.00, 'Available', 2000.00, 'C:\\Users\\lee\\Desktop\\Vehicle-Rental-Management-System\\Vehicle-Rental-Management-System\\bin\\Debug\\VehicleImages\\vehicle_4_20260103133416.jpg', NULL, '2025-12-11 14:45:06', 'Full'),
	(5, 'Toyota', 'Wigo', 2022, 'Red', 'DAE-5521', 'TW8877665544', 1, 'Automatic', 'Gasoline', 5, 22000.00, 'Rented', 1500.00, NULL, NULL, '2025-12-11 14:45:06', 'Full'),
	(6, 'Honda', 'City', 2024, 'White', 'ABC-9988', 'HC1122334455', 1, 'Automatic', 'Gasoline', 5, 5000.00, 'Available', 2500.00, NULL, NULL, '2025-12-11 14:45:06', 'Full'),
	(7, 'Ford', 'Territory', 2023, 'Blue', 'FEF-3321', 'FT9988776655', 2, 'Automatic', 'Gasoline', 5, 12000.00, 'Rented', 3000.00, NULL, NULL, '2025-12-11 14:45:06', 'Full'),
	(8, 'Toyota', 'Fortuner', 2021, 'Black', 'NBV-1122', 'TF5544332211', 3, 'Automatic', 'Diesel', 7, 35000.00, 'Rented', 4500.00, NULL, NULL, '2025-12-11 14:45:06', 'Full'),
	(9, 'Mitsubishi', 'Montero', 2022, 'Gray', 'MMP-7744', 'MM6677889900', 3, 'Automatic', 'Diesel', 7, 28501.00, 'Available', 4200.00, 'C:\\Users\\lee\\Desktop\\Vehicle-Rental-Management-System\\Vehicle-Rental-Management-System\\bin\\Debug\\VehicleImages\\vehicle_9_20260103125119.jpg', NULL, '2025-12-11 14:45:06', 'Full'),
	(10, 'Toyota', 'HiAce', 2019, 'White', 'TXT-5511', 'TH1122334400', 3, 'Manual', 'Diesel', 15, 60000.00, 'Available', 5000.00, NULL, NULL, '2025-12-11 14:45:06', 'Full'),
	(11, 'Nissan', 'Urvan', 2020, 'Silver', 'NUV-8822', 'NU9988776611', 3, 'Manual', 'Diesel', 15, 45000.00, 'Available', 4800.00, NULL, NULL, '2025-12-11 14:45:06', 'Full'),
	(12, 'Ford', 'Raptor', 2023, 'Orange', 'RAP-9911', 'FR2233445566', 4, 'Automatic', 'Diesel', 5, 10000.00, 'Available', 5500.00, 'C:\\Users\\lee\\Desktop\\Vehicle-Rental-Management-System\\Vehicle-Rental-Management-System\\bin\\Debug\\VehicleImages\\vehicle_12_20251230010938.jpg', NULL, '2025-12-11 14:45:06', 'Full'),
	(13, 'Mitsubishi', 'Mirage G4', 2021, 'Gray', 'MMG-1100', 'MM1231231234', 1, 'Manual', 'Gasoline', 5, 32000.00, 'Available', 1800.00, NULL, NULL, '2025-12-11 14:45:06', 'Full'),
	(30, 'Honda', 'Civic', 2020, 'White', 'BNDF 349U', 'WBHGHNWTRHWTH', 2, 'Automatic', 'Gasoline', 5, 1000000.00, 'Available', 4325.00, 'C:\\Users\\lee\\Desktop\\Vehicle-Rental-Management-System\\Vehicle-Rental-Management-System\\bin\\Debug\\VehicleImages\\vehicle_30_20251230003642.jpg', NULL, '2025-12-30 00:36:42', 'Full'),
	(32, 'Ford', 'Ranger', 2024, 'Orange', '325 NSD', '4237GBLWVFEF42', 4, 'Automatic', 'Gasoline', 5, 245.00, 'Rented', 23546.00, 'C:\\Users\\lee\\Desktop\\Vehicle-Rental-Management-System\\Vehicle-Rental-Management-System\\bin\\Debug\\VehicleImages\\vehicle_32_20251230010528.jpg', NULL, '2025-12-30 01:05:28', 'Full');

-- Dumping structure for table vehiclerentaldb.vehicle_images
DROP TABLE IF EXISTS `vehicle_images`;
CREATE TABLE IF NOT EXISTS `vehicle_images` (
  `ImageId` int NOT NULL AUTO_INCREMENT,
  `VehicleId` int NOT NULL,
  `ImagePath` varchar(500) NOT NULL,
  `ThumbnailPath` varchar(500) DEFAULT NULL,
  `ImageType` varchar(50) DEFAULT 'exterior',
  `IsPrimary` tinyint(1) DEFAULT '0',
  `UploadDate` datetime DEFAULT CURRENT_TIMESTAMP,
  `Description` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`ImageId`),
  KEY `VehicleId` (`VehicleId`),
  CONSTRAINT `vehicle_images_ibfk_1` FOREIGN KEY (`VehicleId`) REFERENCES `vehicles` (`VehicleId`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Dumping data for table vehiclerentaldb.vehicle_images: ~6 rows (approximately)
INSERT INTO `vehicle_images` (`ImageId`, `VehicleId`, `ImagePath`, `ThumbnailPath`, `ImageType`, `IsPrimary`, `UploadDate`, `Description`) VALUES
	(1, 3, 'C:\\Users\\lee\\Desktop\\Vehicle-Rental-Management-System\\Vehicle-Rental-Management-System\\bin\\Debug\\VehicleImages\\vehicle_3_20251230010617.jpg', NULL, 'exterior', 1, '2025-12-30 01:06:17', NULL),
	(2, 2, 'C:\\Users\\lee\\Desktop\\Vehicle-Rental-Management-System\\Vehicle-Rental-Management-System\\bin\\Debug\\VehicleImages\\vehicle_2_20251229155016.jpg', NULL, 'exterior', 1, '2025-12-29 15:50:16', NULL),
	(3, 9, '', NULL, 'exterior', 1, '2025-12-29 15:50:41', NULL),
	(4, 12, 'C:\\Users\\lee\\Desktop\\Vehicle-Rental-Management-System\\Vehicle-Rental-Management-System\\bin\\Debug\\VehicleImages\\vehicle_12_20251230010549.jpg', NULL, 'exterior', 1, '2025-12-30 01:05:49', NULL),
	(5, 8, '', NULL, 'exterior', 1, '2025-12-29 23:26:36', NULL),
	(6, 30, 'C:\\Users\\lee\\Desktop\\Vehicle-Rental-Management-System\\Vehicle-Rental-Management-System\\bin\\Debug\\VehicleImages\\vehicle_30_20251230003642.jpg', NULL, 'exterior', 1, '2025-12-30 00:37:21', NULL);

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
