using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Registration { get; set; }
        public double Odometer { get; set; }
        public double Tank { get; set; }
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
        /// MAIN VEHICLE LIST - GET IT FROM THE FILE
        /// </summary>
        private static List<Vehicle> _vehicleList { get { return Sql.Load<Vehicle>(new Vehicle()); } }
        //private static List<Vehicle> _vehicleList { get { return JsonData.Load<Vehicle>(); } }
        /// <summary>
        /// MAIN VEHICLE LIST - PUBLIC
        /// </summary>
        [JsonIgnore]
        public static List<Vehicle> vehicleList { get { return _vehicleList; } }
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
        private Vehicle(int id, string manufacturer, string model, int makeYear, string registrationNumber, double odometerReading, double tankCapacity)
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
        /// <summary>
        /// CREATE A NEW VEHICLE AND ADD TO THE LIST
        /// </summary>
        /// <param name="manufacturer"></param>
        /// <param name="model"></param>
        /// <param name="makeYear"></param>
        /// <param name="registrationNumber"></param>
        /// <param name="odometerReading"></param>
        /// <param name="tankCapacity"></param>
        public static void AddVehicle(string manufacturer, string model, int makeYear, string registrationNumber, double odometerReading, double tankCapacity)
        {
            List<Vehicle> vehicleList = _vehicleList;

            var vId = (vehicleList.Count > 0 ? vehicleList.Last().Id + 1 : 1);

            vehicleList.Add(new Vehicle(vId, manufacturer, model, makeYear, registrationNumber, odometerReading, tankCapacity));

            //JsonData.Save(vehicleList);
            Sql.Save(vehicleList);

            //Service.recordService(vId);
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
        public static void EditVehicle(int id, string manufacturer, string model, int makeYear, string registrationNumber, double odometerReading, double tankCapacity)
        {
            List<Vehicle> vehicleList = _vehicleList;

            Vehicle v = vehicleList.Where(x => x.Id == id).FirstOrDefault();

            v.Manufacturer = manufacturer;
            v.Model = model;
            v.Year = makeYear;
            v.Registration = registrationNumber;
            v.Odometer = odometerReading;
            v.Tank = tankCapacity;
            v.ModifiedDate = DateTime.Now;

            vehicleList.ToArray().SetValue(v, 0);

            JsonData.Save(vehicleList);
        }
        /// <summary>
        /// UPDATES THE VEHICLE ODOMETER
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <param name="newOdometerReading"></param>
        /// <returns></returns>
        public static string UpdateOdometer(int vehicleId, double newOdometerReading)
        {
            List<Vehicle> vehicleList = _vehicleList;

            Vehicle v = vehicleList.Where(x => x.Id == vehicleId).FirstOrDefault();

            if (newOdometerReading < v.Odometer)
            {
                return "New Odometer is lower than actual";
            }

            v.Odometer = newOdometerReading;
            v.ModifiedDate = DateTime.Now;

            vehicleList.ToArray().SetValue(v, 0);

            JsonData.Save(vehicleList);
            
            return "";
        }
        /// <summary>
        /// REMOVE A VEHICLE FROM THE LIST
        /// </summary>
        /// <param name="v"></param>
        public static void DeleteVehicle(Vehicle v)
        {
            List<Vehicle> vehicleList = _vehicleList;
            vehicleList.Remove(vehicleList.Where(x => x.Id == v.Id).FirstOrDefault());
            JsonData.Save(vehicleList);
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

            double economy = FuelPurchase.GetFuelEconomy(v.Id);

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
