using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodExpress.Core.DTOs
{
    public class AddressResponseDto
    {
        public int Id { get; set; }

        public string AddressLine { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string PostalCode { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string Label { get; set; } = string.Empty;

        public bool IsDefault { get; set; }

        public int UserId { get; set; }
    }
}

