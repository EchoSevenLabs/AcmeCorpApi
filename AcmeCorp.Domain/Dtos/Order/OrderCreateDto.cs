using AcmeCorp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AcmeCorp.Domain.Dtos.Order
{
    public class OrderCreateDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be greater than 0.")]
        public int CustomerId { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.InProgress;

        [Required]
        public string ShipMethod { get; set; }

        public DateTime? DateShipped { get; set; }
    }
}
