using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using FoodExpress.Core.Enums;

namespace FoodExpress.Core.DTOs
{
    public class PaymentCreateDto
    {
        [Range(1, int.MaxValue)]
        public int OrderId { get; set; }

        [Range(0.01, 1000000)]
        public decimal Amount { get; set; }

        [Required]
        public PaymentMethod Method { get; set; }
    }
}

