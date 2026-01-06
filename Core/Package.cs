namespace Core.Entities
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
            return $"{Id} | Discount: {Math.Round(Discount, 6)} | TotalCost: {TotalCost} | ETA: {DeliveryTime} hrs";
        }
    }

}
