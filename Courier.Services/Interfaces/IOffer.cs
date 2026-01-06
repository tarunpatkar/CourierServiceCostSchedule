using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier.Services.Interfaces
{
    public interface IOffer
    {
        bool IsApplicable(Package pkg);
        double GetDiscount(double deliveryCost);

    }
}
