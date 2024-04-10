using System.ComponentModel.DataAnnotations;

namespace AcmeCorp.Domain.Dtos.Customer
{
    public class CustomerCreateDto
    {

        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
