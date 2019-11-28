
/* RUN WITH THE ROOT USER
CREATE DATABASE `nmt_fleet_manager` CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_general_ci';

CREATE USER `nmt_fleet_manager_user`@`localhost` IDENTIFIED WITH mysql_native_password BY 'Password1';

GRANT Alter, Alter Routine, Create, Create Routine, Create Temporary Tables, Create User, Create View, Delete, Drop, Event, Execute, File, Grant Option, Index, Insert, Lock Tables, Process, References, Reload, Replication Client, Replication Slave, Select, Show Databases, Show View, Shutdown, Super, Trigger, Update ON *.* TO `nmt_fleet_manager_user`@`localhost`;

FLUSH PRIVILEGES
*/

CREATE TABLE IF NOT EXISTS `Vehicle` (
`Id` 						bigint 			UNSIGNED NOT NULL AUTO_INCREMENT,
`Manufacturer`	varchar(64),
`Model` 				varchar(64),
`Year`			    int,
`Registration`	varchar(16),
`Odometer` 			DECIMAL(18,2),
`Tank` 					DECIMAL(5,2),
`ModifiedDate` 	datetime(0) DEFAULT CURRENT_TIMESTAMP(0),
`Created_At` 		datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
`Updated_At` 		datetime(0) DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP(0),
PRIMARY KEY (`Id`)
);

CREATE TABLE IF NOT EXISTS `Rental` (
`Id` 						bigint 			UNSIGNED NOT NULL AUTO_INCREMENT,
`vehicleId`			bigint 			UNSIGNED NOT NULL,
`customerName`	varchar(256),
`rentType` 			varchar(4),
`startOdometer` DECIMAL(18,2),
`endOdometer` 	DECIMAL(18,2),
`startDate` 		datetime(0) NOT NULL,
`endDate` 			datetime(0),
`notes`					varchar(1024),
`totalPrice` 		DECIMAL(18,2),
`ModifiedDate` 	datetime(0) DEFAULT CURRENT_TIMESTAMP(0),
`Created_At` 		datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
`Updated_At` 		datetime(0) DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP(0),
PRIMARY KEY (`Id`)
);

CREATE TABLE IF NOT EXISTS `Service` (
`Id` 										bigint 			UNSIGNED NOT NULL AUTO_INCREMENT,
`vehicleId` 						bigint 			UNSIGNED NOT NULL,        
`lastServiceOdometerKm`	DECIMAL(18,2),
`lastServiceDate` 			datetime(0) DEFAULT CURRENT_TIMESTAMP(0),
`Created_At` 						datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
`Updated_At` 						datetime(0) DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP(0),
PRIMARY KEY (`Id`)
);


CREATE TABLE IF NOT EXISTS `FuelPurchase` (
`Id` 							bigint 			UNSIGNED NOT NULL AUTO_INCREMENT,
`vehicleId` 			bigint 			UNSIGNED NOT NULL,        
`odometerReading`	DECIMAL(18,2),
`quantity`				DECIMAL(18,2),
`price`						DECIMAL(18,2),
`purchaseDate` 		datetime(0),
`Created_At` 			datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
`Updated_At` 			datetime(0) DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP(0),
PRIMARY KEY (`Id`)
);
