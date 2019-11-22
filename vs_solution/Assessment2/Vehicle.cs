using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using static Assessment2.Vehicle;

namespace Assessment2
{
    /// <summary>
    /// CLASS THAT HANDLES THE VEHICLE'S OPERATIONS
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// VEHICLE MAIN PROPERTIES
        /// </summary>
        public ulong Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Registration { get; set; }
        public decimal Odometer { get; set; }
        public decimal Tank { get; set; }
        public DateTime ModifiedDate { get; set; }
        /// <summary>
        /// RETURN VEHICLE DESCRIPTION
        /// </summary>
        [JsonIgnore]
        public string vehicleDescription
        {
            get
            {
                return Manufacturer + " - " + Model;
            }
        }
        /// <summary>
        /// RETURN THE VEHICLE STATUS
        /// </summary>
        [JsonIgnore]
        public statusType Status
        {
            get
            {
                statusType status = statusType.Available;

                if (Service.isVehicleDueToService(Id))
                {
                    status = statusType.NeedService;
                    btnServiceVisibility = Visibility.Visible;
                }
                else
                {
                    var nAux = Rental.rentalList.Where(x => x.vehicleId == Id && x.totalPrice == 0).ToList().Count;

                    if (nAux > 0)
                    {
                        status = statusType.Rented;
                    }
                }

                return status;
            }
        }
        /// <summary>
        /// RETURN THE FRIENDLY STATUS TYPE USING EXTENSION
        /// </summary>
        [JsonIgnore]
        public string StatusText
        {
            get
            {
                return Status.EnumText();
            }
        }
        /// <summary>
        /// VEHICLE STATUS
        /// </summary>
        public enum statusType
        {
            Available,
            Rented,
            NeedService
        }

        private Visibility _btnServiceVisibility = Visibility.Hidden;
        /// <summary>
        /// USED TO SHOW THE SERVICE BUTTON ON THE VEHICLE LIST FORM
        /// </summary>
        [JsonIgnore]
        public Visibility btnServiceVisibility
        {
            get
            {
                return _btnServiceVisibility;
            }
            set
            {
                _btnServiceVisibility = value;
            }
        }
        /// <summary>
        /// MAIN VEHICLE LIST - GET IT FROM THE SQL
        /// </summary>
        private static List<Vehicle> _vehicleList { get { return Sql.sqlSelectAll<Vehicle>(); } }
        /// <summary>
        /// MAIN VEHICLE LIST - PUBLIC
        /// </summary>
        [JsonIgnore]
        public static List<Vehicle> vehicleList { get { return _vehicleList; } }

        public static Vehicle getVehicle(ulong id)
        {
            return Sql.sqlSelect<Vehicle>(id);
        }

        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public Vehicle() { }

        /// <summary>
        /// CONSTRUCTOR WHICH SET THE VEHICLE PROPERTIES
        /// </summary>
        /// <param name="id"></param>
        /// <param name="manufacturer"></param>
        /// <param name="model"></param>
        /// <param name="makeYear"></param>
        /// <param name="registrationNumber"></param>
        /// <param name="odometerReading"></param>
        /// <param name="tankCapacity"></param>
        private Vehicle(ulong id, string manufacturer, string model, int makeYear, string registrationNumber, decimal odometerReading, decimal tankCapacity)
        {
            Id = id;
            Manufacturer = manufacturer;
            Model = model;
            Year = makeYear;
            Registration = registrationNumber;
            Odometer = odometerReading;
            Tank = tankCapacity;
            ModifiedDate = DateTime.Now;
        }

