using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.DTOs;

namespace FoodExpress.Core.Interfaces
{
    public interface ICartItemService
    {
        Task<CartResponseDto> AddItemAsync(
            int cartId,
            CartItemCreateDto itemDto);

        Task UpdateQuantityAsync(
            int cartId,
            int cartItemId,
            int quantity);

        Task RemoveItemAsync(
            int cartId,
            int cartItemId);
    }
}
