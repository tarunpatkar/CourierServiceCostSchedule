using CourierServiceCost_Schedule;

var packages = new List<Package>
        {
            new Package { Id = "PKG1", Weight = 50, Distance = 30, OfferCode = "OFR001" },
            new Package { Id = "PKG2", Weight = 75, Distance = 125, OfferCode = "OFFR0008" },
            new Package { Id = "PKG3", Weight = 175, Distance = 100, OfferCode = "OFR003" },
            new Package { Id = "PKG4", Weight = 110, Distance = 60, OfferCode = "OFR002" },
            new Package { Id = "PKG5", Weight = 155, Distance = 95, OfferCode = "NA" },
        };

var scheduler = new DeliveryScheduler(vehicleCount: 2, maxSpeed: 70, maxWeight: 200);
scheduler.ScheduleDeliveries(packages);

foreach (var pkg in packages)
{
    Console.WriteLine(pkg.ToString());
}
