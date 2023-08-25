using System.ComponentModel.DataAnnotations;

namespace CustomerOrder.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public DateTime CreationDate { get; set; }

    }
}
