using Core.Entities;
using Courier.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier.Services.Services
{
    public class CostCalculator : ICostCalculator
    {
        private readonly double _baseCost;
        private readonly Dictionary<string, IOffer> _offers;

        public CostCalculator(double baseCost, Dictionary<string, IOffer> offers)
        {
            _baseCost = baseCost;
            _offers = offers;
        }
        public void Calculate(Package pkg)
        {
            try
            {

                ValidatePackage(pkg);
                double deliveryCost = _baseCost + (pkg.Weight * 10) + (pkg.Distance * 5);
                double discount = 0;
                if (pkg.OfferCode != null)
                {
                    if (_offers.ContainsKey(pkg.OfferCode) && _offers[pkg.OfferCode].IsApplicable(pkg))
                        discount = _offers[pkg.OfferCode].GetDiscount(deliveryCost);
                }
                pkg.Discount = discount;
                pkg.TotalCost = deliveryCost - discount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating cost for {pkg?.Id}: {ex.Message}");
                pkg.Discount = 0;
                pkg.TotalCost = 0; // fallback

            }
        }
        private void ValidatePackage(Package pkg)
        {
            if (pkg == null)
                throw new ArgumentNullException(nameof(pkg));
            if (pkg.Weight < 0)
                throw new ArgumentException("Package weight cannot be negative.");
            if (pkg.Distance < 0)
                throw new ArgumentException("Package distance cannot be negative.");
        }
    }
}
