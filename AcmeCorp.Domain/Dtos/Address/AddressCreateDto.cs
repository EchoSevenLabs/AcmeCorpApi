using AcmeCorp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AcmeCorp.Domain.Dtos.Address
{
    public class AddressCreateDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be greater than 0.")]
        public int CustomerId { get; set; }

        [Required]
        public AddressType Type { get; set; }

        [Required]
        public string Street1 { get; set; }

        public string? Street2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string StateProvince { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