        private Vehicle(ulong id, decimal odometerReading)
        {
            Id = id;
            Odometer = odometerReading;
            ModifiedDate = DateTime.Now;
        }
        /// <summary>
        /// CREATE A NEW VEHICLE AND ADD TO THE LIST
        /// </summary>
        /// <param name="manufacturer"></param>
        /// <param name="model"></param>
        /// <param name="makeYear"></param>
        /// <param name="registrationNumber"></param>
        /// <param name="odometerReading"></param>
        /// <param name="tankCapacity"></param>
        public static bool AddVehicle(string manufacturer, string model, int makeYear, string registrationNumber, decimal odometerReading, decimal tankCapacity)
        {
            if (Sql.sqlInsert(new Vehicle(0, manufacturer, model, makeYear, registrationNumber, odometerReading, tankCapacity)))
            {
                Service.recordService(vehicleList.LastOrDefault().Id);
            }

            return true;
        }
        /// <summary>
        /// UPDATE THE VEHICLE'S INFORMATION
        /// </summary>
        /// <param name="id"></param>
        /// <param name="manufacturer"></param>
        /// <param name="model"></param>
        /// <param name="makeYear"></param>
        /// <param name="registrationNumber"></param>
        /// <param name="odometerReading"></param>
        /// <param name="tankCapacity"></param>
        public static bool EditVehicle(ulong id, string manufacturer, string model, int makeYear, string registrationNumber, decimal odometerReading, decimal tankCapacity)
        {
            return Sql.sqlUpdate(new Vehicle(id, manufacturer, model, makeYear, registrationNumber, odometerReading, tankCapacity));
        }
        /// <summary>
        /// UPDATES THE VEHICLE ODOMETER
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <param name="newOdometerReading"></param>
        /// <returns></returns>
        public static string UpdateOdometer(ulong vehicleId, decimal newOdometerReading)
        {
            Vehicle v = Sql.sqlSelect<Vehicle>(vehicleId);

            if (newOdometerReading < v.Odometer)
            {
                return "New Odometer is lower than actual";
            }

            Sql.sqlUpdate(new Vehicle(vehicleId, newOdometerReading));

            return "";
        }
        /// <summary>
        /// REMOVE A VEHICLE FROM THE LIST
        /// </summary>
        /// <param name="v"></param>
        public static void DeleteVehicle(Vehicle v)
        {
            Sql.sqlDelete(v);
        }

        /// <summary>
        /// RETURN A STRING WITH ALL THE NECESSARY INFORMATION OF THE VEHICLE
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string printDetails(Vehicle v)
        {
            StringBuilder sAux2 = new StringBuilder();
            sAux2.AppendFormat("Vehicle: {0} {1} {2}", v.Manufacturer, v.Model, v.Year);
            sAux2.AppendLine();
            sAux2.AppendFormat("Registration No: {0}", v.Registration);
            sAux2.AppendLine();
            sAux2.AppendFormat("Total services: {0}", new Service().GetServiceCount(v.Id));
            sAux2.AppendLine();
            sAux2.AppendFormat("Revenue recorded: {0:C}", Rental.GetTotalRevenue(v.Id));
            sAux2.AppendLine();
            sAux2.AppendFormat("Kilometres since last service: {0:#,###0} km", Service.GetKmSinceLastService(v));
            sAux2.AppendLine();

            decimal economy = FuelPurchase.GetFuelEconomy(v.Id);

            if (economy > 0)
            {
                sAux2.AppendFormat("Fuel economy: {0:#,###0} km/L", economy);
            }
            else
            {
                sAux2.AppendFormat("Fuel economy: Not Available");
            }

            sAux2.AppendLine();
            sAux2.AppendFormat("Requires a service: {0}", v.Status == statusType.NeedService ? "Yes" : "No");
            sAux2.AppendLine();

            return sAux2.ToString();
        }
    }

    public static class ExtensionMethods
    {
        /// <summary>
        /// EXTENSION METHOD TO RETURN A MORE FRIENDLY TEXT FROM THE VEHICLE STATUS
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string EnumText(this statusType e)
        {
            switch (e)
            {
                case statusType.NeedService:
                    return "Needs Service";
            }
            return e.ToString();
        }
    }
}
