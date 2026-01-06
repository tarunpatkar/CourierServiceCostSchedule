using Core.Entities;
using Courier.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier.Services.Services
{
    public class Offer001 : IOffer
    {
        public double GetDiscount(double deliveryCost)
        {
            return deliveryCost * 0.10;
        }

        public bool IsApplicable(Package pkg)
        {
            return pkg.Distance <= 200 && pkg.Weight >= 70 && pkg.Weight <= 200;
        }
    }
    public class Offer002 : IOffer
    {
        public double GetDiscount(double deliveryCost)
        {
            return deliveryCost * 0.07;
        }

        public bool IsApplicable(Package pkg)
        {
            return pkg.Distance <= 150 && pkg.Weight >= 50 && pkg.Weight <= 150;
        }
    }
    public class Offer003 : IOffer
    {
        public double GetDiscount(double deliveryCost)
        {
            return deliveryCost * 0.05;
        }

        public bool IsApplicable(Package pkg)
        {
            return pkg.Distance <= 250 && pkg.Weight >= 10 && pkg.Weight <= 150;
        }
    }
}
