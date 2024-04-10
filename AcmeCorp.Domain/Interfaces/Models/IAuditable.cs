namespace AcmeCorp.Domain.Interfaces.Models
{
    public interface IAuditable
    {
        public bool Archive { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
