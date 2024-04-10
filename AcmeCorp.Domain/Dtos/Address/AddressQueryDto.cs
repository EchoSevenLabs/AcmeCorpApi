using AcmeCorp.Domain.Enums;

namespace AcmeCorp.Domain.Dtos.Address
{
    public class AddressQueryDto : AddressCreateDto
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public AddressType Type { get; set; }

        public string Street1 { get; set; }

        public string? Street2 { get; set; }

        public string City { get; set; }

        public string StateProvince { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public bool Archive { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
