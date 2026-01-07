using Core.Entities;
using Courier.Services.Interfaces;
using Courier.Services.Services;

namespace CourierService.UnitTest
{
    public class CourierServiceTests
    {
        [Fact]
        public void CalculateCost_ShouldNotApplyDiscount_WhenOfferInvalid()
        {
            var offers = new Dictionary<string, IOffer> { { "OFR001", new Offer001() } };
            var calculator = new CostCalculator(100, 200, offers);

            var pkg = new Package { Id = "PKG2", Weight = 20, Distance = 300, OfferCode = "OFR001" };
            calculator.Calculate(pkg);

            Assert.Equal(0, pkg.Discount);

        }

        [Fact]
        public void CalculateCost_ShouldApplyOffer001_WhenValid()
        {
            var offers = new Dictionary<string, IOffer> { { "OFR001", new Offer001() } };
            var calculator = new CostCalculator(100, 200, offers);

            var pkg = new Package { Id = "PKG1", Weight = 100, Distance = 150, OfferCode = "OFR001" };
            calculator.Calculate(pkg);

            Assert.True(pkg.Discount > 0);
            Assert.Equal(pkg.TotalCost, (100 + (100 * 10) + (150 * 5)) - pkg.Discount);
        }

        [Fact]
        public void Schedule_ShouldRespectMaxWeight()
        {
            const double maxWeight = 200;
            var offers = new Dictionary<string, IOffer>();
            var calculator = new CostCalculator(100, maxWeight, offers);

            var packages = new List<Package>
        {
            new Package { Id = "PKG1", Weight = 150, Distance = 100 },
            new Package { Id = "PKG2", Weight = 60, Distance = 100 },
            new Package { Id = "PKG3", Weight = 50, Distance = 100 }
        };

            var scheduler = new DeliveryScheduler(vehicleCount: 1, maxSpeed: 50, maxWeight: maxWeight);
            scheduler.ScheduleDeliveries(packages, calculator);

            var groupedShipments = packages.GroupBy(p => p.DeliveryTime).Select(g => g.Sum(p => p.Weight));
            foreach (var totalWeight in groupedShipments)
            {
                Assert.True(totalWeight <= 200, $"Shipment exceeded max weight: {totalWeight}");
            }
        }

        [Fact]
        public void Schedule_ShouldCalculateDeliveryTime()
        {
            var offers = new Dictionary<string, IOffer>();
            var calculator = new CostCalculator(100, 200, offers);

            var packages = new List<Package>
        {
            new Package { Id = "PKG1", Weight = 50, Distance = 100 }
        };

            var scheduler = new DeliveryScheduler(vehicleCount: 1, maxSpeed: 50, maxWeight: 200);
            scheduler.ScheduleDeliveries(packages, calculator);

            var pkg = packages.First();
            Assert.Equal(2, pkg.DeliveryTime); // 100km / 50kmph = 2 hrs
        }

        [Fact]
        public void Schedule_ShouldThrow_WhenPackageListIsEmpty()
        {
            var offers = new Dictionary<string, IOffer>();
            var calculator = new CostCalculator(100, 200, offers);
            var scheduler = new DeliveryScheduler(vehicleCount: 1, maxSpeed: 50, maxWeight: 200);

            Assert.Throws<ArgumentException>(() => scheduler.ScheduleDeliveries(new List<Package>(), calculator));
        }
        [Fact]
        public void CheckMaximumWeightLimit_ShouldThrow_WhenWeightExceedsLimit()
        {
            var offers = new Dictionary<string, IOffer>();
            var calculator = new CostCalculator(100, 200, offers);
            var pkg = new Package { Id = "PKG4", Weight = 250, Distance = 100 };
            calculator.Calculate(pkg);
            Assert.Equal(0, pkg.TotalCost);
        }


    }
}