using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assessment2
{
    /// <summary>
    /// CLASS THAT HANDLES THE SERVICE'S OPERATIONS
    /// </summary>
    public class Service
    {
        /// <summary>
        /// CONSTANT FOR HOW MANY KM BEFORE THE CAR NEED A SERVICE
        /// </summary>
        const decimal SERVICE_KILOMETER_LIMIT = 10000;
        /// <summary>
        /// SERVICE MAIN PROPERTIES
        /// </summary>
        public decimal lastServiceOdometerKm { get; set; }
        //        public int serviceCount { get; set; }
        public DateTime lastServiceDate { get; set; }
        public ulong vehicleId { get; set; }
        /// <summary>
        /// MAIN SERVICE LIST - GET IT FROM THE FILE
        /// </summary>
        private static List<Service> _serviceList { get { return Sql.sqlSelectAll<Service>(); } }
        /// <summary>
        /// MAIN SERVICE LIST - PUBLIC
        /// </summary>
        [JsonIgnore]
        public static List<Service> serviceList { get { return _serviceList; } }
        /// <summary>
        /// CONSTRUCTOR
        /// </summary>
        public Service() { }
        /// <summary>
        /// CONSTRUCTOR THAT SET THE SERVICE PROPERTIES
        /// </summary>
        /// <param name="vehicleId"></param>
        public Service(ulong vehicleId)
        {
            decimal odometer = Vehicle.getVehicle(vehicleId).Odometer;

            this.vehicleId = vehicleId;
            lastServiceOdometerKm = odometer;
            //serviceCount = GetServiceCount() + 1;
            lastServiceDate = DateTime.Now;
        }
        /// <summary>
        /// RETURN HOW MANY KM SINCE LAST SERVICE
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static decimal GetKmSinceLastService(Vehicle v)
        {
            return (v.Odometer - serviceList.Where(x => x.vehicleId == v.Id).Max(m => m.lastServiceOdometerKm));
        }

        /// <summary>
        /// CREATE A NEW SERVICE TO THE VEHICLE
        /// </summary>
        /// <param name="vehicleId"></param>
        public static void recordService(ulong vehicleId)
        {
            Sql.sqlInsert(new Service(vehicleId));
        }

        /// <summary>
        /// RETURN HOW MANY SERVICES THE VEHICLE HAD
        /// </summary>
        /// <param name="vId"></param>
        /// <returns></returns>
        public int GetServiceCount(ulong vId = 0)
        {
            return serviceList.Where(x => x.vehicleId == ((vId == 0) ? vehicleId : vId)).ToList().Count;
        }

        /// <summary>
        /// RETURN IF THE VEHICE IS DUE TO A SERVICE
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public static bool isVehicleDueToService(ulong vehicleId)
        {
            decimal nextOdoService = SERVICE_KILOMETER_LIMIT;

            decimal actualOdo = Vehicle.getVehicle(vehicleId).Odometer;

            List<Service> sList = serviceList.Where(x => x.vehicleId == vehicleId).ToList();

            if (sList.Count > 0)
            {
                nextOdoService += sList.Max(x => x.lastServiceOdometerKm);
            }

            return (actualOdo > nextOdoService);
        }
    }
}
