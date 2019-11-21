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
        const double SERVICE_KILOMETER_LIMIT = 10000;
        /// <summary>
        /// SERVICE MAIN PROPERTIES
        /// </summary>
        public double lastServiceOdometerKm { get; set; }
        public int serviceCount { get; set; }
        public DateTime lastServiceDate { get; set; }
        public int vehicleId { get; set; }
        /// <summary>
        /// MAIN SERVICE LIST - GET IT FROM THE FILE
        /// </summary>
        private static List<Service> _serviceList { get { return JsonData.Load<Service>(); } }
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
        public Service(int vehicleId)
        {
            double odometer = Vehicle.vehicleList.Where(x => x.Id == vehicleId).Select(f => f.OdometerReading).FirstOrDefault();

            this.vehicleId = vehicleId;
            lastServiceOdometerKm = odometer;
            serviceCount = GetServiceCount() + 1;
            lastServiceDate = DateTime.Now;
        }
        /// <summary>
        /// RETURN HOW MANY KM SINCE LAST SERVICE
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static double GetKmSinceLastService(Vehicle v)
        {
            return (v.OdometerReading - serviceList.Where(x => x.vehicleId == v.Id).LastOrDefault().lastServiceOdometerKm);
        }

        /// <summary>
        /// CREATE A NEW SERVICE TO THE VEHICLE
        /// </summary>
        /// <param name="vehicleId"></param>
        public static void recordService(int vehicleId)
        {
            List<Service> sList = serviceList;
            sList.Add(new Service(vehicleId));

            JsonData.Save(sList);
        }

        /// <summary>
        /// RETURN HOW MANY SERVICES THE VEHICLE HAD
        /// </summary>
        /// <param name="vId"></param>
        /// <returns></returns>
        public int GetServiceCount(int vId = 0)
        {
            var count = 0;

            var sList = serviceList.Where(x => x.vehicleId == ((vId == 0) ? vehicleId : vId)).ToList();

            if (sList.Count > 0)
            {
                count = sList.Max(m => m.serviceCount);
            }

            return count;
        }

        /// <summary>
        /// RETURN IF THE VEHICE IS DUE TO A SERVICE
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public static bool isVehicleDueToService(int vehicleId)
        {
            double nextOdoService = SERVICE_KILOMETER_LIMIT;

            double actualOdo = Vehicle.vehicleList.Where(x => x.Id == vehicleId).Select(x => x.OdometerReading).FirstOrDefault();

            List<Service> sList = serviceList.Where(x => x.vehicleId == vehicleId).ToList();

            if (sList.Count > 0)
            {
                nextOdoService += sList.Max(x => x.lastServiceOdometerKm);
            }

            return (actualOdo > nextOdoService);
        }
    }
}
