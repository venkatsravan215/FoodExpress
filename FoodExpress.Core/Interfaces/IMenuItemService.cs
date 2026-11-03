using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.DTOs;

namespace FoodExpress.Core.Interfaces
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItemResponseDto>> GetAllAsync();

        Task<MenuItemResponseDto?> GetByIdAsync(int id);

        Task<MenuItemResponseDto> CreateAsync(
            MenuItemCreateDto menuItemDto);

        Task UpdateAsync(
            int id,
            MenuItemCreateDto menuItemDto);

        Task DeleteAsync(int id);
    }
}
