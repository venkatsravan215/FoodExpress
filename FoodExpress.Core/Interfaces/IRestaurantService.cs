using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.DTOs;

namespace FoodExpress.Core.Interfaces
{
    public interface IRestaurantService
    {
        Task<IEnumerable<RestaurantResponseDto>> GetAllAsync();

        Task<RestaurantResponseDto?> GetByIdAsync(int id);

        Task<RestaurantResponseDto> CreateAsync(
            RestaurantCreateDto restaurantDto);

        Task UpdateAsync(
            int id,
            RestaurantUpdateDto restaurantDto);
        Task DeleteAsync(int id);
    }
}
