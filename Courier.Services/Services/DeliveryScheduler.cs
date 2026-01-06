using Core.Entities;
using Courier.Services.Interfaces;
using Courier.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier.Services.Services
{
    public class DeliveryScheduler : IDeliveryScheduler
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

        public void ScheduleDeliveries(List<Package> packages, ICostCalculator calculator)
        {
            if (packages == null || packages.Count == 0)
                throw new ArgumentException("Package list cannot be empty.");


            foreach (var pkg in packages)
            {
                try
                {
                    calculator.Calculate(pkg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Skipping package {pkg?.Id}: {ex.Message}");
                    continue;
                }
            }

            try
            {
                var tempPackages = packages.ToList();
                var tempshipmentCombination = ShipmentCombination.GetAllCombinationsUpToWeight(tempPackages, _maxWeight);
                // Initialize vehicles in a priority queue (min-heap by AvailableTime)
                var vehicles = new PriorityQueue<Vehicle, double>();

                for (int i = 1; i <= _vehicleCount; i++)
                    vehicles.Enqueue(new Vehicle { Id = i }, 0);

                while (tempPackages.Any())
                {
                    var shipment = new List<Package>();
                    double currentWeight = 0;

                    // Get best shipment combination as per the criteria
                    var shipmentCombination = ShipmentCombination.GetAllCombinationsUpToWeight(tempPackages, _maxWeight)
                    .OrderByDescending(s => s.Count)
                    .Select(s => s)
                    .Where(s => s.Sum(p => p.Weight) <= _maxWeight)
                    .OrderByDescending(s => s.Sum(p => p.Weight))
                    .ThenBy(s => s.Max(p => p.Distance))
                    .FirstOrDefault();

                    foreach (var pkg in shipmentCombination)
                    {
                        if (currentWeight + pkg.Weight <= _maxWeight)
                        {
                            shipment.Add(pkg);
                            currentWeight += pkg.Weight;
                            tempPackages.RemoveAll(sp => sp.Id == pkg.Id);
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
            catch (Exception ex)
            {
                Console.WriteLine($"Scheduling error: {ex.Message}");
                throw; // rethrow for higher-level handling

            }
        }
    }
}
