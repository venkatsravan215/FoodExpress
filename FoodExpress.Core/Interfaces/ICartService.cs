
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodExpress.Core.DTOs;

namespace FoodExpress.Core.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartResponseDto>> GetAllAsync();

        Task<CartResponseDto?> GetByIdAsync(int id);

        Task<CartResponseDto?> GetByUserIdAsync(int userId);

        Task<CartResponseDto> CreateAsync(CartCreateDto cartDto);

        Task<CartResponseDto> AddItemAsync(
            int cartId, int userId, int menuItemId, int quantity);

        Task<CartResponseDto> UpdateItemQuantityAsync(
            int cartId, int userId, int menuItemId, int quantity);

        Task DeleteAsync(int id);
    }
}
