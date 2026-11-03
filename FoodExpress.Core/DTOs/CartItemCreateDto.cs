using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace FoodExpress.Core.DTOs
{
    public class CartItemCreateDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int MenuItemId { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}
