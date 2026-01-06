using System;
using System.Collections.Generic;

namespace CourierServiceCost_Schedule
{
    public class Package
    {
        public string Id { get; set; }
        public double Weight { get; set; }
        public double Distance { get; set; }
        public string OfferCode { get; set; }
        public double Discount { get; set; }
        public double TotalCost { get; set; }
        public double DeliveryTime { get; set; }

        public override string ToString()
        {
            return $"{Id} | Discount: {Discount} | TotalCost: {TotalCost} | ETA: {DeliveryTime} hrs";
        }
    }
    public class CourierService
    {
        private readonly double _baseCost;
        public CourierService(double baseCost)
        {
            _baseCost = baseCost;
        }

        public void CalculateCost(Package pkg)
        {
            double deliveryCost = _baseCost + (pkg.Weight * 10) + (pkg.Distance * 5);
            double discount = 0;

            switch (pkg.OfferCode)
            {
                case "OFR001":
                    if (pkg.Distance <= 200 && pkg.Weight >= 70 && pkg.Weight <= 200)
                        discount = deliveryCost * 0.10;
                    break;
                case "OFR002":
                    if (pkg.Distance <= 150 && pkg.Weight >= 50 && pkg.Weight <= 150)
                        discount = deliveryCost * 0.07;
                    break;
                case "OFR003":
                    if (pkg.Distance <= 250 && pkg.Weight >= 10 && pkg.Weight <= 150)
                        discount = deliveryCost * 0.05;
                    break;
            }

            pkg.Discount = discount;
            pkg.TotalCost = deliveryCost - discount;
        }



    }

}
