using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.Enums;

namespace FoodExpress.Core.DTOs
{
    public class OrderResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int AddressId { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal SubTotal { get; set; }

        public decimal DeliveryFee { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public List<OrderItemResponseDto> Items { get; set; }
            = new List<OrderItemResponseDto>();
    }
}
