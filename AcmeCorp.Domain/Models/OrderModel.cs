using AcmeCorp.Domain.Enums;
using AcmeCorp.Domain.Interfaces.Models;
using System.ComponentModel.DataAnnotations;

namespace AcmeCorp.Domain.Models
{
    public class Order : IAuditable
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.InProgress;

        public string ShipMethod { get; set; }

        public DateTime? DateShipped { get; set; }

        public bool Archive { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer? Customer { get; set; } = null!;
    }
}
