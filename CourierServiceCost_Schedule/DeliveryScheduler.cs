
using System.Linq;

namespace CourierServiceCost_Schedule
{
    public class Vehicle
    {
        public int Id { get; set; }
        public double AvailableTime { get; set; } = 0;
    }

    public class DeliveryScheduler
    {
        private readonly int _vehicleCount;
        private readonly double _maxSpeed;
        private readonly double _maxWeight;

        public DeliveryScheduler(int vehicleCount, double maxSpeed, double maxWeight)
        {
            _vehicleCount = vehicleCount;
            _maxSpeed = maxSpeed;
            _maxWeight = maxWeight;
        }

        public void ScheduleDeliveries(List<Package> packages)
        {

            // First calculate costs
            var courier = new CourierService(100); // Example base cost
            foreach (var pkg in packages)
                courier.CalculateCost(pkg);

            // Sort packages by weight (desc)
            var sortedPackages = packages.ToList();//.OrderByDescending(p => p.Weight).ToList();

            // Initialize vehicles in a priority queue (min-heap by AvailableTime)
            var vehicles = new PriorityQueue<Vehicle, double>();
            for (int i = 1; i <= _vehicleCount; i++)
                vehicles.Enqueue(new Vehicle { Id = i }, 0);

            while (sortedPackages.Any())
            {
                var shipment = new List<Package>();
                double currentWeight = 0;
                var shipmentCombi = ShipmentCombination.GetAllCombinationsUpToWeight(sortedPackages, _maxWeight)
                .OrderByDescending(s => s.Count)
                .Select(s => s)
                .Where(s => s.Sum(p => p.Weight) <= _maxWeight)
                .OrderByDescending(s => s.Sum(p => p.Weight))
                .ThenBy(s => s.Max(p => p.Distance))
                .FirstOrDefault();
                foreach (var pkg in shipmentCombi)
                {
                    if (currentWeight + pkg.Weight <= _maxWeight)
                    {
                        shipment.Add(pkg);
                        currentWeight += pkg.Weight;
                        sortedPackages.RemoveAll(sp => sp.Id == pkg.Id);
                    }
                }

                vehicles.TryDequeue(out Vehicle vehicle, out double availableTime);
                foreach (var spkg in shipment)
                {
                    double travelTime = spkg.Distance / _maxSpeed;
                    spkg.DeliveryTime = availableTime + travelTime;
                }
                double maxDistance = shipment.Max(p => p.Distance);
                double roundTripTime = (maxDistance / _maxSpeed) * 2;
                vehicle.AvailableTime = availableTime + roundTripTime;
                vehicles.Enqueue(vehicle, vehicle.AvailableTime);

            }

        }
    }

}
