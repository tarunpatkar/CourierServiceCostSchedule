using Core.Entities;
using Courier.Services.Interfaces;
using Courier.Services.Services;

try
{
    const double maximumWeightAllowed = 200;
    var offers = new Dictionary<string, IOffer>
        {
            { "OFR001", new Offer001() },
            { "OFR002", new Offer002() },
            { "OFR003", new Offer003() }
        };

    var calculator = new CostCalculator(100, maximumWeightAllowed, offers);

    var packages = new List<Package>
        {
            new Package { Id = "PKG1", Weight = 50, Distance = 30, OfferCode = "OFR001" },
            new Package { Id = "PKG2", Weight = 75, Distance = 125, OfferCode = "OFFR0008" },
            new Package { Id = "PKG3", Weight = 175, Distance = 100, OfferCode = "OFR003" },
            new Package { Id = "PKG4", Weight = 110, Distance = 60, OfferCode = "OFR002" },
            new Package { Id = "PKG5", Weight = 155, Distance = 95, OfferCode = "NA" },
        };

    var scheduler = new DeliveryScheduler(vehicleCount: 2, maxSpeed: 70, maxWeight: maximumWeightAllowed);
    scheduler.ScheduleDeliveries(packages, calculator);

    foreach (var pkg in packages)
    {
        Console.WriteLine(pkg.ToString());
    }

} 
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}