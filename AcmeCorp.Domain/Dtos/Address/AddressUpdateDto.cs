﻿using System.ComponentModel.DataAnnotations;

namespace AcmeCorp.Domain.Dtos.Address
{
    public class AddressUpdateDto : AddressCreateDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be greater than 0.")]
        public int Id { get; set; }

        public bool? Archive { get; set; } = false;

    }
}
