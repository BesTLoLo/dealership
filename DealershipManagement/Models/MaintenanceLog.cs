using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DealershipManagement.Models
{
    public class MaintenanceLog
    {
        [BsonElement("Id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [BsonElement("PartNumber")]
        public string PartNumber { get; set; } = string.Empty;

        [Required]
        [BsonElement("PartDescription")]
        public string PartDescription { get; set; } = string.Empty;

        [Required]
        [BsonElement("ShopName")]
        public string ShopName { get; set; } = string.Empty;

        [Required]
        [BsonElement("Price")]
        public decimal Price { get; set; }

        [BsonElement("FinalPrice")]
        public decimal FinalPrice { get; set; }

        [Required]
        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("InvoiceFilePath")]
        public string InvoiceFilePath { get; set; } = string.Empty;
    }
}
