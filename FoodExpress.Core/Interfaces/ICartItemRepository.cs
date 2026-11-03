using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.Entities;

namespace FoodExpress.Core.Interfaces
{
    public interface ICartItemRepository
    {
        Task<CartItem?> GetByIdAsync(int id);

        Task<CartItem?> GetByCartAndMenuItemAsync(
            int cartId,
            int menuItemId);

        Task<CartItem> AddAsync(CartItem cartItem);

        Task UpdateAsync(CartItem cartItem);

        Task DeleteAsync(int id);
    }
}

