
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodExpress.Core.Entities;

namespace FoodExpress.Core.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllAsync();

        Task<Cart?> GetByIdAsync(int id);

        Task<Cart?> GetByUserIdAsync(int userId);

        Task<Cart> AddAsync(Cart cart);

        Task UpdateAsync(Cart cart);

        Task DeleteAsync(int id);

        Task<CartItem?> GetCartItemAsync(int cartId, int menuItemId);

        Task AddCartItemAsync(CartItem cartItem);

        Task UpdateCartItemAsync(CartItem cartItem);

        Task DeleteCartItemAsync(CartItem cartItem);
    }
}
