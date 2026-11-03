using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FoodExpress.Core.DTOs
{
    public class CartResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public List<CartItemResponseDto> Items { get; set; }
            = new List<CartItemResponseDto>();

        public decimal TotalAmount { get; set; }
    }
}

