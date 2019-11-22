using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assessment2
{    /// <summary>
     /// CLASS THAT HANDLES THE FUEL'S OPERATIONS
     /// </summary>
    public class FuelPurchase
    {
        /// <summary>
        /// FUEL MAIN PROPERTIES
        /// </summary>
        public decimal odometerReading { get; set; }
        public ulong vehicleId { get; set; }
        public DateTime purchaseDate { get; set; }
        public decimal quantity { get; set; }
        public decimal price { get; set; }

        /// <summary>
        /// MAIN FUEL LIST - GET IT FROM THE FILE
        /// </summary>
        private static List<FuelPurchase> _fuelList { get { return JsonData.Load<FuelPurchase>(); } }
        /// <summary>
        /// MAIN FUEL LIST - PUBLIC
        /// </summary>
        [JsonIgnore]
        public static List<FuelPurchase> fuelList { get { return _fuelList; } }
        /// <summary>
        /// RETURN THE VEHICLE FUEL ECONOMY 
        /// </summary>
        /// <param name="vehicheId"></param>
        /// <returns></returns>
        public static decimal GetFuelEconomy(ulong vehicheId)
        {
            var list = _fuelList.Where(x => x.vehicleId == vehicheId).ToList();

            decimal totalFuel = 0;
            decimal totalKM = 0;

            if (list.Count > 0)
            {
                totalFuel = list.Sum(s => s.quantity);
                totalKM = list.Max(m => m.odometerReading) - list.Min(m => m.odometerReading);
            }

            return totalKM / totalFuel;
        }
        /// <summary>
        /// CONSTRUCTOR THAT SET THE FUEL PROPERTIES
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <param name="odometer"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        public FuelPurchase(ulong vehicleId, decimal odometer, decimal quantity, decimal price)
        {
            this.vehicleId = vehicleId;
            this.odometerReading = odometer;
            this.quantity = quantity;
            this.price = price;
            this.purchaseDate = DateTime.Now;
        }
        /// <summary>
        /// CREATE A NEW FUEL AND ADD TO THE LIST
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="odometer"></param>
        /// <param name="quantity"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static string AddPurchaseFuel(Vehicle vehicle, decimal odometer, decimal quantity, decimal price)
        {
            if (vehicle.Tank < quantity)
            {
                return "Quantity is higher than tank capacity";
            }

            string sMessage = Vehicle.UpdateOdometer(vehicle.Id, odometer);

            if (!string.IsNullOrEmpty(sMessage))
            {
                return sMessage;
            }

            List<FuelPurchase> fList = _fuelList;
            fList.Add(new FuelPurchase(vehicle.Id, odometer, quantity, price));

            JsonData.Save(fList);

            return "";
        }
    }
}
