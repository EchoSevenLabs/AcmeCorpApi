using AcmeCorp.Domain.Interfaces.Models;
using System.ComponentModel.DataAnnotations;

namespace AcmeCorp.Domain.Models
{
    public class Customer : IAuditable
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string LastName { get; set; }

        public bool Archive { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
