using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.Enums;

namespace FoodExpress.Core.DTOs
{
    public class PaymentResponseDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod Method { get; set; }

        public PaymentStatus Status { get; set; }

        public string TransactionReference { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
