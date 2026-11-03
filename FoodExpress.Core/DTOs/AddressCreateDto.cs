using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace FoodExpress.Core.DTOs
{
    public class AddressCreateDto
    {
        [Required]
        [StringLength(200)]
        public string AddressLine { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string State { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Label { get; set; } = string.Empty;

        public bool IsDefault { get; set; }

        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
    }
}
