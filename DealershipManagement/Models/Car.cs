using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DealershipManagement.Models
{
    public class Car
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [Required]
        [BsonElement("VIN")]
        public string VIN { get; set; } = string.Empty;

        [Required]
        [BsonElement("Make")]
        public string Make { get; set; } = string.Empty;

        [Required]
        [BsonElement("Model")]
        public string Model { get; set; } = string.Empty;

        [Required]
        [BsonElement("Year")]
        public int Year { get; set; }

        [Required]
        [BsonElement("StockNumber")]
        public string StockNumber { get; set; } = string.Empty;

        [BsonElement("BuyPrice")]
        public decimal BuyPrice { get; set; }

        [BsonElement("BuyDate")]
        public DateTime? BuyDate { get; set; }

        [BsonElement("SellPrice")]
        public decimal SellPrice { get; set; }

        [BsonElement("SellDate")]
        public DateTime? SellDate { get; set; }

        [BsonElement("Tax")]
        public decimal Tax { get; set; }

        [BsonElement("FinalSellPrice")]
        public decimal FinalSellPrice { get; set; }

        [BsonElement("FinalBuyPrice")]
        public decimal FinalBuyPrice { get; set; }

        [BsonElement("MaintenanceLogs")]
        public List<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();

        public bool IsSold => SellDate.HasValue;
        public string Status => IsSold ? "Sold" : "Available";
    }
}
